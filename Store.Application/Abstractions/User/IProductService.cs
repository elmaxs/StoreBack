using Store.Contracts.UserContracts.Request.ProductUserDTO;
using Store.Contracts.UserContracts.Response.ProductUserDTO;

namespace Store.Application.Abstractions.User
{
    public interface IProductService
    {
        Task<IEnumerable<ReadProductByCategoryDTO>>? GetProductsByCategoryHierarchy(Guid categoryId);
        Task<IEnumerable<ReadProductDTO>> GetFilteredProductsAsync(ProductFilterParams filter);
    }
}
