using Store.Core.Models;

namespace Store.Core.Abstractions.Repository
{
    public interface IUserRepository
    {
        Task<Guid> Create(User user);
        Task<IEnumerable<User>>? GetAll();
        Task<User>? GetById(Guid id);
        Task<Guid> Update(Guid id, User user);
        Task<Guid> Delete(Guid id);
    }
}
