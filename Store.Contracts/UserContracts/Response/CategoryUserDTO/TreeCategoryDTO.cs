namespace Store.Contracts.UserContracts.Response.CategoryUserDTO
{
    public record TreeCategoryDTO(
        Guid Id,
        string Name,
        ICollection<TreeCategoryDTO> ParentsCategories,
        ICollection<TreeCategoryDTO> Subcategories
        );
}
