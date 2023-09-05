using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementCore.Controllers;
using UserManagementCore.Interfaces;

namespace UserManagementCore.Tests
{
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
        }

     
        [Fact]
        public void Index_ReturnLoginViewModel() 
        { 
         var result = (_loginsController.Index() as ViewResult);
            Assert.NotNull(result);
            Assert.Equal("Login",result.ViewName,ignoreCase:true);
        }
    }
}
