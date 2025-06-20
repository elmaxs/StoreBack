namespace Store.Contracts.Request.ProductDTO
{
    public record CreateProductDTO(
        Guid CategoryId,
        string Name,
        string Description,
        string ImageUrl,
        decimal Price,
        int StockQuantity
        );
}
