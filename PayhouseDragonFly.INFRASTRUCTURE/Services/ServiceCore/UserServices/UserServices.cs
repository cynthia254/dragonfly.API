using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PayhouseDragonFly.CORE.ConnectorClasses.Response;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.authresponse;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.DTOs.loginvms;
using PayhouseDragonFly.CORE.DTOs.RegisterVms;
using PayhouseDragonFly.CORE.Models.UserRegistration;
using PayhouseDragonFly.INFRASTRUCTURE.DataContext;
using PayhouseDragonFly.INFRASTRUCTURE.Services.ExtraServices;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IUserServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.INFRASTRUCTURE.Services.ServiceCore.UserServices
{
    public class UserServices : IUserServices
    {
        //private readonly JwtBearerTokenSettings _jwtBearerTokenSettings;
        private readonly UserManager<PayhouseDragonFlyUsers> _userManager;
        private readonly SignInManager<PayhouseDragonFlyUsers> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserServices> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DragonFlyContext _authDbContext;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IEExtraServices _extraservices;
        public UserServices(

         UserManager<PayhouseDragonFlyUsers> userManager,
         SignInManager<PayhouseDragonFlyUsers> signInManager,
         RoleManager<IdentityRole> roleManager,
         ILogger<UserServices> logger,
         IHttpContextAccessor httpContextAccessor,
         DragonFlyContext authDbContext,
         IEExtraServices extraservices,
         IServiceScopeFactory scopeFactory
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _authDbContext = authDbContext;
            _scopeFactory = scopeFactory;
            _extraservices = extraservices;

        }


        public async Task<RegisterResponse> RegisterUser(RegisterVms rv)
        {
            try
            {
                if (rv.FirstName == "")
                {
                    return new RegisterResponse("150", "First Name cannot be empty",null);
                }
                if (rv.LastName == "")
                {
                    return new RegisterResponse("150", "Last Name cannot be empty", null);
                }
                if (rv.DepartmentName == "")
                {
                    return new RegisterResponse("150", "Department Name cannot be empty", null);

                }
                if (rv.Position == "")
                {
                    return new RegisterResponse("150", "Position cannot be empty", null);

                }


                if (rv.BusinessUnit == "")
                {
                    return new RegisterResponse("150", "Business Unit cannot be empty", null);
                }

                if (rv.Address == "")
                {

                    return new RegisterResponse("150", "Location cannot be empty", null);
                }
                if (rv.AdditionalInformation == "")
                {
                    return new RegisterResponse("150", "User Type cannot be empty", null);
                }
                if (rv.PhoneNumber == "")
                {
                    return new RegisterResponse("150", "Phone Number cannot be empty", null);
                }
                if (rv.Email == "")
                {
                    return new RegisterResponse("150", "Email cannot be empty", null);
                }
                if (rv.Password == "")
                {
                    return new RegisterResponse("150", "Password cannot be empty", null);
                }

                using (var scope = _scopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var newuser = new PayhouseDragonFlyUsers
                    {
                        FirstName = rv.FirstName,
                        LastName = rv.LastName,
                        ClientName=rv.DepartmentName,
                        UserName = rv.Email,
                        DepartmentName = rv.DepartmentName,
                        PasswordHash = rv.Password,
                        Email = rv.Email,
                        PhoneNumber = rv.PhoneNumber,
                        Address = rv.Address,
                        Site = rv.Site,
                        VerificationToken = "fbndvdhhhdhd",
                        NormalizedEmail = rv.Email,
                        AnyMessage="SUCCESSFUL",
                        Position = rv.Position,
                        BusinessUnit=rv.BusinessUnit,
                        AdditionalInformation=rv.AdditionalInformation,
                        Salutation= rv.Salutation,
                        County="Any"
                        

                    };

                    var response = await _userManager.CreateAsync(newuser, rv.Password);

                    return new RegisterResponse("200", "Registered user successfully ", newuser);


                }
            }
            catch (Exception ex)
            {

                return new RegisterResponse("150", ex.Message, null);

            }

        }


        public async Task<authenticationResponses> Authenticate(loginvm loggedinuser)
        {
            try
            {


                using (var scope = _scopeFactory.CreateScope())
                {

                    var scopedContext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    //general auths
                    if (loggedinuser.UserName == "")
                    {
                        return
                        new authenticationResponses("140", "Email cant be empty", "", "", "", "");


                    }
                    if (loggedinuser.Password == "")
                    {
                        return
                        new authenticationResponses("150", "Password cant be empty", "", "", "", "");

                    }
                    var identityUser = await _userManager.FindByEmailAsync(loggedinuser.UserName);
                    if (identityUser == null)
                    {
                        return
                         new authenticationResponses("120", "user not found", "", "", "", "");
                    }

                    if (!identityUser.EmailConfirmed)
                    {
                        return new authenticationResponses("110", "Kindly authenticate account first", "", "", "", "");
                    }
                    if (identityUser != null)
                    {
                        var result =

                            _userManager.PasswordHasher
                                .VerifyHashedPassword(identityUser,
                                identityUser.PasswordHash,
                                loggedinuser.Password);

                        if (result == PasswordVerificationResult.Failed)
                        {
                            return
                         new authenticationResponses("114", "Please use the correct password", "", "", "", "");
                        }



                        var tokenexpirytimestamp =
                            DateTime
                                .Now
                                .AddMinutes(CORE.ConnectorClasses.TokenConstants.Constants.JWT_TOKEN_VALIDITY);
                        var jwtsecuritytokenhandler = new JwtSecurityTokenHandler();
                        var tokenkey =
                            Encoding
                                .ASCII
                                .GetBytes(CORE.ConnectorClasses.TokenConstants.Constants.JWT_SECURITY_KEY);
                        var securitytokendescripor =
                            new SecurityTokenDescriptor
                            {
                                Subject =
                                    new ClaimsIdentity(new List<Claim> {
                                    new Claim("UserName",
                                        loggedinuser.UserName),
                                    new Claim("PasswordHash",
                                        identityUser.PasswordHash),
                                    new Claim("LastName",
                                        identityUser.LastName),
                                    new Claim("Id", identityUser.Id)
                                        }),
                                Expires = tokenexpirytimestamp,
                                SigningCredentials =
                                    new SigningCredentials(new SymmetricSecurityKey(tokenkey),
                                        SecurityAlgorithms.HmacSha256Signature)
                            };

                        var securitytoken =
                            jwtsecuritytokenhandler.CreateToken(securitytokendescripor);
                        var token = jwtsecuritytokenhandler.WriteToken(securitytoken);
                        return new authenticationResponses("200", "Successfully logged in",
                            token, loggedinuser.UserName,identityUser.FirstName, identityUser.LastName);
                    }
                    return new authenticationResponses("", "", "", "","","");

                }
            }

            catch (Exception e)
            {
                _logger.LogInformation("Error message on login : ", e.Message);
                return new authenticationResponses("190", e.Message, "", "", "","");
            }
        }

        public async Task<BaseResponse> GetAllUsers()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontent = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var allusers = await scopedcontent.PayhouseDragonFlyUsers.OrderByDescending(x=>x.DateCreated).ToListAsync();

                    if (allusers == null)
                    {
                        return new BaseResponse("120", "No users found", null);
                    }

                    return new BaseResponse("200", "Queried successfully", allusers);
                }
            }
            catch (Exception ex)
            {

                return new BaseResponse("190", ex.Message, ex.StackTrace);
            }
        }
        public async Task<BaseResponse> DeleteUser(string usermail)
        {
            try
            {


                var userexists = await _userManager.FindByEmailAsync(usermail);

                if (userexists == null)
                {

                    return new BaseResponse("190", " User not found", null);
                }


                var response = await _userManager.DeleteAsync(userexists);

                if (!response.Succeeded)
                {
                    return new BaseResponse("140", "User not deleted", null);
                }

                return new BaseResponse("200", "User deleted successfully", null);
            }
            catch (Exception ex)
            {

                return new BaseResponse("190", ex.Message, null);
            }

        }
        public async Task<BaseResponse> ConfirmUserAccount(string useremail)
        {

            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {

                    var scopedcontent = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();


                    var userexists = await _userManager.FindByEmailAsync(useremail);

                    if (userexists == null)
                    {
                        return new BaseResponse("009", "User does not exist", null);
                    }
                    userexists.EmailConfirmed = true;

                    var response = await _userManager.UpdateAsync(userexists);
                    if (response.Succeeded)
                    {
                        return new BaseResponse("200", "User account confirmed successfully", null);
                    }
                    return new BaseResponse("", "", null);
                }

            }
            catch (Exception ex)
            {

                return new BaseResponse("105", ex.Message, null);
            }
        }
        public async Task<BaseResponse> GetUserByEmail(string useremail)
        {
            var userexists = await _userManager.FindByEmailAsync(useremail);

            if (userexists == null)
            {
                return new BaseResponse("1800", "User not found", null);
            }

            return new BaseResponse("200", "Queried successfully", userexists);
        }



        public async Task<BaseResponse> EditUserEmail(string newemail)
        {

            var loggedinuser = await _extraservices.LoggedInUser();


            if (loggedinuser == null)
            {

                return new BaseResponse("92", "user not logged in", null);
            }

            loggedinuser.Email = newemail;
            loggedinuser.UserName= newemail;
            loggedinuser.NormalizedEmail= newemail;

            await _userManager.UpdateAsync(loggedinuser);
            return new BaseResponse("200", "Email  changed successfully ", loggedinuser);


        }
        public async Task<BaseResponse> GetUserById(string userId)
        {
           // var user = await _authDbContext.Tickets.Where(x =>x.Id==userId).FirstOrDefaultAsync();
           var userexists= _userManager.FindByIdAsync(userId);

            if (userexists == null)
            {

                return new BaseResponse("120", "user not available", null);
            }

            return new BaseResponse("200", "Queried succesfully", userexists);

        }


        public async Task<BaseResponse> getAllUsers()
        {
            var allusers = await _authDbContext.PayhouseDragonFlyUsers.ToListAsync();

            return new BaseResponse("200", "queried successfully", allusers);
        }
    

}
}



