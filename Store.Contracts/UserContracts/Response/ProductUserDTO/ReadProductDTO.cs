namespace Store.Contracts.UserContracts.Response.ProductUserDTO
{
    public record ReadProductDTO(
        string Name,
        string CategoryName,
        string ImageURL,
        decimal Price);
}
