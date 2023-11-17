using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagementCore.Models;

namespace UserManagementCore.ModelConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData(
                new ApplicationRole { 
                  Name = "Visitor",
                  NormalizedName = "VISITOR",
                  Id   =    "0"
                },
                new ApplicationRole 
                { 
                 Name = "Administrator",
                 NormalizedName="ADMINISTRATOR",
                 Id = "1"
                }
                );
        }
    }
}
