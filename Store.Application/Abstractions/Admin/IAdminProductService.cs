using Store.Application.Abstractions.User;
using Store.Contracts.AdminContracts.Request.ProductDTO;
using Store.Contracts.AdminContracts.Response.ProductDTO;
using Store.Core.Models;

namespace Store.Application.Abstractions.Admin
{
    public interface IAdminProductService
    {
        Task<Guid> CreateProduct(AdminCreateProductDTO productDTO);
        Task<IEnumerable<AdminReadProductDTO>> GetAllProducts();
        Task<AdminReadProductDTO> GetProductById(Guid id);
        Task<Guid> UpdateProduct(Guid id, AdminUpdateProductDTO productDTO);
        Task<Guid> DeleteProduct(Guid id);
    }
}
