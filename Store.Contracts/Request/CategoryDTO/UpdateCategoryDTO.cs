namespace Store.Contracts.Request.CategoryDTO
{
    public record UpdateCategoryDTO(
        string Name,
        Guid? ParentCategoryId
        );
}
