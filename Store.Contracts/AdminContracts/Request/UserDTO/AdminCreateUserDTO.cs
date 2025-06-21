namespace Store.Contracts.AdminContracts.Request.UserDTO
{
    public record AdminCreateUserDTO(
        Guid Id,
        string FullName,
        string Username,
        string HashedPassword,
        string Email,
        string PhoneNumber,
        int Role,
        DateTime CreatedAt);
}
