using Store.Application.Abstractions.User;
using Store.Contracts.UserContracts.Response.ProductUserDTO;
using Store.Core.Abstractions.Repository;
using Store.Core.Exceptions;
using ValidationException = Store.Core.Exceptions.ValidationException;

namespace Store.Application.Services.User
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ReadProductDTO>>? GetProductsByCategoryId(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            var products = await _productRepository.GetByCategoryId(categoryId);
            if (products is null)
                throw new NotFound(ErrorMessages.ProductNotFound);

            var productsDTO = products.Select(p => new ReadProductDTO(p.Name, p.CategoryName, p.ImageUrl, p.Price));

            return productsDTO;
        }
    }
}
