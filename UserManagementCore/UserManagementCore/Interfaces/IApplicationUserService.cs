using UserManagementCore.Models;

namespace UserManagementCore.Interfaces
{
    public interface IApplicationUserService
    {
        public Task CreateRolesAndUsers();
        public Task<IEnumerable<ApplicationUser>> GetUserList();
    }
}
