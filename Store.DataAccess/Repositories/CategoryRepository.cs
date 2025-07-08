using Microsoft.EntityFrameworkCore;
using Store.Core.Abstractions.Repository;
using Store.Core.Models;
using Store.DataAccess.Entities;

namespace Store.DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly OnlineStoreDbContext _context;

        public CategoryRepository(OnlineStoreDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Create(Category category)
        {
            var categoryEntity = new CategoryEntity
            {
                Id = category.Id,
                ParentCategoryId = category.ParentCategoryId,
                Name = category.CategoryName
            };

            await _context.Categories.AddAsync(categoryEntity);
            await _context.SaveChangesAsync();

            return category.Id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.Categories.Where(c => c.Id == id).ExecuteDeleteAsync();

            return id;
        }

        public async Task<IEnumerable<Category>> GetMains()
        {
            var categoriesEntity = await _context.Categories.Include(c => c.Products).ThenInclude(p => p.Brand)
                .Include(c => c.Subcategories).Where(c => c.ParentCategoryId == null).ToListAsync();
            if (categoriesEntity is null)
                return null;

            //var categories = categoriesEntity.Select(c => Category.CreateCategory(c.Id, c.Name, c.ParentCategoryId,
            //    0, c.Products, c.Subcategories).Category).ToList();

            return categoriesEntity.Select(c => MapCategory(c)).ToList();
        }

        public async Task<IEnumerable<Category>>? GetAll()
        {
            var categoriesEntity = await _context.Categories
                .Include(c => c.Products)
                .ThenInclude(p => p.Brand)
                .Include(c => c.Subcategories)
                .ToListAsync();

            if (categoriesEntity is null)
                return null;

            // Фільтруємо лише кореневі категорії (без батьків)
            var rootCategories = categoriesEntity.Where(c => c.ParentCategoryId == null);

            // Побудова всіх категорій рекурсивно
            //все категории пока в мапинге не нужны(я сам загружаю все сразу вначале. И подкатегории и основные)
            //если бы не грузил подкатегории, тогда пришлось бы отдавать все категории и там искать подкатегории через Where
            //крч если бы в entity у меня не хранился отдельно список подкатегорий, пришлось передавать все категории и там сортировать
            var result = rootCategories.Select(c => MapCategory(c)).ToList();//, categoriesEntity)).ToList();

            return result;
        }

        // Рекурсивна функція для створення Category з CategoryEntity
        private Category MapCategory(CategoryEntity entity)//, List<CategoryEntity> allCategories)
        {
            // Продукти поточної категорії
            var products = entity.Products.Select(p =>
                Product.CreateProduct(
                    p.Id, p.Name, p.Description, p.ImageUrl, p.Price,
                    p.CategoryId, entity.Name, p.BrandId, p.Brand.Name, p.StockQuantity
                ).Product).ToList();

            // Підкатегорії поточної категорії (рекурсивно)
            var subcategories = entity.Subcategories.Select(sub =>
                MapCategory(sub)).ToList();//, allCategories)).ToList();

            var (category, error) = Category.CreateCategory(
                entity.Id,
                entity.Name,
                entity.ParentCategoryId ?? Guid.Empty,
                products.Count,
                products,
                subcategories
            );

            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception($"Category creation error: {error}");
            }

            return category;
        }

        public async Task<Category>? GetById(Guid id)
        {
            var categoryEntity = await _context.Categories.Include(c => c.Products).ThenInclude(p => p.Brand)
                .Include(c => c.Subcategories).FirstOrDefaultAsync(c => c.Id == id);
            if (categoryEntity is null)
                return null;

            //// Додатково потрібно завантажити всі категорії для побудови дерева
            //var allCategories = await _context.Categories
            //    .Include(c => c.Products)
            //    .Include(c => c.Subcategories)
            //    .ToListAsync();

            var category = MapCategory(categoryEntity);//, allCategories);

            return category;
        }

        public async Task<Category> TreeSelected(Guid currentCategoryId)
        {
            var categoryEntity = _context.Categories.FirstOrDefault(c => c.Id == currentCategoryId);
            if (categoryEntity is null)
                return null;

            var category = MapCategory(categoryEntity);

            return category;
        }

        public async Task<int> GetCountProductInCategory(Guid categoryId)
        {
            var categoryEntity = await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == categoryId);
            if (categoryEntity is null)
                return 0;

            var count = categoryEntity.Products.Count();

            return count;
        }

        public async Task<Guid> Update(Guid id, Category category)
        {
            await _context.Categories.Where(c => c.Id == id).ExecuteUpdateAsync(s => s
                .SetProperty(c => c.ParentCategoryId, c => category.ParentCategoryId)
                .SetProperty(c => c.Name, c => category.CategoryName));

            return id;
        }

        public async Task<bool> IsExists(Guid id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }
    }
}
