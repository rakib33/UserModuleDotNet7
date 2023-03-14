using UserManagementCore.Interfaces;
using UserManagementCore.Models;

namespace UserManagementCore.Repositories
{
    public class ApplicationRoleDetailsService : IApplicationRoleDetailsService
    {
        private readonly IRepository<ApplicationRoleDetails> _applicationRoleDetails;

        public ApplicationRoleDetailsService(IRepository<ApplicationRoleDetails> applicationRoleDetails)
        {
            _applicationRoleDetails = applicationRoleDetails;
        }
        //Get ApplicationRoleDetails Details By ApplicationRoleDetails Id  
        public IEnumerable<ApplicationRoleDetails> GetRoleById(int id)
        {
            return _applicationRoleDetails.GetAll().Where(x => x.Id == id).ToList();
        }
        //GET All Perso Details   
        public IEnumerable<ApplicationRoleDetails> GetAllApplicationRoleDetailss()
        {
            try
            {
                return _applicationRoleDetails.GetAll().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Get ApplicationRoleDetails by ApplicationRoleDetails Name  
        //public ApplicationRoleDetails GetApplicationRoleDetailsByUserName(string UserName)
        //{
        //    return _applicationRoleDetails.GetAll().Where(x => x.UserEmail == UserName).FirstOrDefault();
        //}
        //Add ApplicationRoleDetails  
        public async Task<ApplicationRoleDetails> AddApplicationRoleDetails(ApplicationRoleDetails applicationRoleDetails)
        {
            return await _applicationRoleDetails.Create(applicationRoleDetails);
        }
        //Delete ApplicationRoleDetails   
        public bool DeleteApplicationRoleDetails(int id)
        {

            try
            {
                var DataList = _applicationRoleDetails.GetAll().Where(x => x.Id == id).ToList();
                foreach (var item in DataList)
                {
                    _applicationRoleDetails.Delete(item);
                }
                return true;
            }
            catch (Exception)
            {
                return true;
            }

        }
        //Update ApplicationRoleDetails Details  
        public bool UpdateApplicationRoleDetails(List<ApplicationRoleDetails> DataList)
        {
            try
            {
                foreach (var item in DataList)
                {
                    _applicationRoleDetails.Update(item);
                }
                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
