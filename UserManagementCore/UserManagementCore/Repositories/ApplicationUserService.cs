using UserManagementCore.Interfaces;
using UserManagementCore.Models;

namespace UserManagementCore.Repositories
{
    public class ApplicationUserService : IApplicationUserService
    {
        public Task CreateRolesAndUsers()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationUser>> GetUserList()
        {
            throw new NotImplementedException();
        }
    }
}
