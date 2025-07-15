namespace Store.Application.Abstractions.Auth
{
    public interface IJwtProvider
    {
        string GenerateToken(Store.Core.Models.User user);
    }
}
