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
using PayhouseDragonFly.CORE.ConnectorClasses.Response.roleresponse;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.RolesResponse;
using PayhouseDragonFly.CORE.DTOs.Designation;
using PayhouseDragonFly.CORE.DTOs.EmaillDtos;
using PayhouseDragonFly.CORE.DTOs.loginvms;
using PayhouseDragonFly.CORE.DTOs.RegisterVms;
using PayhouseDragonFly.CORE.DTOs.userStatusvm;
using PayhouseDragonFly.CORE.Models.departments;
using PayhouseDragonFly.CORE.Models.Designation;
using PayhouseDragonFly.CORE.Models.Roles;
using PayhouseDragonFly.CORE.Models.statusTable;
using PayhouseDragonFly.CORE.Models.UserRegistration;
using PayhouseDragonFly.INFRASTRUCTURE.DataContext;
using PayhouseDragonFly.INFRASTRUCTURE.Services.ExtraServices;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IEmailServices;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IExtraServices;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IUserServices;
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
        private readonly ILoggeinUserServices _loggeinUserServices;
       
        public UserServices(
          IEmailServices emailServices,
        UserManager<PayhouseDragonFlyUsers> userManager,
         SignInManager<PayhouseDragonFlyUsers> signInManager,
         RoleManager<IdentityRole> roleManager,
         ILogger<UserServices> logger,
         IHttpContextAccessor httpContextAccessor,
         DragonFlyContext authDbContext,
         IEExtraServices extraservices,
         IServiceScopeFactory scopeFactory,
         ILoggeinUserServices loggeinUserServices
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
            _emailServices = emailServices;
            _loggeinUserServices = loggeinUserServices;

        }


        public async Task<RegisterResponse> RegisterUser(RegisterVms rv)
        {
            try
            {
                if (rv.FirstName == "")
                {
                    return new RegisterResponse("150", "First Name cannot be empty", null);
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
                var userexists = await _userManager.FindByEmailAsync(rv.Email);
                if (userexists != null)
                {
                    return new RegisterResponse("180", "Email already exists", null);
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
                        UserStatus = "",
                        StatusReason = "New",
                        UserActive = false,
                        DepartmentDescription = "Any Description",
                        PostionDescription="Any Description",
                        PositionName="Any name",
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

                        var results = await _emailServices.SendEmailOnRegistration(emailbody);

                        if (!results.IsSent)
                        {
                            return new RegisterResponse("130", "User not registered successfully", null);
                           
                        }

                        return new RegisterResponse("200", "Registered user successfully. Kindly wait for admin approval ", newuser);


                    }
                    else
                    {
                        return new RegisterResponse("179", response.ToString(), newuser);
                    }

                 //   return new RegisterResponse("140", "something foregin happened", null);

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
                            token, loggedinuser.UserName, identityUser.FirstName, identityUser.LastName);
                    }
                    return new authenticationResponses("", "", "", "", "", "");

                }
            }

            catch (Exception e)
            {
                _logger.LogInformation("Error message on login : ", e.Message);
                return new authenticationResponses("190", e.Message, "", "", "", "");
            }
        }

        public async Task<BaseResponse> GetAllUsers()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontent = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var allusers = await scopedcontent.PayhouseDragonFlyUsers.OrderByDescending(x => x.DateCreated).ToListAsync();
                    if (allusers == null)
                    {
                        return new BaseResponse("120", "No users found", null);
                    }
                    List<RegistrationOutputVm> userslist = new List<RegistrationOutputVm>();


                    foreach (var user in allusers)
                    {
                        var userslisted = new RegistrationOutputVm()
                        {

                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Salutation = user.Salutation,
                            AdditionalInformation = user.AdditionalInformation,
                            BusinessUnit = user.BusinessUnit,
                            Site = user.Site,
                            County = user.County,
                            DepartmentName = user.DepartmentName,
                            Address = user.Address,
                            ClientName = user.ClientName,
                            EmailConfirmed = user.EmailConfirmed,
                            VerificationTime = user.VerificationTime,
                            VerificationToken = user.VerificationToken,
                            AnyMessage = user.AnyMessage,
                            DateCreated = user.DateCreated,
                            RoleId = user.RoleId,
                            Position = user.Position,
                            DepartmentDescription = user.DepartmentDescription,
                            Email = user.Email,
                            StatusReason = user.StatusReason,
                            UserId=user.Id,
                            StatusDescription=user.StatusDescription,
                            ReasonforStatus=user.ReasonforStatus,
                            PositionName=user.PositionName,
                            PositionDescription=user.PostionDescription





                        };
                        if (user.EmailConfirmed)
                        {
                            userslisted.UserStatus = "Active";
                        }
                        else if (!user.EmailConfirmed)
                        {
                            userslisted.UserStatus = "Inactive";

                        }
                        else
                        {
                            userslisted.UserStatus = "NOTHING";
                        }


                        if (user.UserActive)
                        {
                            userslisted.UserActiveMessage = "Active";
                        }
                        else
                        {
                            userslisted.UserActiveMessage = "InActive";
                        }
                        userslist.Add(userslisted);

                    }


                    return new BaseResponse("200", "Queried successfully", userslist);
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

                    if (userexists.EmailConfirmed)
                    {
                        return new BaseResponse("103", "User Account already confirmed", null);
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
            loggedinuser.UserName = newemail;
            loggedinuser.NormalizedEmail = newemail;

            await _userManager.UpdateAsync(loggedinuser);
            return new BaseResponse("200", "Email  changed successfully ", loggedinuser);


        }
        public async Task<BaseResponse> GetUserById(string userId)
        {
            // var user = await _authDbContext.Tickets.Where(x =>x.Id==userId).FirstOrDefaultAsync();
            var userexists = _userManager.FindByIdAsync(userId);

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
            catch (Exception ex)
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

            }
            catch (Exception ex)
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

            return new BaseResponse("200", "queried successfully", alldepartment);
        }

        public async Task<BaseResponse> GetDepartmentByID(int departmentid)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var departmentexists = await scopedcontext.Departments.Where(x => x.Departnmentid == departmentid).FirstOrDefaultAsync();
                    if (departmentexists == null)
                    {
                        return new BaseResponse("130", "Department does not exist", null);

                    }

                    return new BaseResponse("200", "Queried successfully", departmentexists);
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse("120", ex.Message, null);
            }
        }
        public async Task<BaseResponse> SuspendUser(suspendUservm vm)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var userexists = await scopedcontext.PayhouseDragonFlyUsers.Where(t => t.Email == vm.useremail).FirstOrDefaultAsync();

                    if (userexists == null)
                    {
                        return new BaseResponse("150", "user does not exist", null);
                    }
                    //login begins


                    userexists.UserStatus = "Inactive";
                    userexists.StatusReason = vm.StatusReason;




                    scopedcontext.Update(userexists);
                    await scopedcontext.SaveChangesAsync();
                    return new BaseResponse("200", $"User suspended successfully", null);
                }
            }
            catch (Exception ex)
            {

                return new BaseResponse("190", ex.Message, null);
            }
        }
        public async Task<BaseResponse> ChangeUserStatus(userStatusvm vm)
        {

            try
            {

                using (var scoper = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scoper.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    //logedin user check date

                    
                    if(vm.ReasonforStatus== "PERMANENTLY SUSPENDED")
                    {
                        var anydate = new DateTime();
                        var convertedanydate=anydate.ToString();
                        vm.StartDate = convertedanydate;
                        vm.EndDate = convertedanydate; 
                    }
                    //get loggin in user

                    var userexists = await scopedcontext.PayhouseDragonFlyUsers.Where(u => u.Id == vm.userId).FirstOrDefaultAsync();
                    if (userexists == null)
                    {

                        return new BaseResponse("140", "user does not exist", null);
                    }


                    //add new  user status table
                    var newuserstatus = new UserStatusTable
                    {
                        userId = userexists.Id,
                        StatusDescription = vm.StatusDescription,
                        UsertActive = false,
                        ReasonforStatus = vm.ReasonforStatus,
                        StartDate =Convert.ToDateTime(vm.StartDate),
                        EndDate= Convert.ToDateTime(vm.EndDate)
                    };


                   newuserstatus.Totaldays =(int) (Convert.ToDateTime(vm.EndDate)-Convert.ToDateTime( vm.StartDate)).TotalDays;

                    var useralreadyinactive = await scopedcontext.UserStatusTable.Where(r => r.userId == vm.userId).FirstOrDefaultAsync();

                    if (useralreadyinactive != null)
                    {

                        return new BaseResponse("189", $"user {userexists.FirstName} {userexists.LastName}  is already inactive", null);
                    }
                    await scopedcontext.AddAsync(newuserstatus);
                    await scopedcontext.SaveChangesAsync();

                    userexists.UserActive = false;
                    userexists.StatusReason= vm.ReasonforStatus;
                    userexists.StatusDescription = vm.StatusDescription;
                    userexists.ReasonforStatus = vm.ReasonforStatus;
                    scopedcontext.Update(userexists);
                    await scopedcontext.SaveChangesAsync();

                    return new BaseResponse("200", $"{userexists.FirstName} {userexists.LastName} status changged success fully to '{vm.ReasonforStatus}'", null);
                }

            }
            catch (Exception ex)

            {
                _logger.LogInformation($"__________error message ________________=========++++++{ex.Message}_____==========(0)");
                return new BaseResponse("150", ex.Message, null);
            }
        }
        public async Task<BaseResponse> GetUserActiveStatusByid(string userid)
        {
            try
            {

                using (var scoper = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scoper.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    //get user status

                    var userstatusexists = await scopedcontext.UserStatusTable.Where(u => u.userId == userid).FirstOrDefaultAsync();
                    if (userstatusexists == null)
                    {
                        return new BaseResponse("190", " User status does not exist", null);
                    }

                    //return user status 

                    return new BaseResponse("200", "sucessfully queried", userstatusexists);

                }

            }
            catch (Exception ex)
            {
                return new BaseResponse("160", ex.Message, null);


            }


        }

        //get logged in user
        public async Task<BaseResponse> GetLoggedInUser()
        {

            var loggeinuser =  _loggeinUserServices.LoggedInUser().Result;

            return new BaseResponse("200", " Successfully queried loggin user", loggeinuser);
        }


        //edit user details
        public async Task<BaseResponse> EditUserDetails(RegisterVms edituservm)
        {
            try
            {

                using(var scope= _scopeFactory.CreateScope())
                {

                    var scopedcontext=scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var userexists = await scopedcontext.PayhouseDragonFlyUsers.Where(u => u.Id == edituservm.EditorId).FirstOrDefaultAsync();

                    if (userexists == null)
                    {

                        return new BaseResponse("180", "User does not exist", null);
                    }

                    if (edituservm.FirstName == "string")
                    {
                        userexists.FirstName = userexists.FirstName;
                       
                    }
                    else
                    {
                        userexists.FirstName=edituservm.FirstName;
                    }


                    if (edituservm.LastName == "string")
                    {
                        userexists.LastName=userexists.LastName;
                        _logger.LogInformation("Nothing to show here");
                    }
                    else
                    {
                        userexists.LastName = edituservm.LastName;
                    }


                    if (edituservm.DepartmentName == "string")
                    {
                        userexists.LastName = userexists.LastName;
                        _logger.LogInformation("Nothing to show here");
                    }
                    else
                    {
                        userexists.DepartmentName = edituservm.DepartmentName;
                    } 


                    if (edituservm.Position == "string")
                    {
                        userexists.Position= userexists.Position;
                        _logger.LogInformation("Nothing to show here");

                    }
                    else
                    {
                        userexists.Position = edituservm.Position;
                    }


                    if (edituservm.BusinessUnit =="string")
                    { 
                        userexists.BusinessUnit = userexists.BusinessUnit;
                        _logger.LogInformation("Nothing to show here");
                    }
                    else
                    {
                        userexists.BusinessUnit = edituservm.BusinessUnit;
                    }


                    if (edituservm.Address == "string")
                    {
                        userexists.Address = userexists.Address;

                        _logger.LogInformation("Nothing to show here");
                    }
                    else
                    {

                        userexists.Address = edituservm.Address;
                    }


                    if (edituservm.PhoneNumber == "string")
                    {
                        userexists.PhoneNumber = userexists.PhoneNumber;
                        _logger.LogInformation("Nothing to show here");
                    }
                    else
                    {
                        userexists.PhoneNumber = edituservm.PhoneNumber;

                    }

                    if (edituservm.Email == "string")
                    {
                        userexists.Email = userexists.Email;
                        _logger.LogInformation("Nothing to show here");
                    }
                    else
                    {
                        userexists.Email = edituservm.Email;
                        userexists.NormalizedEmail = edituservm.Email;
                        userexists.UserName = edituservm.Email;

                    }


                    scopedcontext.Update(userexists);
                    await scopedcontext.SaveChangesAsync();

                    return new BaseResponse("200", "Sucessfully updated user details", userexists);


                }

            }
            catch(Exception ex)
            {

                return new BaseResponse("130", ex.Message, null);
            }
        }


        //email on leave completion

        public async Task EmailOnLeaveCompletion()
        {

            try
            {

                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    //get all UserStatus data

                    var allusersstatus = await scopedcontext.UserStatusTable.ToListAsync();

                    foreach (var singleuserstatusobject in allusersstatus)
                    {

                        var currentdate = DateTime.Now;
                        //get user data

                        var userexists = await scopedcontext.PayhouseDragonFlyUsers
                            .Where(u => u.Id == singleuserstatusobject.userId).FirstOrDefaultAsync();

                        if (userexists == null)
                        {
                            _logger.LogInformation("_________________________user does not exist__________");
                        }

                        //create email body

                        var emailbody = new EmailbodyOnLeaveEnd
                            {
                            ToEmail=userexists.Email,
                            Names = userexists.FirstName + " " + userexists.LastName,
                            UserName=userexists.Email,
                            LeaveEndDate=singleuserstatusobject.EndDate,
                            PayLoad="Reminder"

                        };

                        //get one day before end of leave

                        if (currentdate.AddDays(1) == singleuserstatusobject.EndDate)
                        {
                            //send mail if true


                           await  _emailServices.SendEmailOnLeaveCompletion(emailbody);
                        }
                        else
                        {
                            _logger.LogInformation("_______||___________________________Date not yet reached _______________________________|||________");
                        }
                    }


                }

            }
            catch (Exception ex)
            {

                _logger.LogInformation($"___________--------________|||||||{ex.Message}_________");
            }
        }
        public async Task<BaseResponse>  ActivateUser(string useremail)
        {

            // get user

            var userexsists = await _authDbContext.PayhouseDragonFlyUsers.Where(x=>x.UserName==useremail).FirstOrDefaultAsync();
            if (userexsists == null)
            {

                return new BaseResponse("130", "userid does not exist", null);
            }

            userexsists.UserActive = true;
            userexsists.StatusDescription = "";
            userexsists.ReasonforStatus = "";




             _authDbContext.Update(userexsists);
            await _authDbContext.SaveChangesAsync();



                var getuseronstatustable = await _authDbContext.UserStatusTable.Where(x => x.userId == userexsists.Id).FirstOrDefaultAsync();
          
            _authDbContext.Remove(getuseronstatustable);
            await _authDbContext.SaveChangesAsync();



            

            return new BaseResponse("200", $"Successfully activated user {userexsists.FirstName}  {userexsists.LastName}", null);


        }
        public async Task<BaseResponse> AddDesignation(AddDesignationvms addDesignationvms)
        {
            try
            {
                if (addDesignationvms.PositionName == "")
                {
                    return new BaseResponse("150", "Position Name cannot be empty", null);
                }
                if (addDesignationvms.PositionDescription == "")
                {
                    return new BaseResponse("150", "Position Description cannot be empty", null);
                }

                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var newDesignation = new Designation
                    {

                        PositionName = addDesignationvms.PositionName,
                        PositionDescription = addDesignationvms.PositionDescription,


                    };

                    var response = await scopedcontext.AddAsync(newDesignation);
                    await scopedcontext.SaveChangesAsync();
                    return new BaseResponse("200", "Designation created successfully ", newDesignation);



                }


            }
            catch (Exception ex)
            {
                return new BaseResponse("150", ex.Message, null);
            }




        }
        public async Task<BaseResponse> GetAllDesignation()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontent = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var alldesignation = await scopedcontent.Designation.ToListAsync();

                    if (alldesignation == null)
                    {
                        return new BaseResponse("120", "No designation found", null);
                    }

                    return new BaseResponse("200", "Queried successfully", alldesignation);
                }
            }
            catch (Exception ex)
            {

                return new BaseResponse("190", ex.Message, ex.StackTrace);
            }
        }
        public async Task<BaseResponse> getallDesignation()
        {
            var alldesignation = await _authDbContext.Designation.ToListAsync();

            return new BaseResponse("200", "queried successfully", alldesignation);
        }
        public async Task<BaseResponse> GetDesignationByID(int PositionId)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var designationexists = await scopedcontext.Designation.Where(x => x.PostionId == PositionId).FirstOrDefaultAsync();
                    if (designationexists == null)
                    {
                        return new BaseResponse("130", "Designation does not exist", null);

                    }

                    return new BaseResponse("200", "Queried successfully", designationexists);
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse("120", ex.Message, null);
            }
        }
        public async Task<BaseResponse> EditDesignation(EditDesignationvm editDesignationvm)
        {
            try
            {

                using (var scope = _scopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var designationexists = await scopedcontext.Designation.Where(u => u.PostionId == editDesignationvm.PositionId).FirstOrDefaultAsync();

                    if (designationexists == null)
                    {

                        return new BaseResponse("180", "Designation does not exist", null);
                    }

                    if (editDesignationvm.PositionName == "string")
                    {
                        designationexists.PositionName = designationexists.PositionName;

                    }
                    else
                    {
                        designationexists.PositionName = editDesignationvm.PositionName;
                    }


                    if (editDesignationvm.PositionDescription == "string")
                    {
                        designationexists.PositionDescription = designationexists.PositionDescription;
                        _logger.LogInformation("Nothing to show here");
                    }
                    else
                    {
                        designationexists.PositionDescription = editDesignationvm.PositionDescription;
                    }



                    scopedcontext.Update(designationexists);
                    await scopedcontext.SaveChangesAsync();

                    return new BaseResponse("200", "Sucessfully updated designation details", designationexists);


                }

            }
            catch (Exception ex)
            {

                return new BaseResponse("130", ex.Message, null);
            }
        }


        //role services start

     



    }
}










