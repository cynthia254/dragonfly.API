using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.DTOs.Ticketsvms;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.ITicketServices;

namespace PayhouseDragonFly.API.Controllers.Tickets
{

    [Route("api/[controller]", Name = "Tickets")]
    [ApiController]
    public class TicktesController : ControllerBase
    {
        public readonly ITicketServices _ticketServices;
        public TicktesController(ITicketServices ticketServices)
        {
            _ticketServices= ticketServices;
        }

        [Authorize(AuthenticationSchemes ="Bearer")]
        [HttpPost]
        [Route("RegisterTicket")]
        public async Task<BaseResponse> AddTicket(Ticketsvms vm)
        {
            return await _ticketServices.AddTicket(vm);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllTickets")]
        public async Task<BaseResponse> GetAllTickets()
        {

            return await _ticketServices.GetAllTickets();
        }
    }
}
