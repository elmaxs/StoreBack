using Store.Contracts.UserContracts.Response.CategoryUserDTO;

namespace Store.Application.Abstractions.User
{
    public interface ICategoryService
    {
        Task<IEnumerable<BreadcrumbCategoryDTO>> BuildBreadcrumbAsync(Guid currentCategoryId);
        //Task<TreeCategoryDTO> BuildTreeCategory(Guid currentCategoryId); mb late
        Task<IEnumerable<ReadMainCategories>> GetMainsCategories();
        Task<IEnumerable<(Guid, string)>> GetAllNestedCategoryIdsAndNames(Guid categoryId);
        Task<IEnumerable<CategoriesForMainDTO>> GetCategoriesForMainPage();
        Task<bool> CategoryHasProducts(Guid categoryId);
    }
}
