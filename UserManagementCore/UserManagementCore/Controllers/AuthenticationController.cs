using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagement.EmailService.Models;
using UserManagement.EmailService.Services;
using UserManagementCore.Common;
using UserManagementCore.Models;
using UserManagementEntityModel.Models.Authentication.Login;
using UserManagementEntityModel.Models.Authentication.Password;
using UserManagementEntityModel.Models.Authentication.SignUp;

namespace UserManagementCore.Controllers
{
    //https://www.youtube.com/watch?v=J8pxfxLF41g&list=PLX4n-znUpc2b19AoYa4BMuhGuRnZItJK_&index=2
    //https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Core/src/SignInManager.cs
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
         private readonly IConfiguration _configuration;

        private readonly IEmailService _emailService;
        public AuthenticationController(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, IEmailService emailService,
            IConfiguration configuration, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _configuration = configuration;
            _signInManager = signInManager;
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
                UserName = registerUser.UserName,
                TwoFactorEnabled = true
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerUser.Password);
            if(result.Succeeded)
            {
                // Your role creation logic (replace with actual role name)
              //  var roleName = "Visitor";
                var roleName = "Administrator";

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
                return StatusCode(StatusCodes.Status200OK, new Response { Status = AppStatus.SuccessStatus, Message = $"User Created & Email Sent to {user.Email} Successfully!" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = AppStatus.ErrorStatus, Message = "User Failed to Create", Errors= result.Errors });
            }
          

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
         
            if (user.TwoFactorEnabled)
            {
                await _signInManager.SignOutAsync();
               var result =  await _signInManager.PasswordSignInAsync(user,loginModel.Password,true,lockoutOnFailure:true);

                var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

                var message = new Message(new string[] { user.Email! }, "OTP Confirmation", token);
                _emailService.SendMail(message);
                return StatusCode(StatusCodes.Status200OK, new Response { Status = AppStatus.SuccessStatus, Message = $"We have sent an OTP to your Email {user.Email}" });
            }
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
                expiration = jwtToken.ValidTo
                });
                //expirationUtcFrom = jwtToken.ValidFrom, expirationUtcTo = jwtToken.ValidTo ,
                // expirationLocalTimeFrom = TimeZone.CurrentTimeZone.ToLocalTime(jwtToken.ValidFrom),
                // expirationLocalTimeTo = TimeZone.CurrentTimeZone.ToLocalTime(jwtToken.ValidTo)
            }

            return Unauthorized();
        }


        [HttpPost]
        [Route("login-2FA")]
        public async Task<IActionResult> LoginWithOTP(string code, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var result = await _signInManager.TwoFactorSignInAsync("Email", code, false,rememberClient:false);
            if (result.Succeeded)
            {
             
                if (user != null)
                {
                    //claimlist creation
                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                    //we add roles to the user
                    var userRoles = await _userManager.GetRolesAsync(user);
                    foreach (var role in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    //generates the token with the claims
                    var jwtToken = GetToken(authClaims);
                    //returning the token

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        expiration = jwtToken.ValidTo
                    });                
                }
            }
            return StatusCode(StatusCodes.Status404NotFound,
                new Response { Status = AppStatus.SuccessStatus, Message = $"Invalid code" });
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var forgotPasswordLink = Url.Action("ResetPassword", "Authentication", new { token, email = user.Email }, Request.Scheme);

                var message = new Message(new string[] { user.Email }, "Confirmation email link", forgotPasswordLink!);
                try
                {
                    _emailService.SendMail(message);
                    return StatusCode(StatusCodes.Status200OK,
                        new Response { Status = AppStatus.SuccessStatus, Message = $"Password Changed request is sent on Email {user.Email}. Please Open your email & click the link !" });
                }catch (Exception)
                {
                    //add log file
                }
            }

            return StatusCode(StatusCodes.Status400BadRequest,
                new Response { Status = AppStatus.ErrorStatus, Message = $"Couldnot sent link to email. Please try again." });
        }


        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var model = new ResetPassword { Token= token, Email = email };

            return Ok(new { model });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user != null)
            {
                var reserPassResult = await _userManager.ResetPasswordAsync(user,resetPassword.Token,resetPassword.Password);
                  
                if(!reserPassResult.Succeeded)
                {
                    foreach(var error  in reserPassResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }

                    return Ok(ModelState);
                }
                await _signInManager.SignOutAsync();
                return StatusCode(StatusCodes.Status200OK,
                        new Response { Status = AppStatus.SuccessStatus, Message = $"Password has been changed.Please sign in." });
          
            }

            return StatusCode(StatusCodes.Status400BadRequest,
                new Response { Status = AppStatus.ErrorStatus, Message = $"Couldnot sent link to email. Please try again." });
        }
        private JwtSecurityToken GetToken(List<Claim> authClaims) 
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                                issuer: _configuration["JWT:ValidIssuer"],
                                audience: _configuration["JWT:ValidAudience"],
                                notBefore:DateTime.Now, //Valid from this moment
                                expires: DateTime.Now.AddHours(1),
                                claims: authClaims,                                
                                signingCredentials: new SigningCredentials(authSigningKey,SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

    }
}
