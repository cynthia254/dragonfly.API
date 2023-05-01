using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PayhouseDragonFly.API.Controllers.User;
using PayhouseDragonFly.CORE.ConnectorClasses.Response;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.authresponse;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.DTOs.EmaillDtos;
using PayhouseDragonFly.CORE.DTOs.loginvms;
using PayhouseDragonFly.CORE.DTOs.RegisterVms;
using PayhouseDragonFly.CORE.Models.departments;
using PayhouseDragonFly.CORE.Models.UserRegistration;
using PayhouseDragonFly.INFRASTRUCTURE.DataContext;
using PayhouseDragonFly.INFRASTRUCTURE.Services.ExtraServices;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IEmailServices;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IUserServices;
using PayhouseDragonFly.INFRASTRUCTURE.Services.ServiceCore.EmailService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        private readonly IEmailServices _emailServices;
        public UserServices(
          IEmailServices emailServices,
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
            _emailServices= emailServices;

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
                        ClientName = rv.DepartmentName,
                        UserName = rv.Email,
                        DepartmentName = rv.DepartmentName,
                        PasswordHash = rv.Password,
                        Email = rv.Email,
                        PhoneNumber = rv.PhoneNumber,
                        Address = rv.Address,
                        Site = rv.Site,
                        VerificationToken = "fbndvdhhhdhd",
                        NormalizedEmail = rv.Email,
                        AnyMessage = "SUCCESSFUL",
                        Position = rv.Position,
                        BusinessUnit = rv.BusinessUnit,
                        AdditionalInformation = rv.AdditionalInformation,
                        Salutation = rv.Salutation,
                        County = "Any",
                        RoleId = 4,
                        
                        DepartmentDescription="Any Description"
                    };

                    var response = await _userManager.CreateAsync(newuser, rv.Password);
                    if (response.Succeeded)
                    {
                        var emailpayload = $"Hi there {rv.FirstName} {rv.LastName}, thank you for your" +
                              $" registration to Payhouse Limited. Kindly contact the administrator for your account to be activated ";

                        var emailbody = new emailbody
                             {
                                ToEmail = rv.Email,
                                UserName = rv.FirstName + " " + rv.LastName,
                            PayLoad = emailpayload
                              };

                     var   results=  await _emailServices.SendEmailOnRegistration(emailbody);

                        if(results.IsSent)
                        {

                            return new RegisterResponse("200", "Registered user successfully ", newuser);
                        }

                        else
                        {
                            return new RegisterResponse("130", "User not registered successfully", null);
                        }

                       
                    }
                    return new RegisterResponse("120", "User not registered ", newuser);


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
        public async Task<BaseResponse> AddDepartment(AddDepartmentvms addDepartmentvms)
        {
            try
            {
                if (addDepartmentvms.DepartmentName == "")
                {
                    return new BaseResponse("150", "Department Name cannot be empty", null);
                }
                if (addDepartmentvms.DepartmentDescription == "")
                {
                    return new BaseResponse("150", "Department Description cannot be empty", null);
                }

                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var newDepartment = new Departments
                    {
                        
                        DepartmentDescription = addDepartmentvms.DepartmentDescription,
                        DepartmentName = addDepartmentvms.DepartmentName,


                    };
                  
                    var response = await scopedcontext.AddAsync(newDepartment);
                    await scopedcontext.SaveChangesAsync();
                    return new BaseResponse("200", "Department created successfully ", newDepartment);



                }


            }
            catch(Exception ex)
            {
                return new BaseResponse("150", ex.Message, null);
            }
           



      }

        public async Task<mailresponse> TestMail(string testmail) 
        {
            //start


            try
            {
                var sendmailbody = new emailbody
                {
                    ToEmail = testmail,
                    UserName = "Tester",
                    PayLoad = "This is a test mail to confirm the email service is working " +
                    "well "
                };
                var responsevalue = await _emailServices.SenTestMail(sendmailbody);
                if (responsevalue.IsSent)
                {
                    return new mailresponse(true, "email sent successfully");
                }
                else
                {

                    return new mailresponse(false, "Email not sent");
                }

            }catch(Exception ex)
            {

                return new mailresponse(false, ex.Message);
            }

        }

        public async Task<BaseResponse> GetAllDepartment()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontent = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var alldepartment = await scopedcontent.Departments.ToListAsync();

                    if (alldepartment == null)
                    {
                        return new BaseResponse("120", "No department found", null);
                    }

                    return new BaseResponse("200", "Queried successfully", alldepartment);
                }
            }
            catch (Exception ex)
            {

                return new BaseResponse("190", ex.Message, ex.StackTrace);
            }
        }


        public async Task<BaseResponse> getAllUsers()
        {
            var allusers = await _authDbContext.PayhouseDragonFlyUsers.ToListAsync();

            return new BaseResponse("200", "queried successfully", allusers);
        }
        public async Task<BaseResponse> getAllDepartment()
        {
            var alldepartment = await _authDbContext.Departments.ToListAsync();

            return new BaseResponse("200", "queried successfully",alldepartment);
        }

      

    }
}






  



