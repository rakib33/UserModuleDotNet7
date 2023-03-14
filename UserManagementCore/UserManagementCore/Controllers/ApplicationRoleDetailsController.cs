using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Xml;
using UserManagementCore.Interfaces;
using UserManagementCore.Models;
using UserManagementCore.Repositories;

namespace UserManagementCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationRoleDetailsController : ControllerBase
    {
        private readonly IApplicationRoleDetailsService _ApplicationRoleDetailsService;      
        private readonly UserManagementService _UserManagementService;
        // private readonly IRepository<ApplicationRoleDetails> _ApplicationRoleDetails;
        private readonly ILogger<ApplicationRoleDetailsController> _logger;

        public ApplicationRoleDetailsController(ILogger<ApplicationRoleDetailsController> logger, IApplicationRoleDetailsService ApplicationRoleDetailsService, UserManagementService UserManagementService)
        {
            _ApplicationRoleDetailsService = ApplicationRoleDetailsService;
            //  _ApplicationRoleDetails = ApplicationRoleDetails;
            _UserManagementService = UserManagementService;
            _logger = logger;
        }
        //Add Person  
        [HttpPost("AddRoleDetails")]
        public async Task<Object> AddRoleDetails([FromBody] ApplicationRoleDetails roleDetails)
        {
            try
            {
                _logger.LogInformation("[Add role details called]");
                await _ApplicationRoleDetailsService.AddApplicationRoleDetails(roleDetails);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("[AddRoleDetails]-> Error: " + ex);
                return false;
            }
        }
        //Delete Person  
        [HttpDelete("DeleteRoleDetails")]
        public bool DeleteRoleDetails(int id)
        {
            try
            {
                _ApplicationRoleDetailsService.DeleteApplicationRoleDetails(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //Delete Person  
        [HttpPut("UpdateRoleDetails")]
        public bool UpdateRoleDetails(List<ApplicationRoleDetails> dataList)
        {
            try
            {
                _ApplicationRoleDetailsService.UpdateApplicationRoleDetails(dataList);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        ////GET All Person by Name  
        //[HttpGet("GetAllPersonByName")]
        //public Object GetAllPersonByName(string UserEmail)
        //{
        //    var data = _personService.GetPersonByUserName(UserEmail);
        //    var json = JsonConvert.SerializeObject(data, Formatting.Indented,
        //        new JsonSerializerSettings()
        //        {
        //            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        //        }
        //    );
        //    return json;
        //}

        //GET All Person  
        //https://localhost:44317/api/ApplicationRoleDetails/GetAllRoleDetails
        [HttpGet("GetAllRoleDetails")]
        public Object GetAllRoleDetails()
        {
            var data = _ApplicationRoleDetailsService.GetAllApplicationRoleDetailss();
            var json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                }
            );
            return json;
        }


        //https://localhost:44317/api/ApplicationRoleDetails/AddUserRole
        [HttpGet("AddUserRole")]
        public async Task<Object> AddUserRole()
        {
            try
            {
                await _UserManagementService.CreateRolesandUsers();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        //https://localhost:44317/api/ApplicationRoleDetails/GetAllUser
        [HttpGet("GetAllUser")]
        public async Task<IEnumerable<ApplicationUser>> GetAllUser()
        {
            try
            {
                var userlist = await _UserManagementService.GetUserList();
                return userlist;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
