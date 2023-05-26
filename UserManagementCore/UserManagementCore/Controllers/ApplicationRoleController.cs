using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementCore.Common;
using UserManagementCore.Filters;
using UserManagementCore.Interfaces;
using UserManagementCore.Models;
using UserManagementCore.Repositories;

namespace UserManagementCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationRoleController : ControllerBase
    {

        private readonly IApplicationRoleService _ApplicationRoleService;
        private readonly ILogger<ApplicationRoleController> _logger;   
        public ApplicationRoleController(IApplicationRoleService ApplicationRoleService, ILogger<ApplicationRoleController> logger)
        {
            _ApplicationRoleService = ApplicationRoleService;
            _logger = logger;
        }

        [HttpGet("GetTask")]
        public async Task<ActionResult> GetTask()  //IEnumerable<WeatherForecast>
        {
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();
            List<TestItems> tems = new List<TestItems>();
            tems.Add(new TestItems { id = 1, idk = "reactJs" });
            tems.Add(new TestItems { id = 2, idk= "This is reactJs" });
          //  return Ok(new { data = tems, message="This is Test Items" }); // data stored in testItems object 
            return Ok( tems); // direct array render so in reactjs can catch this
        }
        /// <summary>
        /// api/ApplicationRole 
        /// api/ApplicationRole/RoleList
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HttpGet("RoleList")]
        [ResponseCache(Duration = 60)] //60 sec
       // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(MyActionFilters))]
        public async Task<ActionResult<List<ApplicationRole>>> Get()
        {
            try
            {
                // throw new ApplicationException();
                //_logger.LogInformation("[ApplicationRole]-> Get event fire.");
                var roleList = await _ApplicationRoleService.GetRoleList();
                return Ok(new { title = AppStatus.SuccessStatus, data = roleList });

            }
            catch (Exception ex)
            {
                return Ok(new { title = AppStatus.ErrorStatus, data = ex });
            }

        }

        /// <summary>
        /// api/ApplicationRole/1 [just put the id no ]
        /// api/ApplicationRole/RoleById/1
        /// ApplicationRole/RoleByName/1 [Id is 0 and roleName has 1]
        /// api/ApplicationRole/1/Admin [url: Id = 1 roleName = Admin]
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
       // [HttpGet("{Id:int}")]      
        [HttpGet("{Id}")]
        [HttpGet("RoleById/{Id}")]
        [HttpGet("RoleByName/{roleName}")]
        [HttpGet("{Id:int}/{roleName}")]
        [ResponseCache(Duration = 60)] //60 sec
        public async Task<ActionResult<ApplicationRole>> Get(string Id, string roleName)
        {
            try
            {
                _logger.LogInformation("[ApplicationRole]-> Get event fire.");
                var _data = await _ApplicationRoleService.GetRole(Id, roleName);
                if (_data != null)
                    return Ok(new { title = AppStatus.SuccessStatus, data = await _ApplicationRoleService.GetRoleList() });
                else
                    return NotFound();
                #region NotFound
                //{
                //    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                //     "title": "Not Found",
                //      "status": 404,
                //       "traceId": "00-404448690cd7664ea8dd217d33dfc8f1-cd89ab2c1878894f-00"
                //}
                #endregion
            }
            catch (Exception ex)
            {
                return Ok(new { title = AppStatus.ErrorStatus, data = ex });
            }

        }

        [HttpPost]
        public ActionResult Post([FromBody] ApplicationRole applicationRole)
        {
            #region ModelValidationWithout[ApiController]
            //[ApiController] attribute manage this ModelState validation
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            //JsonResult of return BadRequest is:
            //{
            //    "errors": {
            //        "AllAccess": [
            //            "The AllAccess field is required."
            //        ],
            //        "Description": [
            //            "The Description field is required."
            //        ]
            //  },
            // "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            //"title": "One or more validation errors occurred.",
            //"status": 400,
            //"traceId": "00-6649cf20523b8e429c1cef9cc260dc2e-7bd846c1e274eb44-00"
            //}
            #endregion

            try
            {
                _logger.LogInformation("[ApplicationRole]-> Get event fire.");
                return Ok(new { title = AppStatus.SuccessStatus, data = applicationRole });

            }
            catch (Exception ex)
            {
                return Ok(new { title = AppStatus.ErrorStatus, data = ex });
            }
            return null;
        }

        [HttpPut]
        public void Put()
        {

        }

        [HttpDelete]
        public void Delete()
        {

        }
    }
}
