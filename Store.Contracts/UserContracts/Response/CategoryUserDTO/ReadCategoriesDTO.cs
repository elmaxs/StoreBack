namespace Store.Contracts.UserContracts.Response.CategoryUserDTO
{
    public record ReadCategoriesDTO(
        Guid Id,
        string Name,
        bool HasProducts,
        bool HasSubcategories,
        IEnumerable<ReadCategoriesDTO>? Subcategories
        );
}
