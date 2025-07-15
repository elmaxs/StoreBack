using Store.Application.Abstractions.Auth;
using Store.Application.Abstractions.User;
using Store.Core.Abstractions.Repository;
using Store.Core.Exceptions;
using Store.Core.Models;

namespace Store.Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public UserService(IPasswordHasher passwordHasher, IUserRepository userRepository, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _userRepository.GetByEmail(email);
            if (user is null)
                throw new NotFound(ErrorMessages.UserNotFound);

            var result = _passwordHasher.Verify(password, user.PasswordHash);

            if (result == false)
                throw new Exception("Failed to login");

            var token = _jwtProvider.GenerateToken(user);

            return token;
        }

        public async Task Register(string userName, string email, string password)
        {
            var hashedPassword = _passwordHasher.Generate(password);

            var (user, error) = Store.Core.Models.User.CreateUser(Guid.NewGuid(), null, userName, hashedPassword, email,
                null, Core.Enums.UserRole.Customer, DateTime.UtcNow);

            if (!string.IsNullOrEmpty(error))
                throw new ValidationException(error);

            await _userRepository.Create(user);
        }
    }
}
