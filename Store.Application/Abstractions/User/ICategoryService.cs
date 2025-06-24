using Store.Contracts.UserContracts.Response.CategoryUserDTO;
using Store.Core.Models;
using System.Threading.Tasks;

namespace Store.Application.Abstractions.User
{
    public interface ICategoryService
    {
        Task<IEnumerable<BreadcrumbCategoryDTO>> BuildBreadcrumbAsync(Guid currentCategoryId);
        //Task<TreeCategoryDTO> BuildTreeCategory(Guid currentCategoryId); mb late
        Task<IEnumerable<ReadCategoriesDTO>> GetMainsCategories();
        Task<IEnumerable<Guid>> GetAllNestedCategoryIds(Guid categoryId);
    }
}
