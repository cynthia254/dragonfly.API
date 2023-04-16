using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.DTOs.Ticketsvms;
using PayhouseDragonFly.CORE.Models.NewFolder.TicketRegistration;
using PayhouseDragonFly.CORE.Models.UserRegistration;
using PayhouseDragonFly.INFRASTRUCTURE.DataContext;
using PayhouseDragonFly.INFRASTRUCTURE.Services.ExtraServices;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.ITicketServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.INFRASTRUCTURE.Services.ServiceCore.TicketService
{
    public class TicketService: ITicketServices
    {

        //private readonly JwtBearerTokenSettings _jwtBearerTokenSettings;
        private readonly UserManager<PayhouseDragonFlyUsers> _userManager;
        private readonly SignInManager<PayhouseDragonFlyUsers> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<TicketService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DragonFlyContext _authDbContext;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IEExtraServices _extraServices;
       
        public TicketService
            (
                 UserManager<PayhouseDragonFlyUsers> userManager,
                 SignInManager<PayhouseDragonFlyUsers> signInManager,
                 RoleManager<IdentityRole> roleManager,
                 ILogger<TicketService> logger,
                 IHttpContextAccessor httpContextAccessor,
                 DragonFlyContext authDbContext,
                 IServiceScopeFactory scopeFactory,
                 IEExtraServices      extraServices
               )
                {
                    _userManager = userManager;
                    _signInManager= signInManager;
                    _roleManager = roleManager;         
                    _logger= logger;
                    _signInManager = signInManager;
                    _authDbContext= authDbContext;
                    _scopeFactory= scopeFactory;
                    _extraServices= extraServices;
                    
        }

        public async Task<BaseResponse> AddTicket(Ticketsvms  vm)
        {

            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext= scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var loggedinuserobject = await _extraServices.LoggedInUser();

                    var userEmail =  loggedinuserobject.Email;

                    if(loggedinuserobject == null)
                    {

                        return new BaseResponse("190", "user not logged in. login again", null);    

                    }

                    var newticket = new Tickets
                    {
                        TicketTitle = vm.TicketTitle,
                        DateUpdated = vm.DateUpdated,
                        DateCreated = vm.DateCreated,
                        TicketType = vm.TicketType,
                        TicketDescriptiom = vm.TicketDescriptiom,
                        AssignedFrom = vm.AssignedFrom,
                        AssignedTo = vm.AssignedTo,
                        DueDate = vm.DueDate,
                        Priority = vm.Priority,
                        Status = vm.Status,
                        CreatedBy= userEmail
                    };

                    await scopedcontext.Tickets.AddAsync( newticket );
                    await scopedcontext.SaveChangesAsync();

                    return new BaseResponse("200", "Ticket created successfully , the next approver will be informed", null);
                }
                
                

            }
            catch ( Exception ex )
            {

                return new BaseResponse("120", ex.Message, null);
            }

        }

        public async Task<BaseResponse> GetAllTickets()
        {

            var alltickets = await _authDbContext.Tickets.ToListAsync();
            if (alltickets == null)
            {
                return new BaseResponse("130", "there are no tickets available", null);
            }

            return new BaseResponse("200", "Queried successfully", alltickets);
        }
    }
}
