using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.roleresponse;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.RolesResponse;
using PayhouseDragonFly.CORE.Models.Roles;
using PayhouseDragonFly.CORE.Models.UserRegistration;
using PayhouseDragonFly.INFRASTRUCTURE.DataContext;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IExtraServices;
using System.Linq.Expressions;

namespace PayhouseDragonFly.INFRASTRUCTURE.Services.RoleServices
{
    public class RoleServices : IRoleServices
    {
        private UserManager<PayhouseDragonFlyUsers> _userManager;
        private readonly UserManager<PayhouseDragonFlyUsers> _signinmanager;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly DragonFlyContext _dragonflyContext;
        private readonly DragonFlyContext _context;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILoggeinUserServices _loggedinuser;
        public RoleServices(DragonFlyContext context,
            IServiceScopeFactory scopeFactory,
            UserManager<PayhouseDragonFlyUsers> userManager,
            UserManager<PayhouseDragonFlyUsers> signinmanager,
            ILoggeinUserServices loggedinuser,
            DragonFlyContext dragonflyContext,
            IServiceScopeFactory serviceScopeFactory

            )
        {
            _context = context;
            _serviceScopeFactory= serviceScopeFactory;
            _scopeFactory = scopeFactory;
            _userManager = userManager;
            _signinmanager = signinmanager;
            _loggedinuser = loggedinuser;
            _dragonflyContext = dragonflyContext;
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

        public async Task<Rolesresponse> AddRoleClaim(string roleclaimname)
        {


        
            if (roleclaimname == "")
            {

                return new Rolesresponse(false, "Claim name cannot be null", null);
                //return new RolesResponse(false, "Claim name cannot be null", null)
            }


            var roleclainexists = await _dragonflyContext.RoleClaimsTable.Where(x => x.ClaimName == roleclaimname).FirstOrDefaultAsync();

            if (roleclainexists != null)
            {
                return new Rolesresponse(false, "Claim already exists", null);

            }
            var roleclaimadded = new RoleClaimsTable
            {

                ClaimName = roleclaimname
            };

            await _dragonflyContext.AddAsync(roleclaimadded);
            await _dragonflyContext.SaveChangesAsync();
            return new Rolesresponse(true, "Successfully added role Claim type", null);
            // return new RolesResponse(true, "Role claim addedd successfully", null);


        }

        public async Task<Rolesresponse> GetAllRolecLaims()
        {
            var allclaims = await _dragonflyContext.RoleClaimsTable.ToListAsync();

            return  new Rolesresponse(true, "role claims queried successfully", allclaims);
        }



        public async Task<Rolesresponse> AddClaimsToRole(int roleid, int claimid)
        {
            try
            {
                 using( var scope= _serviceScopeFactory.CreateScope())
                        {
                             var scopedcontext= scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var checkifclaimexists=  await scopedcontext.Claim_Role_Map.Where(x=>x.ClaimId == claimid  && x.RoleId==roleid).FirstOrDefaultAsync();


                    if (checkifclaimexists != null)
                    {
                        return new Rolesresponse(false, "Claim already exists", null);

                    }

                    var new_claim_role_map = new Claim_Role_Map
                    {
                        RoleId = roleid,
                        ClaimId = claimid
                    };

                    await scopedcontext.AddAsync(new_claim_role_map);
                    await scopedcontext.SaveChangesAsync();

                    return new Rolesresponse(true, "Successfully added claim to role", null);

                        }

            }
            catch(Exception ex){

                return new Rolesresponse(false, ex.Message, null);
            }

        }
        public async Task<RoleClaimsResponse> GetRoleClaims(int roleid)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();


                    //get associated role
                    var roleexists = await scopedcontext.RolesTable.Where(r => r.RolesID == roleid).FirstOrDefaultAsync();

                    if(roleexists == null)
                    {

                        return new RoleClaimsResponse(false, "Role does not exist", "No role", null);

                    }

                    //get all related  claims to athe role found role mapper(only shows ids)

                    var roleclaimsexists = await scopedcontext.Claim_Role_Map.Where(rc => rc.RoleId == roleid).ToListAsync();

                  
                    if (roleclaimsexists == null)
                    {
                        return new RoleClaimsResponse(false, "Role claims does not exist", "No role claims", null);
                    }
                    //all claims on claim table(specific name)


                    List<RoleClaimsTable> claimstable = new List<RoleClaimsTable>();

                    foreach (var role in roleclaimsexists)
                    {
                        var claimqueried = await scopedcontext.RoleClaimsTable.Where(x => x.RolesClaimsTableId == role.ClaimId).FirstOrDefaultAsync();
                        var newclaimfound = new RoleClaimsTable 
                            { 
                                RolesClaimsTableId=claimqueried.RolesClaimsTableId,                
                                ClaimName =claimqueried.ClaimName,
         
                            };

                        claimstable.Add(newclaimfound);


                    }



                    //return values found

                    return new RoleClaimsResponse(true, "Queries sueccessfully",roleexists.RoleName, claimstable);

                }


            }
            catch (Exception ex)
            {

                return new RoleClaimsResponse(false, ex.Message, "No role", null);
            }

        }
    }
}
