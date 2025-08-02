using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Store.Application.Services.User;
using Store.Core.Models;
using Store.DataAccess;
using Store.DataAccess.Entities;
using Store.DataAccess.Repositories;

namespace Store.API.Tests.Tests.IntegrationTests.Services
{
    public class ProductServiceIntegrationTests
    {
        //private readonly ProductService _productService;
        //private readonly OnlineStoreDbContext _dbContext;

        //public ProductServiceIntegrationTests()
        //{
        //    var options = new DbContextOptionsBuilder<OnlineStoreDbContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        //    _dbContext = new OnlineStoreDbContext(options);

        //    var productRepo = new ProductRepository(_dbContext);

        //    var categoryService = new CategoryService(new CategoryRepository(_dbContext), productRepo);

        //    _productService = new ProductService(productRepo, categoryService);
        //}

        private ProductEntity Product;
        private CategoryEntity Category;
        private BrandEntity Brand;

        private void InitialDefaultProps()
        {
            Category = new CategoryEntity { Id = Guid.NewGuid(), Name = "TestCat" };
            Brand = new BrandEntity { Id = Guid.NewGuid(), Name = "TestBrand", Description = "TestDescBrand" };
            Product = new ProductEntity
            {
                Id = Guid.NewGuid(),
                Name = "TestProd",
                Description = "TestDescProd",
                ImageUrl = "TestImgProd",
                CategoryId = Category.Id,
                BrandId = Brand.Id
            };
        }

        private ProductService CreateServiceWithDb(out OnlineStoreDbContext context)
        {
            var option = new DbContextOptionsBuilder<OnlineStoreDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            context = new OnlineStoreDbContext(option);

            var productRepo = new ProductRepository(context);
            var categoryRepo = new CategoryRepository(context);
            var categoryService = new CategoryService(categoryRepo, productRepo);

            return new ProductService(productRepo, categoryService);
        }

        private class InitialProps
        {
            public CategoryEntity Category { get; }
            public BrandEntity Brand { get; }
            public ProductEntity Product { get; }

            public InitialProps()
            {
                Category = new CategoryEntity { Id = Guid.NewGuid(), Name = "TestCat" };
                Brand = new BrandEntity { Id = Guid.NewGuid(), Name = "TestBrand", Description = "TestDescBrand" };
                Product = new ProductEntity { Id = Guid.NewGuid(), Name = "TestProd", Description = "TestDescProd", 
                    ImageUrl = "TestImgProd", CategoryId = Category.Id, BrandId = Brand.Id};
            }
        }

        #region GetProductsBySearch

        [Fact]
        public async Task GetProductsBySearch_ReturnsMatchingProducts()
        {
            //Arrange
            var _productService = CreateServiceWithDb(out var _dbContext);

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

        #region IsAvailableQuantity

        [Fact]
        public async Task IsAvailableQuantity_ReturnTrueIfAvailable()
        {
            //Arrange
            var _productService = CreateServiceWithDb(out var _dbContext);

            var category = new CategoryEntity { Id = Guid.NewGuid(), Name = "TestCat" };
            var brand = new BrandEntity { Id = Guid.NewGuid(), Name = "TestBrand", Description = "testDescBr" };
            var product = new ProductEntity
            {
                Id = Guid.NewGuid(),
                Name = "TestProd",
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
            var result = await _productService.IsAvailableQuantity(product.Id, 5);

            //Assert
            result.Should().BeTrue();
        }

        #endregion

        #region GetProductById

        [Fact]
        public async Task GetProductById_ReturnProduct_WhenIdEquals()
        {
            //Arrange 
            var _productService = CreateServiceWithDb(out var _dbContext);

            var initial = new InitialProps();

            await _dbContext.Categories.AddAsync(initial.Category);
            await _dbContext.Brands.AddAsync(initial.Brand);
            await _dbContext.Products.AddAsync(initial.Product);

            await _dbContext.SaveChangesAsync();

            //Act
            var result = await _productService.GetProductById(initial.Product.Id);

            //Assert 
            result.Name.Should().Be(initial.Product.Name);
        }

        #endregion

        #region GetProducts

        [Fact]
        public async Task GetProducts_ReturnListProducts()
        {
            //Arrange
            var _productService = CreateServiceWithDb(out var _dbContext);

            InitialDefaultProps();

            await _dbContext.Categories.AddAsync(Category);
            await _dbContext.Brands.AddAsync(Brand);
            await _dbContext.Products.AddAsync(Product);

            await _dbContext.SaveChangesAsync();

            //Act
            var result = await _productService.GetProducts(Category.Id);

            //Assert
            result.Should().NotBeEmpty();
            result.First().Id.Should().Be(Product.Id);
        }
        #endregion
    }
}
