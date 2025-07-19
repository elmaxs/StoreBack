using Store.Contracts.UserContracts.Request.ProductUserDTO;
using Store.Contracts.UserContracts.Response.ProductUserDTO;

namespace Store.Application.Abstractions.User
{
    public interface IProductService
    {
        Task<IEnumerable<ReadProductDTO>> GetProducts(Guid categoryId);
        //Task<IEnumerable<ReadProductDTO>>? GetProductsByCategoryHierarchy(Guid categoryId);
        Task<IEnumerable<ReadProductDTO>> GetFilteredProductsAsync(ProductFilterParams filter);
        Task<IEnumerable<ReadProductBlockDTO>> GetProductsForMainPage();//Task<IEnumerable<ReadProductMainPage>>
        Task<ReadProductDTO> GetProductById(Guid id);
        Task<bool> IsAvailableQuantity(Guid productId, int quantity);
    }
}
