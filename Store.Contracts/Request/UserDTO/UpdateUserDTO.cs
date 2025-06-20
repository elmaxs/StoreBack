namespace Store.Contracts.Request.UserDTO
{
    public record UpdateUserDTO(
        string FullName,
        string Username,
        string HashedPassword,
        string Email,
        string PhoneNumber,
        int Role,
        DateTime CreatedAt
        );
}
