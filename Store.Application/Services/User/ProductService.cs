using Store.Application.Abstractions.User;
using Store.Contracts.UserContracts.Request.ProductUserDTO;
using Store.Contracts.UserContracts.Response.ProductUserDTO;
using Store.Core.Abstractions.Repository;
using Store.Core.Exceptions;
using Store.Core.Models;
using ValidationException = Store.Core.Exceptions.ValidationException;

namespace Store.Application.Services.User
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryService _categoryService;

        public ProductService(IProductRepository productRepository, ICategoryService categoryService)
        {
            _productRepository = productRepository;
            _categoryService = categoryService;
        }

        public async Task<IEnumerable<ReadProductByCategoryDTO>>? GetProductsByCategoryHierarchy(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            var categoryIdsAndNames = await _categoryService.GetAllNestedCategoryIdsAndNames(categoryId);

            var categoryIds = categoryIdsAndNames.Select(c => c.Item1);

            var products = await _productRepository.GetByCategoryIds(categoryIds);
            if (products is null || !products.Any())
                throw new NotFound(ErrorMessages.ProductNotFound);

            return categoryIdsAndNames.Select(c => new ReadProductByCategoryDTO(c.Item1, c.Item2,
                products.Where(p => p.CategoryId == c.Item1)
                .Select(p => new ReadProductInByCategoryDTO(p.Id, p.Name, p.ImageUrl, p.Price)).ToList()));
        }

        public async Task<IEnumerable<ReadProductDTO>> GetFilteredProductsAsync(ProductFilterParams filter)
        {
            var products = await _productRepository.GetFilteredProductsAsync(filter.CategoryId, filter.Order, 
                filter.Page, filter.PageSize);

            var productDTO = products.Select(p => new ReadProductDTO(p.Id, p.Name, p.CategoryName, p.ImageUrl, p.Price)).ToList();

            return productDTO;
        }
    }
}
