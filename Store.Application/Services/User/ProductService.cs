using Store.Application.Abstractions.User;
using Store.Contracts.UserContracts.Request.ProductUserDTO;
using Store.Contracts.UserContracts.Response.CategoryUserDTO;
using Store.Contracts.UserContracts.Response.ProductUserDTO;
using Store.Core.Abstractions.Repository;
using Store.Core.Exceptions;
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

        /// <summary>
        /// Get products by category id.
        /// </summary>
        /// <param name="categoryId">Search by category id</param>
        /// <returns>If category is main parent, will returned all subcategories and his products</returns>
        /// <exception cref="ValidationException">Guid cant be empty</exception>
        /// <exception cref="NotFound">Products not found</exception>
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
                .Select(p => new ReadProductForCategoryDTO(p.Id, p.Name, p.ImageUrl, p.Price)).ToList()));
        }

        /// <summary>
        /// Filter for products
        /// </summary>
        /// <param name="filter">Object has filter props.</param>
        /// <returns>Filtered products by category id, order, page, page size.</returns>
        public async Task<IEnumerable<ReadProductDTO>> GetFilteredProductsAsync(ProductFilterParams filter)
        {
            var products = await _productRepository.GetFilteredProductsAsync(filter.CategoryId, filter.Order, 
                filter.Page, filter.PageSize);

            var productDTO = products.Select(p => new ReadProductDTO(p.Id, p.Name, p.CategoryName, p.ImageUrl, p.Price)).ToList();

            return productDTO;
        }

        public async Task<IEnumerable<IEnumerable<ReadProductByCategoryDTO>>> GetProductsForMainPage()
        {
            var popularCategoryId = new Guid[]
            {
                //main categories
                //Guid.Parse("4fbeeb7b-449b-4638-a2c0-d10213fc727f"),
                //Guid.Parse("c6e6d5e5-bd27-4fc2-85bf-a1876206799a"),
                //Guid.Parse("f809b7d4-2545-42a4-b463-544929c79462")
                Guid.Parse("e9c59552-5c0c-4b83-841c-a0e620d80868"),
                Guid.Parse("d38fa92e-72da-4b4f-90f5-476b21fefb30"),
                Guid.Parse("998ac30f-8553-493d-906c-b2351fdba1da")
            };

            var result = new List<List<ReadProductByCategoryDTO>>();
            foreach(var categoryId in popularCategoryId)
            {
                var products = await GetProductsByCategoryHierarchy(categoryId);
                if(products is not null)
                    result.Add(products.ToList());
            }

            return result;
        }

        //мб потом это переделать и юзать
        #region Get for main
        ///// <summary>
        ///// Get products for main page.
        ///// </summary>
        ///// <returns>Products for main page by popular category.</returns>
        ///// <exception cref="NotFound">Category not found</exception>
        //public async Task<IEnumerable<ReadProductMainPage>> GetProductsForMainPage()
        //{
        //    var categories = await _categoryService.GetCategoriesForMainPage();
        //    if (!categories.Any())
        //        throw new NotFound(ErrorMessages.CategoryNotFound);

        //    var result = new List<ReadProductMainPage>();

        //    foreach (var topCategory in categories)
        //    {
        //        var enrichedTop = await EnrichWithProducts(topCategory);
        //        result.Add(new ReadProductMainPage(enrichedTop));
        //    }

        //    return result;
        //}


        ////мб потом переделать и юзать этот метод
        ///// <summary>
        ///// Mapping for GetProductsForMainPage
        ///// </summary>
        ///// <param name="category">Current category</param>
        ///// <returns></returns>
        //private async Task<CategoriesForMainDTO> EnrichWithProducts(CategoriesForMainDTO category)
        //{
        //    if (category.Subcategories is null || !category.Subcategories.Any())
        //    {
        //        // финальная категория — получаем продукты
        //        var products = await _productRepository.GetByCategoryId(category.CategoryId);
        //        var productDtos = products?
        //            .Where(p => p != null)
        //            .Take(5)
        //            .Select(p => new ReadProductForCategoryDTO(p.Id, p.Name, p.ImageUrl, p.Price))
        //            .ToList();

        //        return category with { Products = productDtos };
        //    }
        //    else
        //    {
        //        // обходим только первую подкатегорию
        //        var sub = await EnrichWithProducts(category.Subcategories.First());
        //        return category with { Subcategories = new List<CategoriesForMainDTO> { sub } };
        //    }
        //}
        #endregion
    }

}

