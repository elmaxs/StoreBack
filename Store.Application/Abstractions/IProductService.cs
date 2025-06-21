using Store.Contracts.AdminContracts.Request.ProductDTO;
using Store.Contracts.AdminContracts.Response.ProductDTO;
using Store.Core.Models;

namespace Store.Application.Abstractions
{
    public interface IProductService
    {
        Task<Guid> CreateProduct(AdminCreateProductDTO productDTO);
        Task<IEnumerable<AdminReadProductDTO>> GetAllProducts();
        Task<AdminReadProductDTO> GetProductById(Guid id);
        Task<Guid> UpdateProduct(Guid id, AdminUpdateProductDTO productDTO);
        Task<Guid> DeleteProduct(Guid id);
        Task<IEnumerable<Product>>? GetProductsByCategoryId(Guid categoryId);
    }
}
