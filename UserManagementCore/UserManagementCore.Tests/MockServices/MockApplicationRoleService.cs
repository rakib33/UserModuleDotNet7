using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementCore.Interfaces;
using UserManagementCore.Models;

namespace UserManagementCore.Tests.MockServices
{
    public class MockApplicationRoleService : IApplicationRole
    {

        private readonly RoleManager<ApplicationRole> _roleManager;
        public MockApplicationRoleService(RoleManager<ApplicationRole> roleManager)
        {
            this._roleManager = roleManager;
        }
        public Task<IEnumerable<ApplicationRole>> GetOnlyRoleList()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationRole> GetRole(string roleId, string roleName)
        {
            throw new NotImplementedException();
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

        public Task<IEnumerable<ApplicationRole>> GetRolesWithClaims()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationRole>> GetRolesWithDetails()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationRole>> GetRolesWithUsers()
        {
            throw new NotImplementedException();
        }
    }
}
