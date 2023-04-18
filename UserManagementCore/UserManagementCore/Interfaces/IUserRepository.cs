using Microsoft.AspNetCore.Identity;
using UserManagementCore.Models;

namespace UserManagementCore.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<ApplicationUser> Get();
        ApplicationUser GetByEmail(string email);
        Task<IdentityResult> Create(ApplicationUser user, string password);
        Task<IdentityResult> Delete(ApplicationUser user);
        Task<IdentityResult> Update(ApplicationUser user);
        UserManager<ApplicationUser> GetUserManager();
    }
}
