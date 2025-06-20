namespace Store.Contracts.Response.ProductDTO
{
    public record ReadProductDTO(
        Guid Id,
        Guid CategoryId,
        string Name,
        string Description,
        string ImageUrl,
        decimal Price,
        int StockQuantity,
        bool IsAvailable
        );
}
