using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserManagementCore.Controllers;
using UserManagementCore.Interfaces;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using UserManagementCore.Models;
using UserManagementCore.Common;
using UserManagementCore.Repositories;
using Microsoft.AspNetCore.Identity;
using UserManagementCore.Tests.MockServices;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace UserManagementCore.Tests
{
    //https://learn.microsoft.com/en-us/aspnet/web-api/overview/testing-and-debugging/unit-testing-controllers-in-web-api
    public class ApplicationRoleControllerTest
    {
        private readonly ApplicationRoleController _controller;
        private readonly IApplicationRoleService _ApplicationRoleService;
        private readonly ILogger<ApplicationRoleController> _logger;
        protected readonly ApplicationDbContext _context;

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

        //[Fact]
        //public async Task Get_AllRole_Return_Ok() 
        //{

        //    // Arrange
        //    var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
        //    var roleName = "TestRole";

        //    // Act
        //    var result = roleManager.CreateAsync(new IdentityRole(roleName)).Result;

        //    // Assert
        //    Assert.IsTrue(result.Succeeded);
        //    Assert.IsNotNull(roleManager.FindByNameAsync(roleName).Result);

        //    //Act
        //    var result = await _controller.Get();
       
        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    var response = Assert.IsType<Dictionary<string, object>>(okResult.Value);
        //    Assert.Equal(AppStatus.SuccessStatus, response["title"]);

        //    var data = Assert.IsType<List<ApplicationRole>>(response["data"]);
        //    Assert.NotEmpty(data);
        //}
    }
}
