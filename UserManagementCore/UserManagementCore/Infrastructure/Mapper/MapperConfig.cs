using AutoMapper;
using UserManagementCore.Models;
using UserManagementEntityModel.ViewModel;

namespace UserManagementCore.Infrastructure.Mapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<ApplicationUser, vm_ApplicationUser>().ReverseMap();
        }
    }
}
