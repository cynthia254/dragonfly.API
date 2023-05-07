
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.DTOs;
using PayhouseDragonFly.CORE.DTOs.escalate;
using PayhouseDragonFly.CORE.DTOs.resolve;
using PayhouseDragonFly.CORE.DTOs.Ticketsvms;
using PayhouseDragonFly.INFRASTRUCTURE.Services.ExtraServices.RoleChecker;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IticketsCoreServices;

namespace PayhouseDragonFly.API.Controllers.Tickets
{

    [Route("api/[controller]", Name = "Tickets")]
    [ApiController]
    public class TicktesController : ControllerBase
    {
        private readonly IRoleChecker _rolechecker;
        public readonly IticketsCoreServices _ticketServices;
        public TicktesController(IticketsCoreServices ticketServices, IRoleChecker rolechecker)
        {
            _ticketServices = ticketServices;
            _rolechecker= rolechecker;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("RegisterTicket")]
        public async Task<BaseResponse> AddTicket(Ticketsvms vm)
        {
            var roleidretured = _rolechecker.Returnedrole().Result;
            if (roleidretured == 2 || roleidretured==1 || roleidretured==3)
            {
                return await _ticketServices.AddTicket(vm);
            }
            else
            {

                return new BaseResponse("120", "You have no permission access this", null);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllTickets")]
        public async Task<BaseResponse> GetAllTickets()
        {
            var roleidretured = _rolechecker.Returnedrole().Result;
            if (roleidretured == 1)
            {
                return await _ticketServices.GetAllTickets();

            }
            else
            {

                return new BaseResponse("120", "You have no permission access this", null);
            }

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("EditTicketsStatus")]
        public async Task<BaseResponse> EditTicketStatus(string status, int ticketid)
        {
            var roleidretured = _rolechecker.Returnedrole().Result;
            if (roleidretured == 2 || roleidretured == 1 || roleidretured == 3)
            {
                return await _ticketServices.EditTicketStatus(status, ticketid);
            }
            else
            {

                return new BaseResponse("120", "You have no permission access this", null);
            }
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("getticketbyid")]

        public async Task<BaseResponse> GetTicketById(int ticketid)
        {
            return await _ticketServices.GetTicketById(ticketid);

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("Assigntickettouser")]
        public async Task<BaseResponse> AsignedUserToTicket(asignuservm assignvm)
        {
            var roleidretured = _rolechecker.Returnedrole().Result;
            if (roleidretured == 2 || roleidretured == 1 || roleidretured == 3)
            {
                return await _ticketServices.AsignedUserToTicket(assignvm);
            }
            else
            {

                return new BaseResponse("120", "You have no permission access this", null);
            }
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("resolveticket")]
        public async Task<BaseResponse> ResolveTicket(ResolveVm resolvevm){
            return await _ticketServices.ResolveTicket(resolvevm);


        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("escalateticket")]
        public async Task<BaseResponse> EscalateTicket(Escalatevm escalatevm)
        {
            return await _ticketServices.EscalateTicket(escalatevm);


        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("closeticket")]
        public async Task<BaseResponse> CloseTicket(CloseTicketvm closeTicketvm)
        {
            return await _ticketServices.CloseTicket(closeTicketvm);


        }

    }
}
