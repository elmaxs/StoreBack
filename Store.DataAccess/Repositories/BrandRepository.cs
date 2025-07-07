using Microsoft.EntityFrameworkCore;
using Store.Core.Abstractions.Repository;
using Store.Core.Models;
using Store.DataAccess.Entities;

namespace Store.DataAccess.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly OnlineStoreDbContext _context;

        public BrandRepository(OnlineStoreDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Create(Brand brand)
        {
            var brandEntity = new BrandEntity
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description
            };

            await _context.Brands.AddAsync(brandEntity);
            await _context.SaveChangesAsync();

            return brand.Id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.Brands.Where(b => b.Id == id).ExecuteDeleteAsync();

            return id;
        }

        public async Task<IEnumerable<Brand>>? GetAll()
        {
            var brandEntity = await _context.Brands.Include(b => b.Products).ThenInclude(p => p.Category).ToListAsync();
            if (brandEntity is null || !brandEntity.Any())
                return null;

            var brand = brandEntity.Select(b =>
            {
                var products = MappingProduct(b.Products, b.Id, b.Name);

                return Brand.CreateBrand(b.Id, b.Name, b.Description, products).Brand;
            }).ToList();

            return brand;
        }

        public async Task<Brand>? GetById(Guid id)
        {
            var brandEntity = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);

            var products = MappingProduct(brandEntity.Products, brandEntity.Id, brandEntity.Name);

            var brand = Brand.CreateBrand(brandEntity.Id, brandEntity.Name, brandEntity.Description, products).Brand;

            return brand;
        }

        public async Task<Guid> Update(Guid id, Brand brand)
        {
            await _context.Brands.Where(b => b.Id == id).ExecuteUpdateAsync(s => s
                .SetProperty(b => b.Name, b => brand.Name)
                .SetProperty(b => b.Description, b => brand.Description));

            return id;
        }

        private List<Product>? MappingProduct(ICollection<ProductEntity>? productsEntity, Guid brandId, string brandName)
        {
            if (productsEntity is null || !productsEntity.Any())
                return null;

            var products = productsEntity.Select(p => Product.CreateProduct(p.Id, p.Name, p.Description,
                p.ImageUrl, p.Price, p.CategoryId, p.Category.Name,
                brandId, brandName, p.StockQuantity).Product).ToList();

            return products;
        }
    }
}
