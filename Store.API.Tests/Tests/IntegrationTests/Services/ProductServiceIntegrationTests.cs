using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Store.Application.Services.User;
using Store.DataAccess;
using Store.DataAccess.Entities;
using Store.DataAccess.Repositories;

namespace Store.API.Tests.Tests.IntegrationTests.Services
{
    public class ProductServiceIntegrationTests
    {
        private readonly ProductService _productService;
        private readonly OnlineStoreDbContext _dbContext;

        public ProductServiceIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<OnlineStoreDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            _dbContext = new OnlineStoreDbContext(options);

            var productRepo = new ProductRepository(_dbContext);

            var categoryService = new CategoryService(new CategoryRepository(_dbContext), productRepo);

            _productService = new ProductService(productRepo, categoryService);
        }

        #region GetProductsBySearch

        [Fact]
        public async Task GetProductsBySearch_ReturnsMatchingProducts()
        {
            //Arrange

            var category = new CategoryEntity { Id = Guid.NewGuid(), Name = "ElectronicsTest", ParentCategoryId = null };
            var brand = new BrandEntity { Id = Guid.NewGuid(), Name = "SmartphoneTest", Description = "Test" };
            var product = new ProductEntity
            {
                Id = Guid.NewGuid(),
                Name = "SamsungTest",
                CategoryId = category.Id,
                BrandId = brand.Id,
                Description = "TestDesc",
                ImageUrl = "testImg",
                Price = 100.50M,
                StockQuantity = 10,
                ReservedQuantity = 2
            };

            await _dbContext.Categories.AddAsync(category);
            await _dbContext.Brands.AddAsync(brand);
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            //Act
            var result = await _productService.GetProductsBySearch("Sam");

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.First().Name.Should().Be("SamsungTest");
        }
        #endregion
    }
}
