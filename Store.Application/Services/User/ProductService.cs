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

        public async Task<IEnumerable<ReadProductDTO>> GetProductsBySearch(string search)
        {
            if (string.IsNullOrEmpty(search))
                return new List<ReadProductDTO>();

            var products = await _productRepository.GetBySearch(search);

            return products.Select(p => new ReadProductDTO(p.Id, p.Name, p.BrandId, p.BrandName, p.CategoryName, p.CategoryId,
                p.ImageUrl, p.Description, p.AvailableQuantity, p.Price)).ToList();
        }

        public async Task<bool> IsAvailableQuantity(Guid productId, int quantity)
        {
            var product = await _productRepository.GetById(productId);
            if (product is null)
                throw new NotFound(ErrorMessages.ProductNotFound);

            if (product.IsAvailable && product.AvailableQuantity > quantity)
                return true;
            else
                return false;
        }

        public async Task<ReadProductDTO> GetProductById(Guid id)
        {
            if (id == Guid.Empty || id == new Guid())
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            var product = await _productRepository.GetById(id);
            if (product is null)
                throw new NotFound(ErrorMessages.ProductNotFound);

            var productDTO = new ReadProductDTO(product.Id, product.Name, product.BrandId, product.BrandName, product.CategoryName,
                product.CategoryId, product.ImageUrl, product.Description, product.AvailableQuantity, product.Price);

            return productDTO;
        }

        /// <summary>
        /// Basic method for get products
        /// </summary>
        /// <param name="categoryId">Search products by cateogry</param>
        /// <param name="includeSubcategories">If true, include subcategories products</param>
        /// <returns>List ReadProductDTO</returns>
        /// <exception cref="ValidationException">If category not has products</exception>
        /// <exception cref="NotFound">If products not found</exception>
        public async Task<IEnumerable<ReadProductDTO>> GetProducts(Guid categoryId)
        {
            if (categoryId == Guid.Empty || categoryId == new Guid())
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            var result = await GetProductsByCategoryHierarchy(categoryId);

            if (result is null || !result.Any())
                throw new NotFound(ErrorMessages.ProductNotFound);

            return result;
        }

        /// <summary>
        /// Get products by category id.
        /// </summary>
        /// <param name="categoryId">Search by category id</param>
        /// <returns>If category is main parent, will returned all subcategories and his products</returns>
        /// <exception cref="ValidationException">Guid cant be empty</exception>
        /// <exception cref="NotFound">Products not found</exception>
        private async Task<IEnumerable<ReadProductDTO>>? GetProductsByCategoryHierarchy(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            var categoryIdsAndNames = await _categoryService.GetAllNestedCategoryIdsAndNames(categoryId);

            var categoryIds = categoryIdsAndNames.Select(c => c.Item1);

            var products = await _productRepository.GetByCategoryIds(categoryIds);
            if (products is null || !products.Any())
                throw new NotFound(ErrorMessages.ProductNotFound);

            return products.Select(p => new ReadProductDTO(p.Id, p.Name, p.BrandId, p.BrandName, p.CategoryName, p.CategoryId, 
                p.ImageUrl, p.Description, p.AvailableQuantity, p.Price)).ToList();
        }

        /// <summary>
        /// Filter for products
        /// </summary>
        /// <param name="filter">Object has filter props.</param>
        /// <returns>Filtered products by category id, order, page, page size.</returns>
        public async Task<IEnumerable<ReadProductDTO>> GetFilteredProductsAsync(ProductFilterParams filter)
        {
            var categoryIdsAndNames = await _categoryService.GetAllNestedCategoryIdsAndNames(filter.CategoryId);
            foreach(var id in categoryIdsAndNames)
            {
                var isHaveProducts = await _categoryService.CategoryHasProducts(id.Item1);
                if(isHaveProducts)
                {
                    var products = await _productRepository.GetFilteredProductsAsync(id.Item1, filter.Sort, filter.Order,
                        filter.Page, filter.PageSize);

                    var productDTO = products.Select(p => new ReadProductDTO(p.Id, p.Name, p.BrandId, p.BrandName, p.CategoryName,
                        p.CategoryId, p.ImageUrl, p.Description, p.AvailableQuantity, p.Price)).ToList();

                    return productDTO;
                }
            }

            throw new NotFound(ErrorMessages.ProductNotFound);
        }

        public async Task<int> GetCountPages(Guid categoryId)
        {
            var products = await GetProductsByCategoryHierarchy(categoryId);

            var maxPages = Math.Ceiling((double)products.Count() / 10);

            return (int)maxPages;
        }

        //переделать
        public async Task<IEnumerable<ReadProductBlockDTO>> GetProductsForMainPage()
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

            var result = new List<ReadProductBlockDTO>();
            var readProductDTO = new List<ReadProductDTO>();
            foreach(var categoryId in popularCategoryId)
            {
                //if берем мейн категории(переделать надо)
                //var products = await GetProductsByCategoryHierarchy(categoryId);
                var products = await _productRepository.GetByCategoryId(categoryId);
                if (products is null || !products.Any())
                    throw new NotFound(ErrorMessages.ProductNotFound);
                //result.Add(products.ToList());

                (string categoryName, Guid categoryId) currentCategory = (products.First().CategoryName, 
                    products.First().CategoryId);

                readProductDTO = products.Select(p => new ReadProductDTO(p.Id, p.Name, p.BrandId, p.BrandName, p.CategoryName, 
                    p.CategoryId, p.ImageUrl, p.Description, p.AvailableQuantity, p.Price)).ToList();

                result.Add(new ReadProductBlockDTO(currentCategory.categoryId, currentCategory.categoryName,
                    readProductDTO));
            }

            return result;
        }
    }

}

