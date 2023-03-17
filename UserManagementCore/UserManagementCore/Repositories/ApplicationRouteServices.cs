using Azure.Core;
using UserManagementCore.Interfaces;
using UserManagementCore.Models;

namespace UserManagementCore.Repositories
{
    public class ApplicationRouteServices : IApplicationRouteServices
    {
        public ApplicationMenu CreateApplicationRoute(ApplicationMenu applicationMenu)
        {
            if (applicationMenu is null)
            {
                return null;
            }
            return applicationMenu;
        }
    }
}
