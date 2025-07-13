namespace Store.Contracts.UserContracts.Response.CategoryUserDTO
{
    public record ReadSubcategoriesDTO(
        Guid Id,
        string Name,
        bool HasProducts,
        bool HasSubcategories,
        bool HasParentCategory,
        IEnumerable<ReadSubcategoriesDTO>? Subcategories
        );
    //public record ReadSubCategoriesDTO(
    //    Guid Id,
    //    string Name,
    //    bool HasProducts,
    //    bool HasSubcategories,
    //    bool HasParentCategory,
    //    IEnumerable<ReadSubCategoriesDTO>? Subcategories
    //    );

    //public record Subcategories(
    //    Guid Id,
    //    string Name,
    //    bool HasSubCategories,
    //    IEnumerable<Subcategory>? Subcateogirs
    //    );

    //public record Subcategory(
    //    Guid Id,
    //    string Name);
}
