namespace Store.Contracts.AdminContracts.Response.ProductDTO
{
    public record AdminReadProductDTO(
        Guid Id,
        Guid CategoryId,
        Guid? BrandId,
        string Name,
        string Description,
        string ImageUrl,
        decimal Price,
        int StockQuantity,
        bool IsAvailable
        );
}
