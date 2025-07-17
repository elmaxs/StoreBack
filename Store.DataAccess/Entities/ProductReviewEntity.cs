namespace Store.DataAccess.Entities
{
    public class ProductReviewEntity
    {
        public Guid Id { get; set; }
        
        public UserEntity User { get; set; }
        public Guid UserId { get; set; }
        public ProductEntity Product { get; set; }
        public Guid ProductId { get; set; }

        public string Text { get; set; }
        public int? Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
