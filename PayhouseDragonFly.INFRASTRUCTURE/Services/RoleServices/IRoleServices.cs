using PayhouseDragonFly.CORE.ConnectorClasses.Response.roleresponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.INFRASTRUCTURE.Services.RoleServices
{
    public interface IRoleServices
    {
       Task<RolesResponse> CreateRole(string Rolename);
        Task<RolesResponse> AssignUserToRole(string useremail, int roleid);
        Task<RolesResponse> GetAllRoles();
    }
}
