using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementCore.Models;

namespace UserManagementCore.Tests
{
    //https://www.learmoreseekmore.com/2022/02/dotnet6-unit-testing-aspnetcore-web-api-using-xunit.html
    public class ApplicationRoleServiceTest
    {
        protected readonly ApplicationDbContext _context;

        public ApplicationRoleServiceTest() 
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
            _context = new ApplicationDbContext(options);

            //_context.Database.EnsureCreated();
        }

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

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
