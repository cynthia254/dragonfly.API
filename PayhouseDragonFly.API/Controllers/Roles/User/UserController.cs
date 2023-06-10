using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayhouseDragonFly.CORE.ConnectorClasses.Response;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.authresponse;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.resetpassword;
using PayhouseDragonFly.CORE.DTOs.Department;
using PayhouseDragonFly.CORE.DTOs.Designation;
using PayhouseDragonFly.CORE.DTOs.loginvms;
using PayhouseDragonFly.CORE.DTOs.RegisterVms;
using PayhouseDragonFly.CORE.DTOs.userStatusvm;
using PayhouseDragonFly.INFRASTRUCTURE.Services.ExtraServices.RoleChecker;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IExtraServices;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IUserServices;
using PayhouseDragonFly.INFRASTRUCTURE.Services.RoleServices;
using System.Runtime.InteropServices;

namespace PayhouseDragonFly.API.Controllers.User
{

    [Route("api/[controller]", Name = "Users")]
    [ApiController]

    public class UserController : ControllerBase
    {
        public readonly IUserServices _userServices;
        private readonly IRoleChecker _rolechecker;
        private readonly ILoggeinUserServices _loggeinuser;

        private readonly IRoleServices _roleservices;
        public UserController(
            IUserServices userServices,
            IRoleChecker rolechecker,
            IRoleServices roleservices,
            ILoggeinUserServices loggeinuser
            )
        {
            _userServices = userServices;
            _rolechecker = rolechecker;
            _roleservices = roleservices;
            _loggeinuser = loggeinuser;

        }



        [HttpPost]
        [Route("Authenticate")]
        public async Task<authenticationResponses> Authenticate(loginvm loggedinuser)
        {

            return await _userServices.Authenticate(loggedinuser);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("Register_User")]
        public async Task<RegisterResponse> RegisterUser(RegisterVms rv)
        {
            var roleclaimname = "CanCreateUsers";
            var loggedinuser = await _loggeinuser.LoggedInUser();
            var roleclaimtrue = await _roleservices
                .CheckClaimInRole(roleclaimname, loggedinuser.RoleId);

            if (loggedinuser.RoleId > 0)
            {
                if (roleclaimtrue)
                {
                    return await _userServices.RegisterUser(rv);
                }
                else if (!roleclaimtrue)
                {
                    return new RegisterResponse("110", "You have no permission access this", null);

                }
            }
            else
            {

                return new RegisterResponse("130", "You have no permission to access this", null);
            }

            return new RegisterResponse("", "", null);


        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllUsers")]

        public async Task<BaseResponse> GetAllUsers()
        {
            var roleclaimname = "CanViewAllUsers";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
            var roleclaimtrue = await _roleservices
                .CheckClaimInRole(roleclaimname, loggedinuser.RoleId);



            if (loggedinuser.RoleId > 0)
            {

                if (roleclaimtrue)
                {
                    return await _userServices.GetAllUsers();
                }
                else if (!roleclaimtrue)
                {
                    return new BaseResponse("110", "You have no permission access this", null);

                }
            }
            else
            {
                return new BaseResponse("120", "You have no permission access this", null);
            }
            return new BaseResponse("", "", null);

        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("DeleteUsers")]
        public async Task<BaseResponse> DeleteUser(string usermail)
        {
            var roleclaimname = "CanDeleteUser";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
            var roleclaimtrue = await _roleservices
                .CheckClaimInRole(roleclaimname, loggedinuser.RoleId);

            if (loggedinuser.RoleId > 0)
            {

                if (roleclaimtrue)
                {
                    return await _userServices.DeleteUser(usermail);
                }
                else if (!roleclaimtrue)
                {
                    return new BaseResponse("110", "You have no permission access this", null);

                }
            }
            else
            {

                return new BaseResponse("120", "You have no permission access this", null);
            }
            return new BaseResponse("", "", null);
        }



        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("ConfirmUserMail")]

