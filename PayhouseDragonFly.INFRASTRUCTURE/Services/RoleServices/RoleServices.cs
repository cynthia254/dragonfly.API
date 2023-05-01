using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.roleresponse;
using PayhouseDragonFly.CORE.Models.Roles;
using PayhouseDragonFly.CORE.Models.UserRegistration;
using PayhouseDragonFly.INFRASTRUCTURE.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.INFRASTRUCTURE.Services.RoleServices
{
    public class RoleServices : IRoleServices
    {
        private UserManager<PayhouseDragonFlyUsers> _userManager;
        private readonly UserManager<PayhouseDragonFlyUsers> _signinmanager;


        private readonly DragonFlyContext _context;
        private readonly IServiceScopeFactory _scopeFactory;
        public RoleServices(DragonFlyContext context,
            IServiceScopeFactory scopeFactory,
            UserManager<PayhouseDragonFlyUsers> userManager,
            UserManager<PayhouseDragonFlyUsers> signinmanager

            )
        {
            _context = context;
            _scopeFactory = scopeFactory;
            _userManager = userManager;
            _signinmanager = signinmanager;



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

                    if(roleexists != null)
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

                    if(allroles == null)
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
    }
}
