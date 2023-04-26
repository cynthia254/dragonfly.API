using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.DTOs.Roles;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IRoleServices;

namespace PayhouseDragonFly.API.Controllers.Roles
{
    [Route("api/[controller]",Name ="Roles")]
    [ApiController]
    public class rolesController : ControllerBase
    {
        private readonly IRolesServices _rolesServices;
        public rolesController(IRolesServices rolesServices)
        {
            _rolesServices = rolesServices;
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes ="Bearer")]
        [Route("AddRole")]

        public async Task<BaseResponse> AddRoles(rolesvms vm)
        {
            return await _rolesServices.AddRoles(vm);
        }

    }
}
