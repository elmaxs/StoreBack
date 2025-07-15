using System.ComponentModel.DataAnnotations;

namespace Store.Contracts.Users
{
    public record LoginUserRequest(
        [Required] string Email,
        [Required] string Password
        ); 
}
