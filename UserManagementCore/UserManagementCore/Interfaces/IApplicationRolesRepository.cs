using Microsoft.AspNetCore.Identity;
using UserManagementCore.Models;
using UserManagementEntityModel.Models;

namespace UserManagementCore.Interfaces
{
    public interface IApplicationRolesRepository
    {
        Task AddRole(ApplicationRole role);
        Task<ApplicationRole> GetRole(String name);
        Task<ApplicationRole> GetRole(Guid roleId);
        Task RemoveRole(String name);
        Task RemoveRole(Guid roleId);
        Task AddClaim(String roleName, RoleClaim claim);
        Task RemoveClaim(String roleName, RoleClaim claim);
        Task<IEnumerable<RoleClaim>> GetClaims(string roleName);
    }
}
