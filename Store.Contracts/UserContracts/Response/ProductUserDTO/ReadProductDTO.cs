namespace Store.Contracts.UserContracts.Response.ProductUserDTO
{
    public record ReadProductDTO(
        Guid? Id,
        string? Name,
        Guid? BrandId,
        string? BrandName,
        string? CategoryName,
        Guid? CategoryId,
        string? ImageURL,
        string? Description,
        int AvailableQuantity,
        decimal? Price);
}
