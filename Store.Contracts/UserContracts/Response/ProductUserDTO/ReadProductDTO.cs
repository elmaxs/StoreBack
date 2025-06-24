namespace Store.Contracts.UserContracts.Response.ProductUserDTO
{
    public record ReadProductDTO(
        Guid? Id,
        string? Name,
        string? CategoryName,
        string? ImageURL,
        decimal? Price);
}
