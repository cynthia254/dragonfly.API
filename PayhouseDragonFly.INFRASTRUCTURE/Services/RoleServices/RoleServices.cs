using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.roleresponse;
using PayhouseDragonFly.CORE.Models.Roles;
using PayhouseDragonFly.CORE.Models.UserRegistration;
using PayhouseDragonFly.INFRASTRUCTURE.DataContext;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IExtraServices;

namespace PayhouseDragonFly.INFRASTRUCTURE.Services.RoleServices
{
    public class RoleServices : IRoleServices
    {
        private UserManager<PayhouseDragonFlyUsers> _userManager;
        private readonly UserManager<PayhouseDragonFlyUsers> _signinmanager;
        

        private readonly DragonFlyContext _context;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILoggeinUserServices _loggedinuser;
        public RoleServices(DragonFlyContext context,
            IServiceScopeFactory scopeFactory,
            UserManager<PayhouseDragonFlyUsers> userManager,
            UserManager<PayhouseDragonFlyUsers> signinmanager,
            ILoggeinUserServices loggedinuser

            )
        {
            _context = context;
            _scopeFactory = scopeFactory;
            _userManager = userManager;
            _signinmanager = signinmanager;
            _loggedinuser = loggedinuser;
        }
        public async Task<RolesResponse> CreateRole(string Rolename)
        {

            try
            {
                if (Rolename == "")
                {

                    return new RolesResponse(false, "Kindly provide a role name to add role", null);
                }
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    //check if role exists 

                    var roleexists = await scopedcontext.RolesTable.Where(x => x.RoleName == Rolename).FirstOrDefaultAsync();

                    if (roleexists != null)
                    {
                        return new RolesResponse(false, $" Role  '{Rolename}' already exist, if  must add a similar role kindly change the " +
                             $"latter cases from lower to upper and vice versa depending on the existing  role . The existsing role is '{roleexists}' with role id {roleexists.RolesID} ", null);
                    }
                    var rolesclass = new RolesTable
                    {
                        RoleName = Rolename
                    };
                    await scopedcontext.AddAsync(rolesclass);
                    await scopedcontext.SaveChangesAsync();
                    return new RolesResponse(true, $"Role '{Rolename}'  created successfully", null);

                }

            }
            catch (Exception ex)
            {
                return new RolesResponse(false, ex.Message, null);

            }

        }
        public async Task<RolesResponse> AssignUserToRole(string useremail, int roleid)
        {
            try
            {
                if (useremail == "")
                {
                    return new RolesResponse(false, "User email cannot be empty", null);
                }
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    //check user exists
                    var userexists = await _userManager.FindByEmailAsync(useremail);

                    if (userexists == null)
                    {
                        return new RolesResponse(false, "User does not exist", null);

                    }

                    //check roleexists


                    var roleexists = await scopedcontext.RolesTable.Where(x => x.RolesID == roleid).FirstOrDefaultAsync();

                    if (roleexists == null)
                    {

                        return new RolesResponse(false, "Role does not exist", null);
                    }


                    //update user side 

                    userexists.RoleId = roleid;

                    scopedcontext.Update(userexists);
                    await scopedcontext.SaveChangesAsync();

                    return new RolesResponse(true, $"Role '{roleexists.RoleName}'  assigned to {userexists.FirstName} successfully", null);

                }
            }
            catch (Exception ex)
            {
                return new RolesResponse(false, ex.Message, null);

            }
        }
        public async Task<RolesResponse> GetAllRoles()
        {

            try
            {

                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var allroles = await scopedcontext.RolesTable.ToListAsync();

                    if (allroles == null)
                    {
                        return new RolesResponse(false, "Roles don't exist", null);
                    }
                    return new RolesResponse(true, "Successfully queried", allroles);

                }
            }
            catch (Exception ex)
            {
                return new RolesResponse(false, ex.Message, null);
            }
        }

        //role checker based on logged in user

        public async Task<RolesResponse> RoleChecker()
        {
            try
            {

                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    //roles chacker

                    return new RolesResponse(false, "not yet implemented , to bve done later", null);
                }
            }
            catch (Exception ex)
            {
                return new RolesResponse(false, ex.Message, null);



            }
        }
        public async Task<string> Roleschecker()
        {

            var user = await _loggedinuser.LoggedInUser();
            var roleexists = await _context.RolesTable.Where(x => x.RolesID == user.RoleId).FirstOrDefaultAsync();
            if (roleexists.RoleName == "SuperAdmin")
            {
                return "SuperAdmin";
            }

            else if (roleexists.RoleName == "Admin")
            {
                return "Admin";
            }
            else if (roleexists.RoleName == "Partner")
            {
                return "Partner";
            }

            else if(roleexists.RoleName=="User")
            {
                return "User";
            }
            return "";
        }

      
        
    }
}
