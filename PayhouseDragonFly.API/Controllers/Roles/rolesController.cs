using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.roleresponse;
using PayhouseDragonFly.CORE.DTOs.Ticketsvms;
using PayhouseDragonFly.INFRASTRUCTURE.Services.RoleServices;

namespace PayhouseDragonFly.API.Controllers.Roles
{

    [Route("api/[controller]", Name = "Role")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleServices _roleServices;
        public RolesController(IRoleServices _roleService)
        {
            _roleServices = _roleService;


        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("CreateRole")]
        public async Task<RolesResponse> CreateRole(string Rolename)
        {
           return await  _roleServices.CreateRole(Rolename);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<RolesResponse> GetAllRoles()
        {
            return await _roleServices.GetAllRoles();
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("AssignUserToRole")]
        public async Task<RolesResponse> AssignUserToRole(string useremail, int roleid)
        {
            return await _roleServices.AssignUserToRole(useremail,roleid);

        }
        

      }

}

