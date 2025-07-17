namespace Store.Contracts.UserContracts.Request.ProductReviewDTO
{
    public record UpdateProductReviewDTO(
        string Text,
        int? Rating);
}
