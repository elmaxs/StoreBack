using Microsoft.EntityFrameworkCore;
using Store.Core.Abstractions.Repository;
using Store.Core.Models;
using Store.DataAccess.Entities;

namespace Store.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OnlineStoreDbContext _context;

        public UserRepository(OnlineStoreDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Create(User user)
        {
            var userEntity = new UserEntity
            {
                Id = user.Id,
                Username = user.Username,
                HashedPassword = "sd",
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.Users.Where(u => u.Id == id).ExecuteDeleteAsync();

            return id;
        }

        public async Task<IEnumerable<User>>? GetAll()
        {
            var usersEntity = await _context.Users.ToListAsync();
            if (usersEntity is null)
                return null;

            var users = usersEntity.Select(u => User.CreateUser(u.Id, u.FullName, u.Username, u.Email, u.PhoneNumber, 
                u.Role, u.CreatedAt).User).ToList();

            return users;
        }

        public async Task<User>? GetById(Guid id)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (userEntity is null)
                return null;

            var user = User.CreateUser(userEntity.Id, userEntity.FullName, userEntity.Username, userEntity.Email,
                userEntity.PhoneNumber, userEntity.Role, userEntity.CreatedAt).User;

            return user;
        }

        public async Task<Guid> Update(Guid id, User user)
        {
            await _context.Users.Where(u => u.Id == id).ExecuteUpdateAsync(s => s
                .SetProperty(u => u.FullName, u => user.FullName)
                .SetProperty(u => u.Username, u => user.Username)
                .SetProperty(u => u.Email, u => user.Email)
                .SetProperty(u => u.PhoneNumber, u => user.PhoneNumber)
                .SetProperty(u => u.Role, u => user.Role));

            return id;
        }
    }
}
