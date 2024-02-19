using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagement.EmailService.Models;
using UserManagement.EmailService.Services;
using UserManagementCore.Common;
using UserManagementCore.Models;
using UserManagementEntityModel.Models.Authentication.Login;
using UserManagementEntityModel.Models.Authentication.SignUp;

namespace UserManagementCore.Controllers
{
    //https://www.youtube.com/watch?v=J8pxfxLF41g&list=PLX4n-znUpc2b19AoYa4BMuhGuRnZItJK_&index=2
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
         private readonly IConfiguration _configuration;

        private readonly IEmailService _emailService;
        public AuthenticationController(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, IEmailService emailService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody]RegisterUser registerUser, string role) 
        {
            //Check user exits
            var userExits = await _userManager.FindByEmailAsync(registerUser.Email);
            if(userExits != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new Response { Status = "Error", Message = "User already exists!" });             
            }
            //Add the user in the database
            ApplicationUser user = new()
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.UserName
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerUser.Password);
            if(result.Succeeded)
            {
                // Your role creation logic (replace with actual role name)
                var roleName = "Visitor";

                // Creating the role if it doesn't exist
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    ApplicationRole applicationRole = new()
                    {
                        Name = roleName,
                    };
                    await _roleManager.CreateAsync(applicationRole);
                }

                // Assigning the user to the role
                await _userManager.AddToRoleAsync(user, roleName);
                
                //Add Token to Verify the email....
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token, email = user.Email }, Request.Scheme);
                var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink!);
                try
                {
                    _emailService.SendMail(message);
                }
                catch(Exception ex)
                {
                    string msg = ex.Message;
                }
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = $"User Created & Email Sent to {user.Email} Successfully!" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User Failed to Create", Errors= result.Errors });
            }
          

        }

        [HttpGet]
        public IActionResult TestEmail()
        {
            var message = new Message(new string[] { "islam.rakibul@bjitgroup.com" }, "Test", "<h1>Subscribe to my channel!</h1>");
          
            _emailService.SendMail(message);
            return StatusCode(StatusCodes.Status200OK,new Response { Status ="Success",Message="Email sent successfully."});
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user,token);
                if(result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Email verified SuccessFully" });
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "This user Doesnot exist!" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            // checking the user
            var user = await _userManager.FindByNameAsync(loginModel.Username);

            //checking the password
            if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                //claimlist creation
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                //we add roles to the user
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach(var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role,role));
                }
                //generates the token with the claims
                 var jwtToken = GetToken(authClaims);
                //returning the token

                return Ok(new { 
                 token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                 expiration= jwtToken.ValidTo
                });
            }

            return Unauthorized();
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims) 
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));
            var token = new JwtSecurityToken(
                                issuer: _configuration["JwtSettings:ValidIssuer"],
                                audience: _configuration["JwtSettings:ValidAudience"],
                                notBefore:DateTime.UtcNow, //Valid from this moment
                                expires: DateTime.UtcNow.AddHours(3),
                                claims: authClaims,
                                signingCredentials: new SigningCredentials(authSigningKey,SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

    }
}
