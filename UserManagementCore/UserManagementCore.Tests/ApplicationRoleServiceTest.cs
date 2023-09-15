using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using UserManagementCore.Contexts;
using UserManagementCore.Models;
using UserManagementCore.Repositories;

namespace UserManagementCore.Tests
{
    //https://www.learmoreseekmore.com/2022/02/dotnet6-unit-testing-aspnetcore-web-api-using-xunit.html
    public class ApplicationRoleServiceTest
    {
        protected readonly ApplicationDbContext _context;
        private readonly IServiceProvider serviceProvider;
        
        public ApplicationRoleServiceTest()
        {

            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase(databaseName: "UserManagement_db"));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            serviceProvider = services.BuildServiceProvider();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;

            //_context = new ApplicationDbContext(options);
            //_context.Database.EnsureCreated();
        }

        //[Fact]
        //public async Task TestGetRoleList()
        //{
        //    // Arrange
        //    var roleManager = serviceProvider.GetService<RoleManager<ApplicationRole>>();
        //    var roleName = "TestRole";
        //    var roleService = new ApplicationRoleService(roleManager);
        //    // Act
        //    // var result = roleManager.CreateAsync(new IdentityRole(roleName)).Result;
        //    var result = roleService.GetRoleList();

        //    // Assert     
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    var response = Assert.IsType<Dictionary<string, object>>(okResult.Value);
        //  //  Assert.Equal(AppStatus.SuccessStatus, response["title"]);

        //    var data = Assert.IsType<List<ApplicationRole>>(response["data"]);
        //    Assert.NotEmpty(data);
        //}
        //[Fact]
        //public async Task GetAllAsync_ReturnTodoCollection()
        //{
        //    /// Arrange
        //    _context.Todo.AddRange(MockData.TodoMockData.GetTodos());
        //    _context.SaveChanges();

        //    var sut = new TodoService(_context);

        //    /// Act
        //    var result = await sut.GetAllAsync();

        //    /// Assert
        //    result.Should().HaveCount(TodoMockData.GetTodos().Count);
        //}
      
        //public void TestCreateRole()
        //{
        //    // Arrange
        //    var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
        //    var roleName = "TestRole";

        //    // Act
        //    var result = roleManager.CreateAsync(new IdentityRole(roleName)).Result;

        //    // Assert
        //    Assert.IsTrue(result.Succeeded);
        //    Assert.IsNotNull(roleManager.FindByNameAsync(roleName).Result);
        //}
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
