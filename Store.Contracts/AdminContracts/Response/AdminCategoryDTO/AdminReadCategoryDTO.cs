namespace Store.Contracts.AdminContracts.Response.CategoryDTO
{
    public record AdminReadCategoryDTO(
        Guid Id,
        Guid? ParentCategoryId,
        string CategoryName,
        List<AdminProductInCategoryDTO>? Products,
        List<AdminReadCategoryDTO>? Subcategories);
}
