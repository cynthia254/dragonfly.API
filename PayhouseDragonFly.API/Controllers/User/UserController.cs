using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayhouseDragonFly.CORE.ConnectorClasses.Response;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.authresponse;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.DTOs.Designation;
using PayhouseDragonFly.CORE.DTOs.loginvms;
using PayhouseDragonFly.CORE.DTOs.RegisterVms;
using PayhouseDragonFly.CORE.DTOs.userStatusvm;
using PayhouseDragonFly.INFRASTRUCTURE.Services.ExtraServices.RoleChecker;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IUserServices;

namespace PayhouseDragonFly.API.Controllers.User
{

    [Route("api/[controller]", Name = "Users")]
    [ApiController]

    public class UserController : ControllerBase
    {
        public readonly IUserServices _userServices;
        private readonly IRoleChecker _rolechecker;
        public UserController(IUserServices userServices, IRoleChecker rolechecker)
        {
            _userServices = userServices;
            _rolechecker = rolechecker;
        }



        [HttpPost]
        [Route("Authenticate")]
        public async Task<authenticationResponses> Authenticate(loginvm loggedinuser)
        {

            
            return await _userServices.Authenticate(loggedinuser);

        }

        [HttpPost]
        [Route("Register_User")]
        public async Task<RegisterResponse> RegisterUser(RegisterVms rv)
        {
            return await _userServices.RegisterUser(rv);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllUsers")]

        public async Task<BaseResponse> GetAllUsers()
        {
            var roleidretured = _rolechecker.Returnedrole().Result;
            if (roleidretured == 1)
            {
                return await _userServices.GetAllUsers();
            }
            else
            {

                return new BaseResponse("120", "You have no permission access this", null); 
            }
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("DeleteUsers")]
        public async Task<BaseResponse> DeleteUser(string usermail)
        {
            var roleidretured = _rolechecker.Returnedrole().Result;
            if (roleidretured == 1)
            {
                return await _userServices.DeleteUser(usermail);
            }
            else
            {

                return new BaseResponse("120", "You have no permission access this", null);
            }
        }



        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("ConfirmUserMail")]

        public async Task<BaseResponse> ConfirmUserAccount(string useremail)
        {

            var roleidretured = _rolechecker.Returnedrole().Result;
            if (roleidretured == 1)
            {
                return await _userServices.ConfirmUserAccount(useremail);
            }
            else
            {

                return new BaseResponse("120", "You have no permission access this", null);
            }
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

        [HttpPost]
        [Route("Add_Department")]
        public async Task<BaseResponse> AddDepartment(AddDepartmentvms addDepartmentvms)
        {

            return await _userServices.AddDepartment(addDepartmentvms);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllDepartment")]

        public async Task<BaseResponse> GetAllDepartment()
        {
            var roleidretured = _rolechecker.Returnedrole().Result;
            if (roleidretured == 1)
            {

                return await _userServices.GetAllDepartment();
        }
            return new BaseResponse("120", "You have no permission access this", null);
    }


        [HttpGet]
        [Route("SendTestMail")]
        public async Task<mailresponse> TestMail(string testmail)
        {

            return await _userServices.TestMail(testmail);
        }

        [HttpPost]
        [Route("Getdepartmentbyid")]
        public async Task<BaseResponse> GetDepartmentByID(int departmentid)
        {
            return await _userServices.GetDepartmentByID(departmentid);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("Suspend_user")]
        public async Task<BaseResponse> SuspendUser(suspendUservm vm)

        {
            return await _userServices.SuspendUser(vm);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("Change_UserStatus")]
       public async Task<BaseResponse> ChangeUserStatus(userStatusvm vm)
        {
            var roleidretured = _rolechecker.Returnedrole().Result;
            if (roleidretured == 1)
            {
                return await _userServices.ChangeUserStatus(vm);
            }
            return new BaseResponse("120", "You have no permission access this", null);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetUserActiveStatusByid")]
        public async Task<BaseResponse> GetUserActiveStatusByid(string userid)
        {
            return await _userServices.GetUserActiveStatusByid(userid);
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("LoggedInUser")]
        public async Task<BaseResponse> GetLoggedInUser()
        {
            return await _userServices.GetLoggedInUser();
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("EditUser")]
        public async Task<BaseResponse> EditUserDetails(RegisterVms edituservm)
        {

            return await _userServices.EditUserDetails(edituservm);
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("ActivateUserAccount")]
        public async Task<BaseResponse> ActivateUser(string useremail)
        {
            return await _userServices.ActivateUser(useremail);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("CreateDesignation")]
        public async Task <BaseResponse>AddDesignation(AddDesignationvms addDesignationvms)
        {
            return await _userServices.AddDesignation(addDesignationvms);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("Getalldesignation")]
        public async Task<BaseResponse> GetAllDesignation()
        {
            return await _userServices.GetAllDesignation();
        }
        [HttpPost]
        [Route("Getdesignationbyid")]
        public async Task<BaseResponse> GetDesignationByID(int PositionId)
        {
            return await _userServices.GetDesignationByID(PositionId);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("EdutDesignation")]
        public async Task<BaseResponse> EditDesignation(EditDesignationvm editDesignationvm)
        {

            return await _userServices.EditDesignation(editDesignationvm);
        }

   


    }



}

