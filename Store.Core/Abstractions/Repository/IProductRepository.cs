using Store.Core.Models;

namespace Store.Core.Abstractions.Repository
{
    public interface IProductRepository
    {
        Task<Guid> Create(Product product);
        Task<IEnumerable<Product>>? GetAll();
        Task<Product>? GetById(Guid id);
        Task<IEnumerable<Product>>? GetByCategoryId(Guid categoryId);
        Task<Guid> Update(Guid id, Product product);
        Task<Guid> Delete(Guid id);
    }
}
