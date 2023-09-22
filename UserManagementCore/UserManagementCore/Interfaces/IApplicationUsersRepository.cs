using UserManagementCore.Models;

namespace UserManagementCore.Interfaces
{
    //https://dejanstojanovic.net/aspnet/2019/september/unit-testing-repositories-in-aspnet-core-with-xunit-and-moq/
    //https://github.com/dejanstojanovic/WebShop/blob/master/src/WebShop.Users.Data/Repositories/IApplicationUsersRepository.cs
    public interface IApplicationUsersRepository
    {
        Task CreateUser(ApplicationUser user, String password);
        Task UpdatePassword(Guid userId, String oldPassword, String newPassword);
        Task<String> GetResetPasswordToken(Guid userId);
        Task ResetPassword(Guid userId, String token, String password);
        Task UpdateProfile(
            Guid userId,
            String name );
        Task<ApplicationUser> GetUser(Guid userId);
        Task<IEnumerable<ApplicationUser>> GetUsers(
            String name,          
            String email,
            int pageindex, int pageSize);
        Task<IEnumerable<String>> GetRoles(Guid userId);
        Task AddRole(Guid userId, String roleName);
        Task AddRole(Guid userId, Guid roleId);
        Task RemoveRole(Guid userId, String roleName);
        Task RemoveRole(Guid userId, Guid roleId);
    }
}
