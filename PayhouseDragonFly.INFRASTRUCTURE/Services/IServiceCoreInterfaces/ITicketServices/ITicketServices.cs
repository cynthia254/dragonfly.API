using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.DTOs.Ticketsvms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.ITicketServices
{
    public  interface ITicketServices
    {
        Task<BaseResponse> AddTicket(Ticketsvms vm);
        Task<BaseResponse> GetAllTickets();

    }
}
