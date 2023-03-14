using UserManagementCore.Models;

namespace UserManagementCore.Interfaces
{
    public interface IApplicationUserService
    {
        public Task CreateRolesandUsers();
        public Task<IEnumerable<ApplicationUser>> GetUserList();
    }
}
