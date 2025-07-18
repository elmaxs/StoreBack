namespace Store.Contracts.UserContracts.Request.CartUserDTO
{
    public record CartItemDTO(
        Guid ProductId,
        int Quantity);
}
