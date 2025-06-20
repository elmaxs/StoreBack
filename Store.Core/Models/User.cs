using Store.Core.Enums;

namespace Store.Core.Models
{
    public class User
    {
        public Guid Id { get; }

        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTime CreatedAt { get; }

        private User(Guid id, string fullName, string username, string email, string phoneNumber, UserRole role, 
            DateTime createdAt)
        {
            Id = id;
            FullName = fullName;
            Username = username;
            Email = email;
            PhoneNumber = phoneNumber;
            Role = role;
            CreatedAt = createdAt;
        }

        public static (User User, string Error) CreateUser(Guid id, string fullName, string username, string email, 
            string phoneNumber, UserRole role, DateTime createdAt)
        {
            string error = string.Empty;

            if (id == Guid.Empty || id == new Guid())
            {
                error = "Id cant be empty or default value";
            }
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
            {
                error = "Full name, username, email, phone number and role cant be null or empty";
            }

            var user = new User(id, fullName, username, email, phoneNumber, role, createdAt);

            return (user, error);
        }
    }
}
