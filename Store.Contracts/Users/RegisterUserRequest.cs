using System.ComponentModel.DataAnnotations;

namespace Store.Contracts.Users
{
    public record RegisterUserRequest(
        [Required] string Email,
        [Required] string UserName,
        [Required] string Password
        );
}
