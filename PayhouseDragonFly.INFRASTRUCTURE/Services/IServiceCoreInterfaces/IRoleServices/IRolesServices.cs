using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.DTOs.Roles;
using PayhouseDragonFly.CORE.DTOs.Ticketsvms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IRoleServices
{
    public interface IRolesServices
    {
        Task<BaseResponse> AddRoles(rolesvms vm);
    }
}
