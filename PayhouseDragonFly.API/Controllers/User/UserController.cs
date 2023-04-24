using Microsoft.AspNetCore.Mvc;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.authresponse;
using PayhouseDragonFly.CORE.ConnectorClasses.Response;
using PayhouseDragonFly.CORE.DTOs.loginvms;
using PayhouseDragonFly.CORE.DTOs.RegisterVms;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IUserServices;

using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using Microsoft.AspNetCore.Authorization;

namespace PayhouseDragonFly.API.Controllers.User
{

    [Route("api/[controller]", Name = "Users")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;   
        public UserController(IUserServices userServices   )
        {
            _userServices= userServices;
        }


        
        [HttpPost]
        [Route("Authenticate")]
        public async Task<authenticationResponses> Authenticate(loginvm loggedinuser)
        {

            return await _userServices.Authenticate(loggedinuser);

        }
  
        [HttpPost]
        [Route("Register_User")]
        public async    Task<RegisterResponse> RegisterUser(RegisterVms rv)
        {
            return await  _userServices.RegisterUser(rv);

        }
        [Authorize(AuthenticationSchemes ="Bearer")]
        [HttpGet]
        [Route("GetAllUsers")]
      
        public async Task<BaseResponse> GetAllUsers()
        {

            return await _userServices.GetAllUsers();   
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("DeleteUsers")]
        public async Task<BaseResponse> DeleteUser(string usermail)
        {
            return await _userServices.DeleteUser(usermail);
        }



        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("ConfirmUserMail")]
      
        public async Task<BaseResponse> ConfirmUserAccount(string useremail)
        {

            return await _userServices.ConfirmUserAccount(useremail);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetuserByEmail")]
        public async Task<BaseResponse> GetUserByEmail(string useremail)
        {

            return await _userServices.GetUserByEmail(useremail);   
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("Edituseremail")]
        public async Task<BaseResponse> EditUserEmail(string newemail)
        {

            return await _userServices.EditUserEmail(newemail);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("getuserbyid")]
        public async Task<BaseResponse> GetUserById(string userId)
        {
            return await _userServices.GetUserById(userId);

        }


    }
}
