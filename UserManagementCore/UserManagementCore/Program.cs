using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using UserManagementCore.Filters;
using UserManagementCore.Interfaces;
using UserManagementCore.Models;
using UserManagementCore.Repositories;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = System.Configuration.GetConnectionString("DefaultConnection");


#region Add_DI_ApplicationServices
//Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin();
        });
});
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(opts => {
    opts.Password.RequiredLength = 6;
    opts.Password.RequireNonAlphanumeric = true;
    opts.Password.RequireDigit = true;
    opts.Password.RequireLowercase = true;
    opts.Password.RequireUppercase = true;
    //opts.Password.RequiredUniqueChars = 1;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//add global exception filter upon controller
builder.Services.AddControllers(options => {
    options.Filters.Add(typeof(MyExceptionFilters));
});
/* [Caching]*/
builder.Services.AddResponseCaching();

// TODO: Inject custom validation 
// services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordValidator>();
// services.AddTransient<IUserValidator<AppUser>, CustomUserValidator>();


builder.Services.AddScoped<IRepository<ApplicationRoleDetails>, RepositoryRoleDetails>();
builder.Services.AddScoped<IApplicationRoleService, ApplicationRoleService>();
builder.Services.AddScoped<IApplicationUserService, UserManagementService>();
builder.Services.AddScoped<IApplicationRoleDetailsService, ApplicationRoleDetailsService>();
builder.Services.AddScoped<IApplicationRouteServices, ApplicationRouteServices>();
builder.Services.AddTransient<MyActionFilters>();
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

try
{
    var app = builder.Build();
    app.UseCors("AllowAllOrigins");
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
}
catch (Exception ex)
{
    var message = ex.Message;
}
