using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagementCore.ModelConfigurations;
using UserManagementCore.Models;

namespace UserManagementCore.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
           ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
           ApplicationRoleClaim, ApplicationUserToken>
    {

        public DbSet<ApplicationMenu> ApplicationMenus { get; set; }
        public DbSet<ApplicationRoleDetails> RoleDetails { get; set; }
        public DbSet<ApplicationUserDetails> UserDetails { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(); // Enable lazy loading proxies
            // optionsBuilder.UseSqlServer(@"Server=DESKTOP-OC677T4;Database=TestDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("AppUsers");
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                //Each User can have one to one relation in the UserDetails table
                b.HasOne(e => e.UserDetails).WithOne(e => e.User).HasForeignKey<ApplicationUserDetails>(c => c.UserId);

            });

            modelBuilder.Entity<ApplicationUserClaim>(b =>
            {
                b.ToTable("AppUserClaims");
            });
            //IdentityUserLogin<string>
            modelBuilder.Entity<ApplicationUserLogin>(b =>
            {
                b.HasKey(l => new { l.LoginProvider, l.ProviderKey });      //, l.UserId         
                b.ToTable("AppUserLogins");
            });
            //IdentityUserToken<string>
            modelBuilder.Entity<ApplicationUserToken>(b =>
            {
                b.HasKey(l => new { l.UserId, l.LoginProvider, l.Name });
                b.ToTable("AppUserTokens");
            });


            modelBuilder.Entity<ApplicationRole>(b =>
            {
                b.ToTable("AppRoles");
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();

                //Each Role can have many associated RoleDetails
                b.HasMany(e => e.RoleDetails).WithOne(e => e.Role).HasForeignKey(rd => rd.RoleId).IsRequired();
            });       
            //IdentityRoleClaim<string>
            modelBuilder.Entity<ApplicationRoleClaim>(b =>
            {
                b.ToTable("AppRoleClaims");
            });

            //IdentityUserRole<string>
            modelBuilder.Entity<ApplicationUserRole>(b =>
            {
                b.HasKey(l => new { l.UserId, l.RoleId });
                b.ToTable("AppUserRoles");
            });

            modelBuilder.Entity<ApplicationRoleDetails>(b =>
            {
                b.ToTable("AppRoleDetails");
            });
            modelBuilder.Entity<ApplicationUserDetails>(b => {
                b.ToTable("AppUserDetails");
            });
            modelBuilder.Entity<ApplicationMenu>(b => {
                b.HasKey(l => l.Id);
                b.ToTable("ApplicationMenu");
            });

            #region AddConfiguration
           modelBuilder.ApplyConfiguration(new RoleConfiguration());
            #endregion
        }
    }
}
