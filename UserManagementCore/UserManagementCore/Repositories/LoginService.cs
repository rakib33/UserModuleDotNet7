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

        public Task<ApplicationUser> GetUser(string email)
        {
            throw new NotImplementedException();
        }
    }
}
