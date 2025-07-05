namespace Store.Contracts.UserContracts.Response.CategoryUserDTO
{
    public record ReadMainCategories(
        Guid Id,
        string CategoryName,
        bool HasProducts,
        bool HasSubcategories
        );
}
