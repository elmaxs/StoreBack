using Store.Application.Abstractions.User;
using Store.Contracts.UserContracts.Request.ProductReviewDTO;
using Store.Contracts.UserContracts.Response.ProductReviewDTO;
using Store.Core.Abstractions.Repository;
using Store.Core.Exceptions;
using Store.Core.Models;

namespace Store.Application.Services.User
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly IProductReviewRepository _productReviewRepo;
        private readonly IUserRepository _userRepository;

        public ProductReviewService(IProductReviewRepository productReviewRepo, IUserRepository userRepository)
        {
            _productReviewRepo = productReviewRepo;
            _userRepository = userRepository;
        }

        public async Task<double> GetAverageRatingForProduct(Guid productId)
        {
            if (productId == Guid.Empty)
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            var ratings = await _productReviewRepo.GetRatingsForProduct(productId);
            if (ratings.Count() == 0)
                return 0.0;

            var average = Math.Round(ratings.Average(), 1);

            return average;
        }

        public async Task<Dictionary<int, int>> GetRatingsInfoForProduct(Guid productId)
        {
            if (productId == Guid.Empty)
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            var ratings = await _productReviewRepo.GetRatingsForProduct(productId);
            if (ratings.Count() == 0)
                return new Dictionary<int, int>();

            var total = ratings.Count();

            var distribution = Enumerable.Range(1, 5).ToDictionary(
                rating => rating,
                rating => (int)Math.Round((double)ratings.Count(r => r == rating) / total * 100));

            return distribution;
        }

        public async Task<ICollection<ReadProductReviewDTO>> GetReviewsForProduct(Guid productId)
        {
            if (productId == Guid.Empty)
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            var reviews = await _productReviewRepo.GetForProduct(productId);

            var reviewsDTO = reviews.Select(r => new ReadProductReviewDTO(r.Id, r.UserId, r.UserName, r.Text, 
                r.Rating, r.CreatedAt)).ToList();

            return reviewsDTO;
        }

        public async Task<Guid> CreateProductReview(CreateProductReviewDTO reviewDTO)
        {
            var user = await _userRepository.GetById(reviewDTO.UserId);
            if (user is null)
                throw new NotFound(ErrorMessages.UserNotFound);

            var (review, error) = ProductReview.CreateProductReview(Guid.NewGuid(), reviewDTO.UserId, reviewDTO.ProductId,
                user.FullName, reviewDTO.Text, reviewDTO.Rating, DateTime.UtcNow);

            if (review is not null)
                return await _productReviewRepo.Create(review);
            else
                throw new ErrorDuringCreation(error);
        }

        public async Task<Guid> DeleteProductReview(Guid id)
        {
            if (id == Guid.Empty || id == new Guid())
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            return await _productReviewRepo.Delete(id);
        }

        public async Task<List<ReadProductReviewDTO>> GetAllProductReviews()
        {
            var reviews = await _productReviewRepo.GetAll();
            if (reviews is null || !reviews.Any())
                throw new NotFound(ErrorMessages.ProductReviewNotFound);

            var reviewsDTO = reviews.Select(r => new ReadProductReviewDTO(r.Id, r.UserId, r.UserName, r.Text, r.Rating, r.CreatedAt))
                .ToList();

            return reviewsDTO;
        }

        public async Task<ReadProductReviewDTO> GetProductReviewById(Guid id)
        {
            if (id == Guid.Empty || id == new Guid())
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            var review = await _productReviewRepo.GetById(id);
            if (review is null)
                throw new NotFound(ErrorMessages.ProductReviewNotFound);

            var reviewDTO = new ReadProductReviewDTO(review.Id, review.UserId, review.UserName, review.Text, 
                review.Rating, review.CreatedAt);

            return reviewDTO;
        }

        public async Task<Guid> UpdateProductReview(Guid id, UpdateProductReviewDTO reviewDTO)
        {
            if (id == Guid.Empty || id == new Guid())
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            var review = await _productReviewRepo.GetById(id);
            if(review is null)
                throw new NotFound(ErrorMessages.ProductReviewNotFound);

            var (updateReview, error) = ProductReview.CreateProductReview(review.Id, review.UserId, review.ProductId, review.UserName,
                reviewDTO.Text, reviewDTO.Rating, review.CreatedAt);

            if (review is not null)
                return await _productReviewRepo.Update(id, updateReview);
            else
                throw new ErrorDuringCreation(error);
        }
    }
}
