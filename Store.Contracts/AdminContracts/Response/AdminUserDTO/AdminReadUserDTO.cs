namespace Store.Contracts.AdminContracts.Response.UserDTO
{
    public record AdminReadUserDTO(
        Guid Id,
        string FullName,
        string Username,
        string HashedPassword,
        string Email,
        string PhoneNumber,
        int Role,
        DateTime CreatedAt,
        List<AdminOrderInUserDTO> Orders);
}
