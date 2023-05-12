using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.roleresponse;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.RolesResponse;

namespace PayhouseDragonFly.INFRASTRUCTURE.Services.RoleServices
{
    public interface IRoleServices
    {
       Task<RolesResponse> CreateRole(string Rolename);
        Task<RolesResponse> AssignUserToRole(string useremail, int roleid);
        Task<RolesResponse> GetAllRoles();
        Task<string> Roleschecker();
        Task<Rolesresponse> AddRoleClaim(string roleclaimname);
        Task<Rolesresponse> GetAllRolecLaims();
        Task<Rolesresponse> AddClaimsToRole(int roleid, int claimid);
        Task<RoleClaimsResponse> GetRoleClaims(int roleid);
        Task<BaseResponse> GetRoleByID(int Roleid);
        Task<BaseResponse> GetRoleClaimByname(string claimname);
        Task<bool> CheckClaimInRole(String claimname, int roleid);
    }
}
