namespace Store.Contracts.Request.CategoryDTO
{
    public record CreateCategoryDTO(
        Guid? ParentCategoryId,
        string Name
        );
}
