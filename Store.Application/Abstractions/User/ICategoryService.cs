using Store.Contracts.UserContracts.Response.CategoryUserDTO;

namespace Store.Application.Abstractions.User
{
    public interface ICategoryService
    {
        Task<IEnumerable<BreadcrumbCategoryDTO>> BuildBreadcrumbAsync(Guid currentCategoryId);

        //Task<TreeCategoryDTO> BuildTreeCategory(Guid currentCategoryId); mb late
    }
}
