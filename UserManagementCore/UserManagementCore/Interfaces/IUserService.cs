using Microsoft.AspNetCore.Identity;
using UserManagementCore.Models;
using UserManagementEntityModel.ViewModel;

namespace UserManagementCore.Interfaces
{
    public interface IUserService
    {
        IQueryable<ApplicationUser> Get();
        ApplicationUser GetByEmail(string email);
        Task<IdentityResult> Create(ApplicationUser user, string password);
        Task<IdentityResult> Delete(ApplicationUser user);
        Task<IdentityResult> Update(ApplicationUser user);
        Task<IdentityResult> ValidatePassword(ApplicationUser user, string password);
        Task<IdentityResult> ValidateUser(ApplicationUser user);
        string HashPassword(ApplicationUser user, string password);
        Task SignOutAsync();
        Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool lockoutOnFailure,
            bool isPersistent);


    }
}
