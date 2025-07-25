using Store.Core.Models;

namespace Store.Core.Abstractions.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetBySearch(string search);
        Task<Guid> Create(Product product);
        Task<IEnumerable<Product>>? GetAll();
        Task<Product>? GetById(Guid id);
        Task<IEnumerable<Product>>? GetByCategoryId(Guid categoryId);
        Task<Guid> Update(Guid id, Product product);
        Task<Guid> Delete(Guid id);
        Task<List<Product>> GetFilteredProductsAsync(Guid? categoryId, string sortBy, string orderBy, int page, int pageSize);
        Task<IEnumerable<Product>>? GetByCategoryIds(IEnumerable<Guid> categoryIds);
    }
}
