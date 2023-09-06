using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementCore.Controllers;
using UserManagementCore.Models;
using Xunit;

namespace UserManagementCore.Tests
{
    public class ApiTestControllerTest
    {
        [Fact]
        public async void GetTask_ReturnTestItemsList()
        {
            //arrange
            var controller = new ApiTestController();
            var ExpectedResult = new List<TestItems> {
            new TestItems { id = 1, idk = "reactJs" },
            new TestItems { id = 2, idk = "This is reactJs" }
            };
            //act
            var result = await controller.GetTask();
          
            //assert            
            var OkObjectResult = Assert.IsType<OkObjectResult>(result);
            //Type checking
            var IsOkResultValue = Assert.IsType<List<TestItems>>(OkObjectResult.Value);
            //value checking

            var model = Assert.IsAssignableFrom<List<TestItems>>(OkObjectResult.Value);
            Assert.NotNull(model);        
            Assert.Equal(ExpectedResult.Count, model.Count);
            Assert.Equivalent(ExpectedResult, model);

         
        }
    }
}
