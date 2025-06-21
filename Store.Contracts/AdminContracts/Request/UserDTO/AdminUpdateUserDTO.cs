namespace Store.Contracts.AdminContracts.Request.UserDTO
{
    public record AdminUpdateUserDTO(
        string FullName,
        string Username,
        string HashedPassword,
        string Email,
        string PhoneNumber,
        int Role,
        DateTime CreatedAt
        );
}
