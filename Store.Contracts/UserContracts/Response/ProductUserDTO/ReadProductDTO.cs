namespace Store.Contracts.UserContracts.Response.ProductUserDTO
{
    public record ReadProductDTO(
        Guid? Id,
        string? Name,
        string? CategoryName,
        Guid? CategoryId,
        string? ImageURL,
        string? Description,
        decimal? Price);
}
