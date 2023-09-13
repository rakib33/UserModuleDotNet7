using UserManagementCore.Interfaces;
using UserManagementCore.Models;

namespace UserManagementCore.Repositories
{
    public class LoginService : ILogins
    {
        public Task CreateUser(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser>? GetUser(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                ApplicationUser user = new ApplicationUser { Email = email, PasswordHash = "123" };
                return Task.FromResult(user);
            }
            else
                return null;
        }
    }
}
