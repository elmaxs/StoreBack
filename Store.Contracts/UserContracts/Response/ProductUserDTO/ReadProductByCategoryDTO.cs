namespace Store.Contracts.UserContracts.Response.ProductUserDTO
{
    public record ReadProductByCategoryDTO(
        Guid CategoryId,
        string? CategoryName,
        List<ReadProductForCategoryDTO>? Products
        );

    public record ReadProductForCategoryDTO(
        Guid? ProductId,
        string? ProductName,
        string? ImageURL,
        decimal? Price
        );
}
