using Microsoft.EntityFrameworkCore;
using Store.Core.Abstractions.Repository;
using Store.Core.Models;
using Store.DataAccess.Entities;

namespace Store.DataAccess.Repositories
{
    public class ProductReviewRepository : IProductReviewRepository
    {
        private readonly OnlineStoreDbContext _context;

        public ProductReviewRepository(OnlineStoreDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ProductReview>> GetForProduct(Guid productId)
        {
            var reviewsEntity = await _context.ProductReviews.Include(r => r.User)
                .Where(r => r.ProductId == productId).ToListAsync();

            var reviews = reviewsEntity.Select(r => ProductReview.CreateProductReview(r.Id, r.UserId, r.ProductId, r.User.Username,
                r.Text, r.Rating, r.CreatedAt).ProductReview).ToList();

            return reviews;
        }

        public async Task<ICollection<int>> GetRatingsForProduct(Guid productId)
        {
            var ratings = await _context.ProductReviews.Where(r => r.ProductId == productId && r.Rating != null)
                .Select(r => r.Rating!.Value).ToListAsync();

            return ratings;
        }

        public async Task<Guid> Create(ProductReview review)
        {
            var reviewEntity = new ProductReviewEntity
            {
                Id = review.Id,
                UserId = review.UserId,
                ProductId = review.ProductId,
                Text = review.Text,
                Rating = review.Rating,
                CreatedAt = review.CreatedAt
            };

            await _context.ProductReviews.AddAsync(reviewEntity);
            await _context.SaveChangesAsync();

            return review.Id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.ProductReviews.Where(pr => pr.Id == id).ExecuteDeleteAsync();

            return id;
        }

        public async Task<List<ProductReview>> GetAll()
        {
            var reviewsEntity = await _context.ProductReviews.Include(pr => pr.User).ToListAsync();

            var review = reviewsEntity.Select(pr => ProductReview.CreateProductReview(pr.Id, pr.UserId, pr.ProductId,
                pr.User.FullName, pr.Text, pr.Rating, pr.CreatedAt).ProductReview).ToList();

            return review;
        }

        public async Task<ProductReview>? GetById(Guid id)
        {
            var reviewEntity = await _context.ProductReviews.Include(pr => pr.User).FirstOrDefaultAsync(pr => pr.Id == id);
            if (reviewEntity is null)
                return null;

            var review = ProductReview.CreateProductReview(reviewEntity.Id, reviewEntity.UserId, reviewEntity.ProductId,
                reviewEntity.User.FullName, reviewEntity.Text, reviewEntity.Rating, reviewEntity.CreatedAt).ProductReview;

            return review;
        }

        public async Task<Guid> Update(Guid id, ProductReview review)
        {
            await _context.ProductReviews.Where(pr => pr.Id == id).ExecuteUpdateAsync(s => s
                .SetProperty(pr => pr.Text, review.Text)
                .SetProperty(pr => pr.Rating, review.Rating));

            return id;
        }
    }
}
