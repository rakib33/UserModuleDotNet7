using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserManagementCore.Controllers;
using UserManagementCore.Interfaces;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace UserManagementCore.Tests
{
    //https://learn.microsoft.com/en-us/aspnet/web-api/overview/testing-and-debugging/unit-testing-controllers-in-web-api
    public class ApplicationRoleControllerTest
    {
        private readonly ApplicationRoleController _controller;
        private readonly IApplicationRoleService _ApplicationRoleService;
        private readonly ILogger<ApplicationRoleController> _logger;
    
        public ApplicationRoleControllerTest()
        {
            _controller = new ApplicationRoleController(_ApplicationRoleService, _logger);

        }


        [Fact]
        public async void Get_WhenCalled_ReturnsOkResult()
        {
            //Arrange

            // Act
            var actionResult = await _controller.GetTask(); 
            // Assert
            Assert.IsType<OkObjectResult>(actionResult as OkObjectResult);
        }
    }
}
