using Store.Core.Models;

namespace Store.Core.Abstractions.Repository
{
    public interface IProductReviewRepository
    {
        Task<Guid> Create(ProductReview review);
        Task<Guid> Delete(Guid id);
        Task<ProductReview>? GetById(Guid id);
        Task<List<ProductReview>> GetAll();
        Task<Guid> Update(Guid id, ProductReview review);
    }
}
