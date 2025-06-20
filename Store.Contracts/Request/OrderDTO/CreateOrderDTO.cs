namespace Store.Contracts.Request.OrderDTO
{
    public record CreateOrderDTO(
        Guid UserId,
        DateTime CreatedAt,
        decimal TotalPrice,
        int Status
        );
}
