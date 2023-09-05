using UserManagementCore.Models;

namespace UserManagementCore.Interfaces
{
    public interface ILogins
    {
        Task CreateUser(string email, string password);
        Task<ApplicationUser> GetUser(string email);
    }
}
