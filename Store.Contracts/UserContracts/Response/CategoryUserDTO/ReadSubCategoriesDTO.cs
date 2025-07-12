namespace Store.Contracts.UserContracts.Response.CategoryUserDTO
{
    public record ReadSubCategoriesDTO(
        Guid Id,
        string Name,
        bool HasProducts,
        bool HasSubcategories,
        bool HasParentCategory,
        IEnumerable<Subcategory>? Subcategories
        );

    public record Subcategory(
        Guid Id,
        string Name);
}
