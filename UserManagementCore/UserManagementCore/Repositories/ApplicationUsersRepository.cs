using Microsoft.AspNetCore.Identity;
using UserManagementCore.Contexts;
using UserManagementCore.Interfaces;
using UserManagementCore.Models;

namespace UserManagementCore.Repositories
{
    public class ApplicationUsersRepository : IApplicationUsersRepository
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole>? _roleManager;

        public ApplicationUsersRepository(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager,RoleManager<ApplicationRole> roleManager) 
        { 
         _dbContext = dbContext;
         _userManager = userManager;
         _roleManager = roleManager;
        }

        public async Task CreateUser(ApplicationUser user, String password)
        {
            if (_dbContext.Set<ApplicationUser>().Where(u =>
                 u.Id == user.Id ||
                 u.Email == user.Email ||
                 u.UserName == user.UserName).Any())
            {
                throw new Exception("Duplicate user");
            }
            user.UserName = user.Email;
            await _userManager.CreateAsync(user, password);
            await _userManager.SetLockoutEnabledAsync(user, false);
        }

        public async Task UpdatePassword(Guid userId, String oldPassword, String newPassword)
        {
            var user = await this.GetUser(userId);
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                throw new Exception("MismatchException");
            }
        }

        public async Task UpdateProfile(Guid userId,
           String name )
        {
            var user = await this.GetUser(userId);
            user.UserName = name;         
        }

        public async Task<ApplicationUser> GetUser(Guid userId)
        {
            var result = await this._userManager.FindByIdAsync(userId.ToString());
            if (result == null)                
                throw new Exception("NotFoundException");

            return result;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers(
            String name,          
            String email,
            int pageindex, int pageSize)
        {
            await Task.CompletedTask;
            var result = this._dbContext.Set<ApplicationUser>().Where(
                u =>
                    (name == null || name == "" || u.UserName.StartsWith(name)) &&             
                    (email == null || email == "" || u.UserName == name)
                )
                .Skip(pageindex * pageSize)
                .Take(pageSize);

            if (!result.Any())
                throw new Exception("NotFoundException");

            return result;

        }

        public async Task<string> GetResetPasswordToken(Guid userId)
        {
            var user = await GetUser(userId);
            if (user != null)
            {
                return await _userManager.GeneratePasswordResetTokenAsync(user);
            }
            throw new Exception("NotFoundException");
        }

        public async Task ResetPassword(Guid userId, string token, string password)
        {
            var user = await GetUser(userId);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, token, password);
                if (!result.Succeeded)
                {
                    throw new Exception("MismatchException"); 
                }
            }
            throw new Exception("NotFoundException");


        }

        public async Task<IEnumerable<String>> GetRoles(Guid userId)
        {
            return await _userManager.GetRolesAsync(await GetUser(userId));
        }

        public async Task AddRole(Guid userId, string roleName)
        {
            await _userManager.AddToRoleAsync(await GetUser(userId), roleName);
        }

        public async Task AddRole(Guid userId, Guid roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            await _userManager.AddToRoleAsync(await GetUser(userId), role.Name);
        }

        public async Task RemoveRole(Guid userId, string roleName)
        {
            await _userManager.RemoveFromRoleAsync(await GetUser(userId), roleName);
        }

        public async Task RemoveRole(Guid userId, Guid roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            await _userManager.RemoveFromRoleAsync(await GetUser(userId), role.Name);
        }

    
    }
}
