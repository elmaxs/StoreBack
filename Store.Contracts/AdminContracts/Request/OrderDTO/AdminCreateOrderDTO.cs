namespace Store.Contracts.AdminContracts.Request.OrderDTO
{
    public record AdminCreateOrderDTO(
        Guid UserId,
        DateTime CreatedAt,
        decimal TotalPrice,
        int Status
        );
}
