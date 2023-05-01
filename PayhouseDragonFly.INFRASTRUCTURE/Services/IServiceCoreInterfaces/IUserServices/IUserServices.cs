using PayhouseDragonFly.API.Controllers.User;
using PayhouseDragonFly.CORE.ConnectorClasses.Response;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.authresponse;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.DTOs.loginvms;
using PayhouseDragonFly.CORE.DTOs.RegisterVms;
using PayhouseDragonFly.CORE.DTOs.Roles;

namespace PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IUserServices
{
    public interface IUserServices
    {
        Task<authenticationResponses> Authenticate(loginvm loggedinuser);
        Task<RegisterResponse> RegisterUser(RegisterVms rv);
        Task<BaseResponse> GetAllUsers();
        Task<BaseResponse> DeleteUser(string usermail);
        Task<BaseResponse> ConfirmUserAccount(string useremail);
        Task<BaseResponse> GetUserByEmail(string useremail);
        Task<BaseResponse> EditUserEmail(string newemail);
        Task<BaseResponse> GetUserById(string userId);
        Task<BaseResponse> AddDepartment(AddDepartmentvms addDepartmentvm);
        Task<BaseResponse> GetAllDepartment();
        Task<BaseResponse> getAllDepartment();
       
        Task<BaseResponse> getAllUsers();
        Task<mailresponse> TestMail(string testmail);
    }
}
