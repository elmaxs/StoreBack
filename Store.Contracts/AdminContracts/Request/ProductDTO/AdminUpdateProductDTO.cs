namespace Store.Contracts.AdminContracts.Request.ProductDTO
{
    public record AdminUpdateProductDTO(
        Guid BrandId,
        Guid CategoryId,
        string Name,
        string Description,
        string ImageUrl,
        decimal Price,
        int StockQuantity);
}
