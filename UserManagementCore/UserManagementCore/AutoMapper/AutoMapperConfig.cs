using UserManagementCore.Models;
using UserManagementEntityModel.ViewModel;
using AutoMapper;
namespace UserManagementCore.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {           
            CreateMap<ApplicationUser,vm_ApplicationUser>().ReverseMap();
        }
    }
}
