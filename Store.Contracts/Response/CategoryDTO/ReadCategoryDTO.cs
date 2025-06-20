namespace Store.Contracts.Response.CategoryDTO
{
    public record ReadCategoryDTO(
        Guid Id,
        Guid? ParentCategoryId,
        string CategoryName,
        List<ProductInCategoryDTO>? Products,
        List<SubcategoryDto>? Subcategories);
}
