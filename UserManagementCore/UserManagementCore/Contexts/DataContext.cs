using Microsoft.EntityFrameworkCore;
using UserManagementCore.Models;

namespace UserManagementCore.DataContext
{
    public class DataContext : DbContext
    {
        public DbSet<ApplicationMenu> ApplicationMenus { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=turnmeup;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}
