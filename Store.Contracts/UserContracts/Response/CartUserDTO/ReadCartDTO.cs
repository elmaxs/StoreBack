namespace Store.Contracts.UserContracts.Response.CartUserDTO
{
    public record ReadCartDTO(
        Guid ProductId,
        string ProductName,
        string ImageUrl,
        int Quantity,
        decimal UnitPrice,
        decimal TotalPrice);

}
