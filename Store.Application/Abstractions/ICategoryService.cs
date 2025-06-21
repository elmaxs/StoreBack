using Store.Contracts.AdminContracts.Request.CategoryDTO;
using Store.Contracts.AdminContracts.Response.CategoryDTO;

namespace Store.Core.Abstractions.Services
{
    public interface ICategoryService
    {
        Task<Guid> CreateCategory(AdminCreateCategoryDTO categoryDTO);
        Task<IEnumerable<AdminReadCategoryDTO>> GetAllCategory();
        Task<AdminReadCategoryDTO> GetCategoryById(Guid id);
        Task<Guid> UpdateCategory(Guid id, AdminUpdateCategoryDTO categoryDTO);
        Task<Guid> DeleteCategory(Guid id);
        Task<bool> IsExistsCategory(Guid id);
    }
}
