namespace Store.Contracts.Response.UserDTO
{
    public record ReadUserDTO(
        Guid Id,
        string FullName,
        string Username,
        string HashedPassword,
        string Email,
        string PhoneNumber,
        int Role,
        DateTime CreatedAt,
        List<OrderInUserDTO> Orders);
}
