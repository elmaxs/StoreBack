namespace Store.Contracts.AdminContracts.Request.OrderDTO
{
    public record AdminUpdateOrderDTO(
        DateTime CreatedAt,
        decimal TotalPrice,
        int Status
        );
}
