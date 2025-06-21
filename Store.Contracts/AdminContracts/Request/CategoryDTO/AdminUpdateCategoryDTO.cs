namespace Store.Contracts.AdminContracts.Request.CategoryDTO
{
    public record AdminUpdateCategoryDTO(
        string Name,
        Guid? ParentCategoryId
        );
}
