using Store.Contracts.Request.CategoryDTO;
using Store.Contracts.Response.CategoryDTO;

namespace Store.Core.Abstractions.Services
{
    public interface ICategoryService
    {
        Task<Guid> CreateCategory(CreateCategoryDTO categoryDTO);
        Task<IEnumerable<ReadCategoryDTO>> GetAllCategory();
        Task<ReadCategoryDTO> GetCategoryById(Guid id);
        Task<Guid> UpdateCategory(Guid id, UpdateCategoryDTO categoryDTO);
        Task<Guid> DeleteCategory(Guid id);
        Task<bool> IsExistsCategory(Guid id);
    }
}
