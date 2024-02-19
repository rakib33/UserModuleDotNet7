using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserManagement.EmailService.Models;
using UserManagement.EmailService.Services;
using UserManagementCore.Contexts;
using UserManagementCore.Filters;
using UserManagementCore.Infrastructure.ErrorHandler;
using UserManagementCore.Infrastructure.Mapper;
using UserManagementCore.Interfaces;
using UserManagementCore.Models;
using UserManagementCore.Repositories;
using UserManagementEntityModel.Models.Authentication.JWTToken;

var builder = WebApplication.CreateBuilder(args);

#region Add_DI_ApplicationServices


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(5),
         errorNumbersToAdd: null);
    }));


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

#region Configure JWT authentication

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

//Adding authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.ValidIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});


#endregion



//Add Mapping
builder.Services.AddAutoMapper(typeof(MapperConfig));

//add global exception filter upon controller
builder.Services.AddControllers(options => {
    options.Filters.Add(typeof(MyExceptionFilters));
});
/* [Caching]*/
builder.Services.AddResponseCaching();

builder.Services.AddScoped<IRepository<ApplicationRoleDetails>, RepositoryRoleDetails>();
builder.Services.AddScoped<IApplicationRole, ApplicationRoleService>();
builder.Services.AddScoped<IApplicationUserService, UserManagementService>();
builder.Services.AddScoped<IApplicationRoleDetailsService, ApplicationRoleDetailsService>();
builder.Services.AddScoped<IApplicationRouteServices, ApplicationRouteServices>();
builder.Services.AddScoped<IApplicationUsersRepository, ApplicationUsersRepository>();
builder.Services.AddScoped<IErrorHandler, ErrorHandler>();
builder.Services.AddScoped<IUserRepository, UsersRepository>();
builder.Services.AddScoped<IUserService, UsersService>();
builder.Services.AddScoped<ILogins, LoginService>();
builder.Services.AddTransient<MyActionFilters>();

//Add email Config
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailService, EmailService>();
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
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors(x=>x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.Run();
}
catch (Exception ex)
{
    var message = ex.Message;
}
