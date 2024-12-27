using Microsoft.AspNetCore.Identity;

namespace TaskManagementSystem.Services
{
    public class UserService
        {
            private readonly PasswordHasher<object> _passwordHasher;

            public UserService()
            {
                _passwordHasher = new PasswordHasher<object>();
            }

            public string HashPassword(string plainPassword)
            {
                return _passwordHasher.HashPassword(null, plainPassword);
            }

            public bool VerifyPassword(string hashedPassword, string plainPassword)
            {
                return _passwordHasher.VerifyHashedPassword(null, hashedPassword, plainPassword)
                       == PasswordVerificationResult.Success;
            }
        }
}
