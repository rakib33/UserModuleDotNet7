using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagementCore.Interfaces;
using UserManagementCore.Models;

namespace UserManagementCore.Repositories
{
    public class ApplicationRoleService : IApplicationRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        public ApplicationRoleService(RoleManager<ApplicationRole> roleManager)
        {
            this._roleManager = roleManager;
        }

        public async Task<IEnumerable<ApplicationRole>> GetOnlyRoleList()
        {
            try
            {
                return await _roleManager.Roles.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ApplicationRole>> GetRoleList()
        {
            try
            {
                return await _roleManager.Roles.Include(userRole => userRole.UserRoles)
                                                                       .Include(roleDetails => roleDetails.RoleDetails)
                                                                       .Include(roleClaims => roleClaims.RoleClaims).ToListAsync();
            }            
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<ApplicationRole>> GetRolesWithDetails()
        {
            try
            {
                return await _roleManager.Roles.Include(u => u.RoleDetails).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ApplicationRole>> GetRolesWithUsers()
        {
            try
            {
                return await _roleManager.Roles.Include(u => u.UserRoles).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ApplicationRole>> GetRolesWithClaims()
        {
            try
            {
                return await _roleManager.Roles.Include(u => u.RoleClaims).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApplicationRole> GetRole(string roleId, string roleName)
        {

            if (!string.IsNullOrEmpty(roleId) && !string.IsNullOrEmpty(roleName))
                return await _roleManager.Roles.Where(u => u.Id == roleId && u.Name == roleName)
                                                                        .Include(userRole => userRole.UserRoles)
                                                                        .Include(roleDetails => roleDetails.RoleDetails)
                                                                        .Include(roleClaims => roleClaims.RoleClaims).FirstOrDefaultAsync();

            else if (!string.IsNullOrEmpty(roleId))
                return await _roleManager.Roles.Where(u => u.Id == roleId)
                                                                        .Include(userRole => userRole.UserRoles)
                                                                        .Include(roleDetails => roleDetails.RoleDetails)
                                                                        .Include(roleClaims => roleClaims.RoleClaims).FirstOrDefaultAsync();
            else if (!string.IsNullOrEmpty(roleName))
                return await _roleManager.Roles.Where(u => u.Name == roleName)
                                                                        .Include(userRole => userRole.UserRoles)
                                                                        .Include(roleDetails => roleDetails.RoleDetails)
                                                                        .Include(roleClaims => roleClaims.RoleClaims).FirstOrDefaultAsync();
            else
                return null;
        }
    }
}
