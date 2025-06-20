namespace Store.Contracts.Request.OrderDTO
{
    public record UpdateOrderDTO(
        DateTime CreatedAt,
        decimal TotalPrice,
        int Status
        );
}
