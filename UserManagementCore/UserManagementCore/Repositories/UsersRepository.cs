using Microsoft.AspNetCore.Identity;
using UserManagementCore.Interfaces;
using UserManagementCore.Models;

namespace UserManagementCore.Repositories
{
    public class UsersRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IQueryable<ApplicationUser> Get() => _userManager.Users;

        public ApplicationUser GetByEmail(string email) => _userManager.Users.First(u => u.Email == email);

        public Task<IdentityResult> Create(ApplicationUser user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> Delete(ApplicationUser user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> Update(ApplicationUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public UserManager<ApplicationUser> GetUserManager()
        {
            return _userManager;
        }
    }
}
