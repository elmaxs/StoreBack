namespace Store.Contracts.AdminContracts.Request.ProductDTO
{
    public record AdminCreateProductDTO(
        Guid CategoryId,
        string Name,
        string Description,
        string ImageUrl,
        decimal Price,
        int StockQuantity
        );
}
