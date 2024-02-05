using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagementCore.Common;
using UserManagementCore.Models;

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

        public AuthenticationController(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody]RegisterUser registerUser, string role=null) 
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
                return StatusCode(StatusCodes.Status201Created, new Response { Status = "Success", Message = "User Created Successfully!" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User Failed to Create", Errors= result.Errors });
            }
          

        }
        
    }
}
