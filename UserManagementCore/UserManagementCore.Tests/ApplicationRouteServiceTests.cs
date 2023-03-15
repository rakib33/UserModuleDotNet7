using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementCore.Models;
using UserManagementCore.Repositories;

namespace UserManagementCore.Tests
{
    public class ApplicationRouteServiceTests
    {
        [Fact]
        public void ShouldReturnAppRouteResultWithRequestValues()
        {
            var processor = new ApplicationRouteServices();
            ApplicationMenu request = new ApplicationMenu { 
              MenuName= "app",
              Id= 1
            };
            // Act  
            ApplicationMenu response = processor.CreateApplicationRoute(request);

            // Assert  
            Assert.NotNull(response);
            Assert.Equal(request.MenuName, response.MenuName);
            Assert.Equal(request.Id, response.Id);
        
        }
    }
}
