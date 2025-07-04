using Store.Contracts.UserContracts.Request.ProductUserDTO;
using Store.Contracts.UserContracts.Response.ProductUserDTO;

namespace Store.Application.Abstractions.User
{
    public interface IProductService
    {
        Task<IEnumerable<ReadProductDTO>> GetProducts(Guid categoryId, bool includeSubcategories);
        //Task<IEnumerable<ReadProductDTO>>? GetProductsByCategoryHierarchy(Guid categoryId);
        Task<IEnumerable<ReadProductDTO>> GetFilteredProductsAsync(ProductFilterParams filter);
        Task<IEnumerable<IEnumerable<ReadProductDTO>>> GetProductsForMainPage();//Task<IEnumerable<ReadProductMainPage>>
    }
}
