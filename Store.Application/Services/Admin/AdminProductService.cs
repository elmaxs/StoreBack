using Store.Application.Abstractions.Admin;
using Store.Contracts.AdminContracts.Request.ProductDTO;
using Store.Contracts.AdminContracts.Response.ProductDTO;
using Store.Core.Abstractions.Repository;
using Store.Core.Exceptions;
using Store.Core.Models;

namespace Store.Application.Services.Admin
{
    public class AdminProductService : IAdminProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;

        public AdminProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, 
            IBrandRepository brandRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
        }

        public async Task<Guid> CreateProduct(AdminCreateProductDTO productDTO)
        {
            if (productDTO is null)
                throw new BadDataDTO(ErrorMessages.BadDataDTO);

            var id = Guid.NewGuid();

            var category = await _categoryRepository.GetById(productDTO.CategoryId);
            var brand = await _brandRepository.GetById(productDTO.BrandId);
            if (category is null)
                throw new NotFound(ErrorMessages.CategoryNotFound);
            if (brand is null)
                throw new NotFound(ErrorMessages.BrandNotFound);

            var (product, error) = Product.CreateProduct(id, productDTO.Name, productDTO.Description, productDTO.ImageUrl, productDTO.Price,
                productDTO.CategoryId, category.CategoryName, brand.Id, brand.Name, productDTO.StockQuantity);
            if (!string.IsNullOrEmpty(error))
                throw new ErrorDuringCreation(error);

            return await _productRepository.Create(product);
        }

        public async Task<Guid> DeleteProduct(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            return await _productRepository.Delete(id);
        }

        public async Task<IEnumerable<AdminReadProductDTO>> GetAllProducts()
        {
            var products = await _productRepository.GetAll();
            if (products is null)
                throw new NotFound(ErrorMessages.ProductNotFound);

            var productsDTO = products.Select(p => new AdminReadProductDTO
            (
                p.Id,
                p.CategoryId,
                p.BrandId,
                p.Name,
                p.Description,
                p.ImageUrl,
                p.Price,
                p.StockQuantity,
                p.IsAvailable
            )).ToList();

            return productsDTO;
        }

        public async Task<AdminReadProductDTO> GetProductById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            var product = await _productRepository.GetById(id);
            if (product is null)
                throw new NotFound(ErrorMessages.ProductNotFound);

            var productDTO = new AdminReadProductDTO(
                product.Id,
                product.CategoryId,
                product.Name,
                product.Description,
                product.ImageUrl,
                product.Price,
                product.StockQuantity,
                product.IsAvailable);

            return productDTO;
        }

        public async Task<Guid> UpdateProduct(Guid id, AdminUpdateProductDTO productDTO)
        {
            if (productDTO is null)
                throw new BadDataDTO(ErrorMessages.BadDataDTO);

            var product = await _productRepository.GetById(id);
            if (product is null)
                throw new NotFound(ErrorMessages.ProductNotFound);

            product.CategoryId = productDTO.CategoryId;
            product.Name = productDTO.Name;
            product.Description = productDTO.Description;
            product.ImageUrl = productDTO.ImageUrl;
            product.Price = productDTO.Price;
            product.StockQuantity = productDTO.StockQuantity;

            return await _productRepository.Update(id, product);
        }
    }
}
