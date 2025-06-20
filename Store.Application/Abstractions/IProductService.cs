using Store.Contracts.Request.ProductDTO;
using Store.Contracts.Response.ProductDTO;

namespace Store.Application.Abstractions
{
    public interface IProductService
    {
        Task<Guid> CreateProduct(CreateProductDTO productDTO);
        Task<IEnumerable<ReadProductDTO>> GetAllProducts();
        Task<ReadProductDTO> GetProductById(Guid id);
        Task<Guid> UpdateProduct(Guid id, UpdateProductDTO productDTO);
        Task<Guid> DeleteProduct(Guid id);
    }
}
