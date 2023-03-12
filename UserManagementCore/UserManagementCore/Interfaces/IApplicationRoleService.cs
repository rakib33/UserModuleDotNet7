using UserManagementCore.Models;

namespace UserManagementCore.Interfaces
{
    public interface IApplicationRoleService
    {
        public Task<IEnumerable<ApplicationRole>> GetOnlyRoleList();
        public Task<IEnumerable<ApplicationRole>> GetRoleList();
        public Task<IEnumerable<ApplicationRole>> GetRolesWithDetails();
        public Task<IEnumerable<ApplicationRole>> GetRolesWithUsers();
        public Task<IEnumerable<ApplicationRole>> GetRolesWithClaims();
        public Task<ApplicationRole> GetRole(string roleId, string roleName);
    }
}
