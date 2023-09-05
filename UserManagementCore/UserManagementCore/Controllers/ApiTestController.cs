using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementCore.Models;

namespace UserManagementCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiTestController : ControllerBase
    {
        [HttpGet("GetTask")]
        public async Task<ActionResult> GetTask()  //IEnumerable<WeatherForecast>
        {
            List<TestItems> tems = new List<TestItems>();
            tems.Add(new TestItems { id = 1, idk = "reactJs" });
            tems.Add(new TestItems { id = 2, idk = "This is reactJs" });
            await Task.Delay(500);
            //return Ok(new { data = tems, message="This is Test Items" }); // data stored in testItems object
            return Ok(tems); // direct array render so in reactjs can catch this
        }

        [HttpGet("GetPropertiesWithArrayTask")]
        public async Task<ActionResult> GetPropertiesWithArrayListTask()  //IEnumerable<WeatherForecast>
        {
            List<TestItems> tems = new List<TestItems>();
            tems.Add(new TestItems { id = 1, idk = "reactJs data array 2" });
            tems.Add(new TestItems { id = 2, idk = "This is reactJs data array 2" });
            await Task.Delay(500);
            return Ok(new { TestList = tems, message = "This is Test Items of data array 2" }); // data stored in testItems object 

        }
    }
}
