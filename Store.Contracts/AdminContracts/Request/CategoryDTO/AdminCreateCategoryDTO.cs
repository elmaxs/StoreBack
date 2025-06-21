namespace Store.Contracts.AdminContracts.Request.CategoryDTO
{
    public record AdminCreateCategoryDTO(
        Guid? ParentCategoryId,
        string Name
        );
}
