using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UserManagementCore.Interfaces;
using UserManagementCore.Models;
using UserManagementEntityModel.Models;

namespace UserManagementCore.Repositories
{
    public class ApplicationRolesRepository: IApplicationRolesRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public ApplicationRolesRepository(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task AddClaim(string roleName, RoleClaim claim)
        {
            await _roleManager.AddClaimAsync(await GetRole(roleName), new Claim(claim.ClaimType, claim.ClaimValue));
        }

        public async Task AddRole(ApplicationRole role)
        {
            await _roleManager.CreateAsync(role);
        }

        public Task AddRole(IdentityRole role)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RoleClaim>> GetClaims(string roleName)
        {
            return (await _roleManager.GetClaimsAsync(await GetRole(roleName))).Select(c => new RoleClaim() { ClaimType = c.ValueType, ClaimValue = c.Value });
        }

        public async Task<ApplicationRole> GetRole(string name)
        {
            return await _roleManager.FindByNameAsync(name);
        }

        public async Task<ApplicationRole> GetRole(Guid roleId)
        {
            return await _roleManager.FindByIdAsync(roleId.ToString());
        }

        public async Task RemoveClaim(string roleName, RoleClaim claim)
        {
            await _roleManager.RemoveClaimAsync(await GetRole(roleName), new Claim(claim.ClaimType, claim.ClaimValue));
        }

        public async Task RemoveRole(string name)
        {
            await _roleManager.DeleteAsync(await GetRole(name));
        }

        public async Task RemoveRole(Guid roleId)
        {
            await _roleManager.DeleteAsync(await GetRole(roleId));
        }
    }
}
