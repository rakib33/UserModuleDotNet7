using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserManagementCore.Controllers;
using UserManagementCore.Interfaces;
using UserManagementCore.Models;
using Xunit;

namespace UserManagementCore.Tests
{

    [Collection("Login")]
    public class LoginTest
    {
        private readonly Mock<IHttpContextAccessor> _accessor;
        private readonly LoginsController _loginsController;
        private readonly Mock<ILogins> _logins;

        public LoginTest()
        {
            _logins = new Mock<ILogins>();
            var session = Mock.Of<ISession>();
            var httpContext = Mock.Of<HttpContext>(x => x.Session == session);
            _accessor = new Mock<IHttpContextAccessor>();
            _accessor.Setup(x => x.HttpContext).Returns(httpContext);
            _loginsController = new LoginsController(_logins.Object, _accessor.Object);
        }

     
        [Fact]
        public void Index_ReturnLoginViewModel() 
        { 
            var result = (_loginsController.Index() as ViewResult);
            Assert.NotNull(result);
            Assert.Equal("Login",result.ViewName,ignoreCase:true);
        }

        [Fact]
        public async Task Login_ModelStateIsValidTest_RetrurnLoginViewModel() 
        {
            var expectedViewName = "Login";
            var model = new LoginViewModel(); // Mock.Of<LoginViewModel>();
            _loginsController.ModelState.AddModelError("Test","Test");
            var result = await _loginsController.Login(model) as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(expectedViewName, result.ViewName,ignoreCase:true);
            Assert.Equal(model,result.Model);
        }

    
        [Fact]
        public async Task Login_GivenCorrectPassword_RedirectToLoginAction()
        {

            var modelView = Mock.Of<LoginViewModel>(x => x.Email == "a@gmail.com" && x.Password == "123");
            var model = Mock.Of<ApplicationUser>(x => x.PasswordHash == "123");

            _logins.Setup(x => x.GetUser(It.IsAny<string>())).ReturnsAsync(model);

            var result = await _loginsController.Login(modelView);

            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task Login_GivenCorrectPassword_RedirectUrl()
        {

            var modelView = Mock.Of<LoginViewModel>(x => x.Email == "a@gmail.com" && x.Password == "123" && x.ReturnUrl=="abc.com");
            var model = Mock.Of<ApplicationUser>(x => x.PasswordHash == "123");

            _logins.Setup(x => x.GetUser(It.IsAny<string>())).ReturnsAsync(model);

            var result = await _loginsController.Login(modelView);

            Assert.NotNull(result);
            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public async Task Login_GivenInvalidCredential_RedirectToLoginViewModel()
        {
            var expectedViewName = "Login";
            var modelView = Mock.Of<LoginViewModel>(x => x.Email == "" && x.Password == "");
            var model = Mock.Of<ApplicationUser>(x => x.PasswordHash == "123");

            _logins.Setup(x => x.GetUser(It.IsAny<string>())).ReturnsAsync(model);

            var result = await _loginsController.Login(modelView) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("Login", result.ViewName, ignoreCase: true);
            Assert.Equal(modelView, result.Model);
        }
    }
}
