namespace Store.Contracts.UserContracts.Response.CartUserDTO
{
    public record ReadCartDTO(
        decimal TotalPrice,
        ICollection<ItemCartDTO> Items);

    public record ItemCartDTO(
        Guid ProductId,
        string ProductName,
        string ImageUrl,
        int Quantity,
        int AvailableQuantity,
        decimal UnitPrice,
        decimal TotalPrice);
}
