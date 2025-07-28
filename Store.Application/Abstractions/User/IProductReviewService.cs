using Store.Contracts.UserContracts.Request.ProductReviewDTO;
using Store.Contracts.UserContracts.Response.ProductReviewDTO;
using Store.Core.Models;

namespace Store.Application.Abstractions.User
{
    public interface IProductReviewService
    {
        Task<ReadRatingsDTO> GetRatingsDTO(Guid productId);
        Task<Guid> CreateProductReview(Guid userId, CreateProductReviewDTO reviewDTO);
        Task<Guid> DeleteProductReview(Guid id);
        Task<ReadProductReviewDTO> GetProductReviewById(Guid id);
        Task<List<ReadProductReviewDTO>> GetAllProductReviews();
        Task<Guid> UpdateProductReview(Guid id, UpdateProductReviewDTO reviewDTO);
        Task<ICollection<ReadProductReviewDTO>> GetReviewsForProduct(Guid productId);
        Task<Dictionary<int, int>> GetRatingsInfoForProduct(Guid productId);
        Task<double> GetAverageRatingForProduct(Guid productId);
    }
}
