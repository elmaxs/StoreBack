using FluentAssertions;
using Moq;
using Store.Application.Abstractions.User;
using Store.Application.Services.User;
using Store.Core.Abstractions.Repository;
using Store.Core.Exceptions;
using Store.Core.Models;
using System;

namespace Store.API.Tests.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _productService = new ProductService(_productRepositoryMock.Object, _categoryServiceMock.Object);
        }

        #region GetProductById
        [Fact]
        public async Task GetProductById_ShouldThrowValidationException_WhenIdIsEmpty()
        {
            //Arrange
            //Підготовка: створення об'єктів, підстановка моків, налаштування вхідних даних.
            Guid empty = Guid.Empty;

            //Act
            //Виклик методу, який тестується.
            Func<Task> act = async () => await _productService.GetProductById(empty);

            //Assert
            //Перевірка, чи результат відповідає очікуваному
            await act.Should().ThrowAsync<ValidationException>().WithMessage(ErrorMessages.GuidCannotBeEmpty);
        }

        [Fact]
        public async Task GetProductById_ShouldThrowNotFound_WhenProductIsNull()
        {
            //Arrange
            var id = Guid.NewGuid();

            //Act
            Func<Task> act = async () => await _productService.GetProductById(id);

            //Assert
            await act.Should().ThrowAsync<NotFound>().WithMessage(ErrorMessages.ProductNotFound);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnDTO_WhenProductExists()
        {
            //Arrange
            //Підготовка: створення об'єктів, підстановка моків, налаштування вхідних даних.
            var id = Guid.NewGuid();
            var product = Product.CreateProduct(id, "Apple", "apple test desc", "testAppleImg", 10, new Guid(), "Fruit",
                new Guid(), "Fruit cool", 10, 1).Product;

            _productRepositoryMock.Setup(r => r.GetById(id)).ReturnsAsync(product);

            //Act
            //Виклик методу, який тестується.
            var result = await _productService.GetProductById(id);

            //Assert
            //Перевірка, чи результат відповідає очікуваному
            result.Id.Should().Be(id);
            result.Name.Should().Be("Apple");
            result.Description.Should().Be("apple test desc");
            result.ImageURL.Should().Be("testAppleImg");
            result.Price.Should().Be(10);
            result.CategoryName.Should().Be("Fruit");

            //Arrange

            //Act

            //Assert
        }
        #endregion

        #region IsAvailableQuantity
        [Fact]
        public async Task IsAvailableQuantity_ShouldThrowNotFound_WhenProductIsNull()
        {
            //Arrange
            var id = new Guid();
            var quantity = 10;

            //Act
            Func<Task> act = async () => await _productService.IsAvailableQuantity(id, quantity);

            //Assert
            await act.Should().ThrowAsync<NotFound>().WithMessage(ErrorMessages.ProductNotFound);
        }

        [Fact]
        public async Task IsAvailableQuantity_ShouldReturnTrue_WhenAvailableQuantityMoreThanQuantity()
        {
            //Arrange
            var id = Guid.NewGuid();
            var quantity = 5;
            var product = Product.CreateProduct(id, "Apple", "apple test desc", "testAppleImg", 10, new Guid(), "Fruit",
                new Guid(), "Fruit cool", 10, 1).Product;

            _productRepositoryMock.Setup(r => r.GetById(id)).ReturnsAsync(product);

            //Act
            var result = await _productService.IsAvailableQuantity(id, 5);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsAvailableQuantity_ShouldReturnFalse_WhenNotAvailableQuantityMoreThanQuantity()
        {
            //Arrange
            var id = Guid.NewGuid();
            var product = Product.CreateProduct(id, "Apple", "apple test desc", "testAppleImg", 10, new Guid(), "Fruit",
                new Guid(), "Fruit cool", 10, 1).Product;

            _productRepositoryMock.Setup(r => r.GetById(id)).ReturnsAsync(product);

            //Act
            var result = await _productService.IsAvailableQuantity(id, 10);

            //Assert
            result.Should().BeFalse();
        }
        #endregion

        #region GetProductsBySearch
        [Fact]
        public async Task GetProductsBySearch_ShouldReturnNewList_WhenSearchIsEmpty()
        {
            //Arrange
            var search = string.Empty;

            //Act
            var result = await _productService.GetProductsBySearch(search);

            //Assert
            result.Should().BeEmpty();

        }

        [Fact]
        public async Task GetProductsBySearch_ShouldReturnDTO_ProductBySearchIsExists()
        {
            //Arrange
            var search = "pp";
            var id = Guid.NewGuid();
            var product = Product.CreateProduct(id, "Apple", "apple test desc", "testAppleImg", 10,
                new Guid(), "Fruit", new Guid(), "Fruit cool", 10, 1).Product;

            //Act
            var result = await _productService.GetProductsBySearch(search);

            //Assert
            product.Id.Should().Be(id);
            product.Name.Should().Be("Apple");
        }
        #endregion

        #region GetProducts
        [Fact]
        public async Task GetProducts_ShouldThrowValidationException_WhenIdIsEmpty()
        {
            //Arrange
            var empty = Guid.Empty;

            //Act
            Func<Task> act = async () => await _productService.GetProducts(empty);

            //Assert
            await act.Should().ThrowAsync<ValidationException>().WithMessage(ErrorMessages.GuidCannotBeEmpty);
        }

        [Fact]
        public async Task GetProducts_ShouldNotFound_WhenProductsIsNullOrEmpty()
        {
            //Arrange
            var catId = Guid.NewGuid();

            //Act
            Func<Task> act = async () => await _productService.GetProducts(catId);

            //Assert
            await act.Should().ThrowAsync<NotFound>().WithMessage(ErrorMessages.ProductNotFound);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnDTO_WhenProductsExists()
        {
            //Arrange
            var productId = Guid.NewGuid();
            var catId = Guid.NewGuid();
            var products = new List<Product> { Product.CreateProduct(productId, "Apple", "Desc", "ImageURL", 10, catId,
                "CatName", new Guid(), "BrandName", 10, 2).Product};

            _categoryServiceMock.Setup(c => c.GetAllNestedCategoryIdsAndNames(catId))
                .ReturnsAsync(new List<(Guid, string)> { (catId, "CatName") });

            _productRepositoryMock.Setup(r => r.GetByCategoryIds(It.Is<IEnumerable<Guid>>(ids => ids.Contains(catId))))
            .ReturnsAsync(products);

            //Act
            var result = await _productService.GetProducts(catId);

            //Assert
            result.First().Id.Should().Be(productId);
            result.First().CategoryId.Should().Be(catId);
            result.First().Name.Should().Be("Apple");
        }
        #endregion

        #region GetFilteredProducts


        #endregion

        #region GetCountPages

        [Fact]
        public async Task GetCountPages_ShouldReturnCountPage()
        {
            //Arrange
            var categoryId = Guid.NewGuid();

            var product = Product.CreateProduct(Guid.NewGuid(), "TestProd", "TestDescProd", "TestImgProd", 100.5M, categoryId,
                "TestCatName", Guid.NewGuid(), "TestBrandName", 10, 2).Product;

            _categoryServiceMock.Setup(c => c.GetAllNestedCategoryIdsAndNames(categoryId))
                .ReturnsAsync(new List<(Guid, string)> { (categoryId, product.CategoryName) });

            _productRepositoryMock.Setup(p => p.GetByCategoryIds(It.Is<IEnumerable<Guid>>(ids => ids.Contains(categoryId))))
                .ReturnsAsync(new List<Product> { product });

            //Act
            var result = await _productService.GetCountPages(categoryId);

            //Assert
            result.Should().Be(1);
        }

        #endregion
        //Arrange
        //Підготовка: створення об'єктів, підстановка моків, налаштування вхідних даних.
        //Act
        //Виклик методу, який тестується.
        //Assert
        //Перевірка, чи результат відповідає очікуваному
    }
}