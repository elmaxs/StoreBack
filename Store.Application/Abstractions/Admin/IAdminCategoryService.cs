using Store.Contracts.AdminContracts.Request.CategoryDTO;
using Store.Contracts.AdminContracts.Response.CategoryDTO;

namespace Store.Application.Abstractions.Admin
{
    public interface IAdminCategoryService
    {
        Task<Guid> CreateCategory(AdminCreateCategoryDTO categoryDTO);
        Task<IEnumerable<AdminReadCategoryDTO>> GetAllCategory();
        Task<AdminReadCategoryDTO> GetCategoryById(Guid id);
        Task<Guid> UpdateCategory(Guid id, AdminUpdateCategoryDTO categoryDTO);
        Task<Guid> DeleteCategory(Guid id);
        Task<bool> IsExistsCategory(Guid id);
    }
}
