﻿using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.DTOs;
using PayhouseDragonFly.CORE.DTOs.escalate;
using PayhouseDragonFly.CORE.DTOs.resolve;
using PayhouseDragonFly.CORE.DTOs.serviceissue;
using PayhouseDragonFly.CORE.DTOs.Ticketsvms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.INFRASTRUCTURE.Services.ITicketServices
{
    public interface ITicketServices
    {
        Task<BaseResponse> AddTicket(Ticketsvms vm);
        Task<BaseResponse> GetAllTickets();
        Task<BaseResponse> EditTicketStatus(string status, int ticketid);
        Task<BaseResponse> GetTicketById(int ticketid);
        Task<BaseResponse> AsignedUserToTicket(asignuservm assignvm);
        Task<BaseResponse> ResolveTicket(ResolveVm resolvevm);
        Task<BaseResponse> EscalateTicket(Escalatevm escalatevm);
        Task<BaseResponse> CloseTicket(CloseTicketvm closeTicketvm);

    }
}
