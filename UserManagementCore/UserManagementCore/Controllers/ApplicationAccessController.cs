using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementEntityModel.ViewModel;

namespace UserManagementCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationAccessController : ControllerBase
    {

        public ApplicationAccessController() { }

        [HttpGet("signin")]
        public ActionResult SignIn(UserSignInModel userSignInModel)
        {
            return Ok(userSignInModel);
        }
    }
}