        public async Task<BaseResponse> ConfirmUserAccount(string useremail)
        {
            var roleclaimname = "CanConfirmUser";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
            var roleclaimtrue = await _roleservices
                .CheckClaimInRole(roleclaimname, loggedinuser.RoleId);

            if (loggedinuser.RoleId > 0)
            {

                if (roleclaimtrue)
                {
                    return await _userServices.ConfirmUserAccount(useremail);
                }
                else if (!roleclaimtrue)
                {
                    return new BaseResponse("110", "You have no permission access this", null);

                }
            }
            else
            {

                return new BaseResponse("120", "You have no permission access this", null);
            }
            return new BaseResponse("", "", null);
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("Add_Department")]
        public async Task<BaseResponse> AddDepartment(AddDepartmentvms addDepartmentvms)
        {
            var roleclaimname = "CanAddDepartment";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
            var roleclaimtrue = await _roleservices
                .CheckClaimInRole(roleclaimname, loggedinuser.RoleId);

            if (loggedinuser.RoleId > 0)
            {

                if (roleclaimtrue)
                {
                    return await _userServices.AddDepartment(addDepartmentvms);
                }
                else if (!roleclaimtrue)
                {
                    return new BaseResponse("110", "You have no permission access this", null);

                }
            }
            else
            {

                return new BaseResponse("120", "You have no permission access this", null);
            }
            return new BaseResponse("", "", null);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllDepartment")]

        public async Task<BaseResponse> GetAllDepartment()
        {
            var roleclaimname = "CanViewDepartments";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
            var roleclaimtrue = await _roleservices
                .CheckClaimInRole(roleclaimname, loggedinuser.RoleId);

            if (loggedinuser.RoleId > 0)
            {

                if (roleclaimtrue)
                {
                    return await _userServices.GetAllDepartment();
                }
                else if (!roleclaimtrue)
                {
                    return new BaseResponse("110", "You have no permission access this", null);

                }
            }
            else
            {

                return new BaseResponse("120", "You have no permission access this", null);
            }
            return new BaseResponse("", "", null);
        }


        [HttpGet]
        [Route("SendTestMail")]
        public async Task<mailresponse> TestMail(string testmail)
        {

            return await _userServices.TestMail(testmail);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("Getdepartmentbyid")]
        public async Task<BaseResponse> GetDepartmentByID(int departmentid)
        {
            var roleclaimname = "CanSeeRelatedDepartment";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
            var roleclaimtrue = await _roleservices
               .CheckClaimInRole(roleclaimname, loggedinuser.RoleId);

            if (loggedinuser.RoleId > 0)
            {

                if (roleclaimtrue)
                {
                    return await _userServices.GetDepartmentByID(departmentid);

                }
                else if (!roleclaimtrue)
                {
                    return new BaseResponse("110", "You have no permission access this", null);

                }
            }
            else
            {

                return new BaseResponse("120", "You have no permission access this", null);
            }
            return new BaseResponse("", "", null);
        }
    

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("Suspend_user")]
        public async Task<BaseResponse> SuspendUser(suspendUservm vm)

