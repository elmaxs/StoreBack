namespace Store.Contracts.Response.OrderDTO
{
    public record ReadOrderDTO(
        Guid Id,
        Guid UserId,
        DateTime CreatedAt,
        decimal TotalPrice,
        int Status,
        List<OrderItemInOrderDTO> OrderItems
        );
}
