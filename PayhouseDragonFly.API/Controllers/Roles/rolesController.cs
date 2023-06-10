﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.roleresponse;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.RolesResponse;
using PayhouseDragonFly.CORE.DTOs.loginvms;
using PayhouseDragonFly.CORE.DTOs.Roles;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IExtraServices;
using PayhouseDragonFly.INFRASTRUCTURE.Services.RoleServices;

namespace PayhouseDragonFly.API.Controllers.Roles
{

    [Route("api/[controller]", Name = "Role")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleServices _roleServices;
        //public readonly IticketsCoreServices _ticketServices;
        private readonly ILoggeinUserServices _loggeinuser;

        public RolesController(IRoleServices _roleService, ILoggeinUserServices loggeinuser)
        {
            _roleServices = _roleService;
            _loggeinuser = loggeinuser;


        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("CreateRole")]
        public async Task<RolesResponse> CreateRole(string Rolename)
        {
            var roleclaimname = "CanCreateRole";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
            var roleclaimtrue = await _roleServices
                .CheckClaimInRole(roleclaimname, loggedinuser.RoleId);

            if (loggedinuser.RoleId > 0)
            {

                if (roleclaimtrue)
                {
                    return await _roleServices.CreateRole(Rolename);
                }
            }
            else
            {
                return new RolesResponse(false, "You have no permission access this", null);
            }
            return new RolesResponse(false, "", null);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<RolesResponse> GetAllRoles()
        {
            var roleclaimname = "CanViewAllRoles";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
            var roleclaimtrue = await _roleServices
                .CheckClaimInRole(roleclaimname, loggedinuser.RoleId);

            if (    loggedinuser.RoleId==1 || loggedinuser.RoleId==2 )
            {

                if (roleclaimtrue)
                {
                    return await _roleServices.GetAllRoles();

                }
            }
            else
            {

                return new RolesResponse(false, "You have no permission access this", null);
            }
            return new RolesResponse(false, "", null);
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("AssignUserToRole")]
        public async Task<RolesResponse> AssignUserToRole(string useremail, int roleid)
        {
            return await _roleServices.AssignUserToRole(useremail, roleid);

        }



        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("AddRoleClaim")]
        public async Task<Rolesresponse> AddRoleClaim(string roleclaimname)
        {
            var roleclaimnamename = "CanAddRoleClaim";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
            var roleclaimtrue = await _roleServices
                .CheckClaimInRole(roleclaimnamename, loggedinuser.RoleId);

            if (loggedinuser.RoleId > 0)
            {

                if (roleclaimtrue)
                {
                    return await _roleServices.AddRoleClaim(roleclaimname);
                }
            }
            else
            {

                return new Rolesresponse(false, "You have no permission access this", null);
            }
            return new Rolesresponse(false, "", null);

        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("AllClaims")]

        public async Task<Rolesresponse> GetAllRolecLaims()
        {
            var roleclaimname = "CanViewRoleClaims";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
            var roleclaimtrue = await _roleServices
                .CheckClaimInRole(roleclaimname, loggedinuser.RoleId);

            if (loggedinuser.RoleId > 0)
            {

                if (roleclaimtrue)
                {
                    return await _roleServices.GetAllRolecLaims();
                }
            }
            else
            {

                return new Rolesresponse(false, "You have no permission access this", null);
            }
            return new Rolesresponse(false, "", null);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("AddClaimstoRoles")]
        public async Task<Rolesresponse> AddClaimsToRole(int roleid, int claimid)
        {
            var roleclaimname = "CanAddClaimsToRoles";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
            var roleclaimtrue = await _roleServices
                .CheckClaimInRole(roleclaimname, loggedinuser.RoleId);

            if (loggedinuser.RoleId > 0)
            {

                if (roleclaimtrue)
                {
                    return await _roleServices.AddClaimsToRole(roleid, claimid);
                }
            }
            else
            {

                return new Rolesresponse(false, "You have no permission access this", null);
            }
            return new Rolesresponse(false, "", null);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetAllroleClaims")]
        public async Task<RoleClaimsResponse> GetRoleClaims(int roleid)
        {
            var roleclaimname = "CanViewRoleClaims";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
            var roleclaimtrue = await _roleServices
                .CheckClaimInRole(roleclaimname, loggedinuser.RoleId);

            if (loggedinuser.RoleId > 0)
            {

                if (roleclaimtrue)
                {
                    return await _roleServices.GetRoleClaims(roleid);
                }
            }
            else
            {

                return new RoleClaimsResponse(false, "You have no permission access this", "", null);
            }
            return new RoleClaimsResponse(false, "", "", null);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("Getrolebyid")]
        public async Task<BaseResponse> GetRoleByID(int Roleid)
        {
            var roleclaimname = "CanViewRoleClaims";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
            var roleclaimtrue = await _roleServices
                .CheckClaimInRole(roleclaimname, loggedinuser.RoleId);

            if (loggedinuser.RoleId > 0)
            {

                if (roleclaimtrue)
                {
                    return await _roleServices.GetRoleByID(Roleid);
                }
            }
            else
            {

                return new BaseResponse("130", "You have no permission access this", null);
            }
            return new BaseResponse("140", "", null);


        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("Deleteroleclaim")]
        public async Task<BaseResponse> DeleteRoleClaim(int ClaimId, int roleid)
        {
          return await _roleServices.DeleteRoleClaim(ClaimId, roleid);     
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("AssignUserOtherRoles")]
        public async Task<BaseResponse> AssignUserOtherRoles(otherRolesvm vm)
        {
            return await _roleServices.AssignUserOtherRoles(vm);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]

        [HttpPost]
        [Route("GetUserOtherRoles")]
        public async Task<BaseResponse> GetUserOtherRoles(string userid)
        {
            return await _roleServices .GetUserOtherRoles(userid);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("DeleteRole")]
        public async Task<BaseResponse> DeleteRole(string RoleName)
        {
            return await _roleServices.DeleteRole(RoleName);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("DeleteResponsibility")]
        public async Task<BaseResponse> DeleteResponsibility(int ClaimId)
        {
            return await _roleServices.DeleteResponsibility(ClaimId);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetUserRoles")]
        public async Task<BaseResponse> GetUserRoles(string userid)
        {
            return await _roleServices.GetUserRoles(userid);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetRoleByUserId")]
        public async Task<Rolesresponse> GetRoleByUserId(string userid)
        {
            return await _roleServices.GetRoleByUserId(userid);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("Total_Users_With_Role")]
        public async Task<Roles_User_CounterResponse> UsersWithRole(int roleid)
        {
            return await _roleServices.UsersWithRole(roleid);
        }





    }
}

