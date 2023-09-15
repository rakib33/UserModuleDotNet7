using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;
using System.Security.Claims;
using UserManagementCore.Infrastructure.ErrorHandler;
using UserManagementCore.Interfaces;
using UserManagementCore.Models;
using UserManagementEntityModel.ViewModel;

namespace UserManagementCore.Controllers
{
    /// <summary>
    /// [UNIT TEST] ->  https://code-maze.com/aspnetcore-identity-testing-usermanager-rolemanager/
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _usersService;

        private readonly IErrorHandler _errorHandler;
        public ApplicationUserController(IMapper mapper, IUserService userService, IErrorHandler errorHandler) {
            _mapper = mapper;
            _usersService = userService;
            _errorHandler = errorHandler;
        }
        //public void MyMethod()
        //{
        //    var source = new SourceType();
        //    var destination = _mapper.Map<SourceType, DestinationType>(source);
        //}


        [HttpGet]
        public List<ApplicationUser> Get()
        {
           IQueryable<ApplicationUser> user = _usersService.Get();
            return user.ToList();

            //Microsoft.Data.SqlClient.SqlException: 'A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify 
        }

        //[HttpPost("/api/[controller]/login")]
        //public async Task<UserModel> Login([FromBody] LoginRequestModel loginModel)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        throw new HttpRequestException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.ModelValidation), "", ModelState.Values.First().Errors.First().ErrorMessage));
        //    }

        //    var user = _usersService.GetByEmail(loginModel.Email);

        //    if (user == null)
        //        throw new HttpRequestException(_errorHandler.GetMessage(ErrorMessagesEnum.AuthUserDoesNotExists));

        //    await _usersService.SignOutAsync();
        //    var result = await _usersService.PasswordSignInAsync(
        //        user, loginModel.Password, false, false);

        //    if (result.Succeeded)
        //    {
        //        return user;
        //    }

        //    throw new HttpRequestException(_errorHandler.GetMessage(ErrorMessagesEnum.AuthWrongCredentials));
        //}

        //[HttpGet("{email}")]
        //public UserModel Get(string email)
        //{
        //    return _usersService.GetByEmail(email);
        //}


        //[HttpPost("/api/[controller]/create")]
        //public async Task<UserModel> Create([FromBody] CreateRequestModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        throw new HttpRequestException(string.Format(
        //            _errorHandler.GetMessage(ErrorMessagesEnum.ModelValidation),
        //            ModelState.Values.First().Errors.First().ErrorMessage));
        //    }
        //    var user = new UserModel
        //    {
        //        UserName = model.Name,
        //        Email = model.Email
        //    };

        //    var result = await _usersService.Create(user, model.Password);

        //    if (result.Succeeded)
        //    {
        //        return _usersService.GetByEmail(model.Email);
        //    }
        //    throw new HttpRequestException(_errorHandler.GetMessage(ErrorMessagesEnum.AuthCannotCreate));
        //}


        //[HttpPost("/api/[controller]/delete")]
        //public async Task<UserModel> Delete([FromBody] DeleteRequestModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        throw new HttpRequestException(string.Format(
        //            _errorHandler.GetMessage(ErrorMessagesEnum.ModelValidation),
        //            ModelState.Values.First().Errors.First().ErrorMessage));
        //    }
        //    var user = _usersService.GetByEmail(model.Email);
        //    if (user == null)
        //        throw new HttpRequestException(_errorHandler.GetMessage(ErrorMessagesEnum.AuthUserDoesNotExists));


        //    var result = await _usersService.Delete(user);
        //    if (result.Succeeded)
        //    {
        //        return user;
        //    }

        //    throw new HttpRequestException(_errorHandler.GetMessage(ErrorMessagesEnum.AuthCannotDelete));
        //}

        //[HttpPost]
        //public async Task<UserModel> Edit([FromBody] UpdateRequestModel request)
        //{
        //    var user = _usersService.GetByEmail(request.Email);

        //    if (user == null) throw new HttpRequestException(_errorHandler.GetMessage(ErrorMessagesEnum.AuthUserDoesNotExists));

        //    var validEmail = await _usersService.ValidateUser(user);

        //    if (!validEmail.Succeeded)
        //    {
        //        _errorHandler.ErrorIdentityResult(validEmail);
        //    }

        //    IdentityResult validPass = null;

        //    if (!string.IsNullOrEmpty(request.Password))
        //    {
        //        validPass = await _usersService.ValidatePassword(user, request.Password);
        //        if (validPass.Succeeded)
        //        {
        //            user.PasswordHash = _usersService.HashPassword(user,
        //                request.Password);
        //        }
        //        else
        //        {
        //            _errorHandler.ErrorIdentityResult(validPass);
        //        }
        //    }
        //    if (validPass != null && ((!validEmail.Succeeded || request.Password == string.Empty || !validPass.Succeeded)))
        //        throw new HttpRequestException(_errorHandler.GetMessage(ErrorMessagesEnum.AuthNotValidInformations));

        //    var result = await _usersService.Update(user);

        //    if (result.Succeeded)
        //    {
        //        return user;
        //    }

        //    throw new HttpRequestException(_errorHandler.GetMessage(ErrorMessagesEnum.AuthCannotUpdate));

        //}

        //[HttpPost, Produces("application/json")]
        //public async Task<SignInResult> Token(TokenRequestModel request)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        throw new HttpRequestException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.ModelValidation), ModelState.Values.First().Errors.First().ErrorMessage));
        //    }

        //    var user = _usersService.GetByEmail(request.Username);
        //    if (user == null) throw new HttpRequestException(_errorHandler.GetMessage(ErrorMessagesEnum.AuthUserDoesNotExists));


        //    await _usersService.SignOutAsync();
        //    var result = await _usersService.PasswordSignInAsync(
        //        user, request.Password, false, false);

        //    if (!result.Succeeded) throw new HttpRequestException(_errorHandler.GetMessage(ErrorMessagesEnum.AuthCannotRetrieveToken));

        //    // Create a new ClaimsIdentity holding the user identity.
        //    var identity = new ClaimsIdentity(
        //        OpenIdConnectServerDefaults.AuthenticationScheme,
        //        OpenIdConnectConstants.Claims.Name, null);
        //    // Add a "sub" claim containing the user identifier, and attach
        //    // the "access_token" destination to allow OpenIddict to store it
        //    // in the access token, so it can be retrieved from your controllers.
        //    identity.AddClaim(OpenIdConnectConstants.Claims.Subject,
        //        "71346D62-9BA5-4B6D-9ECA-755574D628D8",
        //        OpenIdConnectConstants.Destinations.AccessToken);
        //    identity.AddClaim(OpenIdConnectConstants.Claims.Name, user.UserName,
        //        OpenIdConnectConstants.Destinations.AccessToken);


        //    var principal = new ClaimsPrincipal(identity);
        //    return SignIn(principal, OpenIdConnectServerDefaults.AuthenticationScheme);
        //}
    }
}
