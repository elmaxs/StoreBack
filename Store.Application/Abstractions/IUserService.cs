using Store.Contracts.Request.UserDTO;
using Store.Contracts.Response.UserDTO;

namespace Store.Application.Abstractions
{
    public interface IUserService
    {
        Task<Guid> CreateUser(CreateUserDTO userDTO);
        Task<IEnumerable<ReadUserDTO>> GetAllUsers();
        Task<ReadUserDTO> GetUserById(Guid id);
        Task<Guid> UpdateUser(UpdateUserDTO userDTO);
        Task<Guid> DeleteUser(Guid id);
    }
}
