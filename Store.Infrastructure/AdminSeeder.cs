using Store.Application.Abstractions.Auth;
using Store.Core.Abstractions.Repository;

namespace Store.Infrastructure
{
    public class AdminSeeder
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _hasher;

        public AdminSeeder(IUserRepository userRepository, IPasswordHasher hasher)
        {
            _userRepository = userRepository;
            _hasher = hasher;
        }

        public async Task SeedAsync()
        {
            var adminEmail = "admin@store.com";
            var admin = await _userRepository.GetByEmail(adminEmail);

            if (admin != null) return;

            var hashedPassword = _hasher.Generate("admin123");

            var (user, error) = Store.Core.Models.User.CreateUser(
                Guid.NewGuid(),
                null,
                "admin",
                hashedPassword,
                adminEmail,
                null,
                Core.Enums.UserRole.Admin,
                DateTime.UtcNow
            );

            if (!string.IsNullOrEmpty(error)) throw new Exception(error);

            await _userRepository.Create(user);
        }
    }
}
