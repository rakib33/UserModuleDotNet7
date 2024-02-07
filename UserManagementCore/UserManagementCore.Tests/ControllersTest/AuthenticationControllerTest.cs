using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using UserManagementCore.Controllers;
using UserManagementCore.Interfaces;
using UserManagementCore.Models;
using Xunit;

namespace UserManagementCore.Tests.ControllersTest
{
    [Collection("Authentication")]
    public class AuthenticationControllerTest
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly Mock<RoleManager<ApplicationRole>> _roleManager;
        private readonly Mock<IConfiguration> _configuration;
        private  AuthenticationController _controller;

        public AuthenticationControllerTest()
        {
            // Arrange
            _configuration = new Mock<IConfiguration>(MockBehavior.Strict);
            _userManager = new Mock<UserManager<ApplicationUser>>(MockBehavior.Strict);
            _roleManager = new Mock<RoleManager<ApplicationRole>>(MockBehavior.Strict);
       
        }

        [Fact]
        public async Task CreateUserWithRole_Returns201Created()
        {
            // Arrange
            //var userManagerMock = new Mock<UserManager<ApplicationUser>>(MockBehavior.Strict);
            //var roleManagerMock = new Mock<RoleManager<IdentityRole>>(MockBehavior.Strict);
            //var configurationMock = new Mock<IConfiguration>(MockBehavior.Strict);

            _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
           
            _userManager.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _roleManager.Setup(m => m.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var modelView = Mock.Of<RegisterUser>(x => x.Email == "a@gmail.com" && x.Password == "123");
            var model = Mock.Of<ApplicationUser>(x => x.PasswordHash == "123");

            // _logins.Setup(x => x.GetUser(It.IsAny<string>())).ReturnsAsync(model);
            _controller = new AuthenticationController(_userManager.Object, _roleManager.Object, _configuration.Object);
            // Act
            var result = await _controller.RegisterUser(modelView,"visitor") as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);

            _userManager.Verify(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            _userManager.Verify(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            _roleManager.Verify(m => m.RoleExistsAsync(It.IsAny<string>()), Times.Once);
            _configuration.VerifyGet(x => x["DefaultRole"], Times.AtLeastOnce);

        }

        [Fact]
        public async Task RegisterUser_SuccessfulRegistration_Returns201Created()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(/* pass required parameters */);
            var roleManagerMock = new Mock<RoleManager<ApplicationRole>>(/* pass required parameters */);
            var configurationMock = new Mock<IConfiguration>();

            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            roleManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            var authenticationController = new AuthenticationController(
                userManagerMock.Object,
                roleManagerMock.Object,
                configurationMock.Object
            );

            // Act
            var registerUser = new RegisterUser { /* set properties */ };
            var result = await authenticationController.RegisterUser(registerUser, "Visitor");

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            var statusCodeResult = (StatusCodeResult)result;
            Assert.Equal(StatusCodes.Status201Created, statusCodeResult.StatusCode);
        }
    }
}
