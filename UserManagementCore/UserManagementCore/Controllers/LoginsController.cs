using Microsoft.AspNetCore.Mvc;
using UserManagementCore.Interfaces;
using UserManagementCore.Models;

namespace UserManagementCore.Controllers
{
    public class LoginsController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly ILogins _loginService;

        public LoginsController(ILogins loginService, IHttpContextAccessor httpContextAccessor)
        {
            _loginService = loginService;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index(string returnUrl = null)
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View("Login", model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            /* [ [ TEST CASE METHOD ]-> Login_ModelStateIsValidTest_RetrurnLoginViewModel ] */         
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid login detils");
                return View("Login", model);
            }
           

            var user = await _loginService.GetUser(model.Email);
            if (user != null)
            {                
                if (user.PasswordHash == model.Password)
                {
                    /* [ [ TEST CASE METHOD ]-> Login_GivenCorrectPassword_RedirectToLoginAction ] */
                    _httpContextAccessor.HttpContext.Session.SetString("User", model.Email);
                }
                else   /* [ [ TEST CASE METHOD ]-> Login_GivenInvalidCredential_RedirectToLoginAction ] */
                {
                    ModelState.AddModelError("", "Invalid password");
                    return View("Login", model);
                }
            }

            /* [ [ TEST CASE METHOD ]-> Login_GivenInvalidCredential_RedirectToLoginAction ] */
            else
            {
                ModelState.AddModelError("", "User was not found");
                return View("Login", model);
            }


            /* [ [ TEST CASE METHOD ]-> Login_GivenCorrectPassword_RedirectUrl ] */
            if (!string.IsNullOrEmpty(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

            /* [ [ TEST CASE METHOD ]-> Login_GivenCorrectPassword_RedirectToLoginAction ] */
            return RedirectToAction("Display", "Photos"); 
        }

        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid user details");
                return View(model);
            }

            var existingUser = await _loginService.GetUser(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "This email adress is already registered");
                return View(model);
            }

            await _loginService.CreateUser(model.Email, model.Password);

            return RedirectToAction("Index", "Logins");
        }
    }
}