        {
            var roleclaimname = "CanSuspendUser";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
            var roleclaimtrue = await _roleservices
                .CheckClaimInRole(roleclaimname, loggedinuser.RoleId);

            if (loggedinuser.RoleId >0)
            {

                if (roleclaimtrue)
                {
                    return await _userServices.SuspendUser(vm);
                }
                else if (!roleclaimtrue)
                {
                    return new BaseResponse("110", "You have no permission access this", null);

                }
            }
            else
            {

                return new BaseResponse("120", "You have no permission access this", null);
            }
            return new BaseResponse("", "", null);

        }   
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("Change_UserStatus")]
       public async Task<BaseResponse> ChangeUserStatus(userStatusvm vm)
        {
            var roleclaimname = "CanEditUserStatus";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
            var roleclaimtrue = await _roleservices
                .CheckClaimInRole(roleclaimname, loggedinuser.RoleId);

            if (loggedinuser.RoleId>0)
            {

                if (roleclaimtrue)
                {
                    return await _userServices.ChangeUserStatus(vm);
                }
                else if (!roleclaimtrue)
                {
                    return new BaseResponse("110", "You have no permission access this", null);

                }
            }
            else
            {

                return new BaseResponse("120", "You have no permission access this", null);
            }
            return new BaseResponse("", "", null);
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
            var roleclaimname = "CanActivateUser";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
            var roleclaimtrue = await _roleservices
                .CheckClaimInRole(roleclaimname, loggedinuser.RoleId);

            if (loggedinuser.RoleId >0)
            {

                if (roleclaimtrue)
                {
                    return await _userServices.ActivateUser(useremail);

                }
                else if (!roleclaimtrue)
                {
                    return new BaseResponse("110", "You have no permission access this", null);

                }
            }
            else
            {

                return new BaseResponse("120", "You have no permission access this", null);
            }
            return new BaseResponse("", "", null);

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("CreateDesignation")]
        public async Task <BaseResponse>AddDesignation(AddDesignationvms addDesignationvms)
        {
            var roleclaimname = "CanCreateDesignation";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
            var roleclaimtrue = await _roleservices
                .CheckClaimInRole(roleclaimname, loggedinuser.RoleId);

            if (loggedinuser.RoleId >0)
            {

                if (roleclaimtrue)
                {
                    return await _userServices.AddDesignation(addDesignationvms);
                }
                else if (!roleclaimtrue)
                {
                    return new BaseResponse("110", "You have no permission access this", null);

                }
            }
            else
            {

                return new BaseResponse("120", "You have no permission access this", null);
            }
            return new BaseResponse("", "", null);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("Getalldesignation")]
        public async Task<BaseResponse> GetAllDesignation()
        {
            var roleclaimname = "CanViewDesignations";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
            var roleclaimtrue = await _roleservices
                .CheckClaimInRole(roleclaimname, loggedinuser.RoleId);

            if (loggedinuser.RoleId >0)
            {

                if (roleclaimtrue)
                {
                    return await _userServices.GetAllDesignation();
                }
                else if (!roleclaimtrue)
                {
                    return new BaseResponse("110", "You have no permission access this", null);

                }
            }
            else
            {

                return new BaseResponse("120", "You have no permission access this", null);
            }
            return new BaseResponse("", "", null);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
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
            var roleclaimname = "CanEditDesignation";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
            var roleclaimtrue = await _roleservices
                .CheckClaimInRole(roleclaimname, loggedinuser.RoleId);

            if (loggedinuser.RoleId > 0)
            {

                if (roleclaimtrue)
                {
                    return await _userServices.EditDesignation(editDesignationvm);
                }
                else if (!roleclaimtrue)
                {
                    return new BaseResponse("110", "You have no permission access this", null);
                }
            }
            else
            {
                return new BaseResponse("120", "You have no permission access this", null);
            }
            return new BaseResponse("", "", null);

        }

        [HttpPost]
        [Route("Send_Forget_Password_Link")]
        public async   Task<BaseResponse> SendForgetPasswordLink(string useremail)
        {
            return await _userServices.SendForgetPasswordLink(useremail);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("Update_password")]
        public async Task<BaseResponse> Updatepassword(Changepasswordvm updatepasswordvm)
        {
            return await _userServices.Updatepassword(updatepasswordvm);
        }

        [HttpPost]
        [Route("Reset_user_Password")]
        public async Task<BaseResponse> Reset_Forget_User_Password(ResetPasswordvm vm)
        {
            return await _userServices.Reset_Forget_User_Password(vm);
        }
        
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("EditDepartment")]
        public async Task<BaseResponse> EditDepartment(EditDepartmentvms editDepartmentvms)
        {
            var roleclaimname = "CanEditDepartment";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
            var roleclaimtrue = await _roleservices
                .CheckClaimInRole(roleclaimname, loggedinuser.RoleId);

            if (loggedinuser.RoleId > 0)
            {

                if (roleclaimtrue)
                {
                    return await _userServices.EditDepartment(editDepartmentvms);
                }
                else if (!roleclaimtrue)
                {
                    return new BaseResponse("110", "You have no permission access this", null);
                }
            }
            else
            {
                return new BaseResponse("120", "You have no permission access this", null);
            }
            return new BaseResponse("", "", null);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("DeleteDepartment")]
        public async Task<BaseResponse> DeleteDepartment(string DepartmentName)
        {
            return await _userServices.DeleteDepartment(DepartmentName);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("DeleteDesignation")]
        public async Task<BaseResponse> DeleteDesignation(string PositionName)
        {
            return await _userServices.DeleteDesignation(PositionName);
        }


        [HttpPost]
        [Route("SearchUsers")]
        public async Task<BaseResponse> SearchForUsers(string search_query)
        {
             return await  _userServices.SearchForUsers(search_query);
        }


    }



}

