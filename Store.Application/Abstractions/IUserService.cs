using Store.Contracts.AdminContracts.Request.UserDTO;
using Store.Contracts.AdminContracts.Response.UserDTO;

namespace Store.Application.Abstractions
{
    public interface IUserService
    {
        Task<Guid> CreateUser(AdminCreateUserDTO userDTO);
        Task<IEnumerable<AdminReadUserDTO>> GetAllUsers();
        Task<AdminReadUserDTO> GetUserById(Guid id);
        Task<Guid> UpdateUser(AdminUpdateUserDTO userDTO);
        Task<Guid> DeleteUser(Guid id);
    }
}
