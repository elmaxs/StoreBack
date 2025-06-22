using Store.Core.Models;

namespace Store.Core.Abstractions.Repository
{
    public interface ICategoryRepository
    {
        Task<Guid> Create(Category category);
        Task<IEnumerable<Category>>? GetAll();
        Task<Category>? GetById(Guid id);
        Task<Guid> Update(Guid id, Category category);
        Task<Guid> Delete(Guid id);

        Task<int> GetCountProductInCategory(Guid categoryId);
        Task<bool> IsExists(Guid id);
        Task<Category> TreeSelected(Guid currentCategoryId);
    }
}
