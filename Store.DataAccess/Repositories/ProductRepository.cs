using Microsoft.EntityFrameworkCore;
using Store.Core.Abstractions.Repository;
using Store.Core.Models;
using Store.DataAccess.Entities;

namespace Store.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly OnlineStoreDbContext _context;

        public ProductRepository(OnlineStoreDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Create(Product product)
        {
            var productEntity = new ProductEntity
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                Name = product.Name,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                IsAvailable = product.IsAvailable
            };

            await _context.Products.AddAsync(productEntity);
            await _context.SaveChangesAsync();

            return product.Id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.Products.Where(p => p.Id == id).ExecuteDeleteAsync();

            return id;
        }

        public async Task<IEnumerable<Product>>? GetAll()
        {
            var productsEntity = await _context.Products.Include(p => p.Category).ToListAsync();
            if (productsEntity is null)
                return null;

            var products = productsEntity.Select(p => Product.CreateProduct(p.Id, p.Name, p.Description, p.ImageUrl, p.Price,
                p.CategoryId, p.Category.Name, p.StockQuantity).Product).ToList();

            return products;
        }

        public async Task<Product>? GetById(Guid id)
        {
            var productEntity = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (productEntity is null)
                return null;

            var product = Product.CreateProduct(productEntity.Id, productEntity.Name, productEntity.Description, productEntity.ImageUrl,
                productEntity.Price, productEntity.CategoryId, productEntity.Category.Name, productEntity.StockQuantity).Product;

            return product;
        }

        public async Task<IEnumerable<Product>>? GetByCategoryId(Guid categoryId)
        {
            var productsEntity = await _context.Products.Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category).ToListAsync();
            if (productsEntity is null)
                return null;

            var products = productsEntity.Select(p => Product.CreateProduct(p.Id, p.Name, p.Description, p.ImageUrl, p.Price,
                p.CategoryId, p.Category.Name, p.StockQuantity).Product);

            return products;
        }

        public async Task<Guid> Update(Guid id, Product product)
        {
            await _context.Products.Where(p => p.Id == id).ExecuteUpdateAsync(s => s
                .SetProperty(p => p.Name, p => product.Name)
                .SetProperty(p => p.Description, p => product.Description)
                .SetProperty(p => p.ImageUrl, p => product.ImageUrl)
                .SetProperty(p => p.Price, p => product.Price)
                .SetProperty(p => p.CategoryId, p => product.CategoryId)
                .SetProperty(p => p.StockQuantity, p => product.StockQuantity)
                .SetProperty(p => p.IsAvailable, p => product.IsAvailable));

            return id;
        }

        public async Task<List<Product>> GetFilteredProductsAsync(Guid? categoryId, string order, int page, int pageSize)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if(categoryId != null)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            if(order.ToLower() == "desc")
            {
                query = query.OrderByDescending(p => p.Price);
            }
            else
            {
                query = query.OrderBy(p => p.Price);
            }

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var productsEntity = await query.ToListAsync();

            return productsEntity.Select(p => Product.CreateProduct(p.Id, p.Name, p.Description, p.ImageUrl, p.Price,
                p.CategoryId, p.Category.Name, p.StockQuantity).Product).ToList();

            //return await MappingFiltered(await query.ToListAsync());
        }

        //private async Task<List<Product>> MappingFiltered(List<ProductEntity> productsEntity)
        //{
        //    return productsEntity.Select(p => Product.CreateProduct(p.Id, p.Name, p.Description, p.ImageUrl, p.Price, 
        //        p.CategoryId, p.Category.Name, p.StockQuantity).Product).ToList();
        //}
    }
}
