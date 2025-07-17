using Store.Core.Exceptions;

namespace Store.Core.Models
{
    public class ProductReview
    {
        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }
        public Guid ProductId { get; private set; }

        public string? UserName { get; private set; }
        public string Text { get; private set; }
        public int? Rating { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private ProductReview(Guid id, Guid userId, Guid productId, string? userName, string text, int? rating, DateTime createdAt)
        {
            Id = id;
            UserId = userId;
            ProductId = productId;
            UserName = userName;
            Text = text;
            Rating = rating;
            CreatedAt = createdAt;
        }

        public static (ProductReview? ProductReview, string? Error) CreateProductReview(Guid id, Guid userId, Guid productId, 
            string? userName, string text, int? rating, DateTime createdAt)
        {
            if (id == Guid.Empty || userId == Guid.Empty || productId == Guid.Empty)
                return (null, ErrorMessages.GuidCannotBeEmpty);

            if (string.IsNullOrEmpty(text))
                return (null, "Text cant be empty");

            var review = new ProductReview(id, userId, productId, userName, text, rating, createdAt);

            return (review, null);
        }
    }
}
