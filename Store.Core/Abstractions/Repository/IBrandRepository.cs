using Store.Core.Models;

namespace Store.Core.Abstractions.Repository
{
    public interface IBrandRepository
    {
        Task<Guid> Create(Brand brand);
        Task<IEnumerable<Brand>>? GetAll();
        Task<Brand>? GetById(Guid id);
        Task<Guid> Update(Guid id, Brand brand);
        Task<Guid> Delete(Guid id);
    }
}
