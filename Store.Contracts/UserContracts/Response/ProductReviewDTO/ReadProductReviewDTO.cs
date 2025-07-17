namespace Store.Contracts.UserContracts.Response.ProductReviewDTO
{
    public record ReadProductReviewDTO(
        Guid Id,
        Guid UserId,
        string UserName,
        string Text,
        int? Rating,
        DateTime CreatedAt
        );
}
