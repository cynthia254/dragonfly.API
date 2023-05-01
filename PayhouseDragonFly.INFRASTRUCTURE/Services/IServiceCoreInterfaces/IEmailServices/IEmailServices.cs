using PayhouseDragonFly.CORE.ConnectorClasses.Response;
using PayhouseDragonFly.CORE.DTOs.EmaillDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IEmailServices
{
    public interface IEmailServices
    {
        Task<mailresponse> SenTestMail(emailbody emailvm);
        Task<mailresponse> SendEmailOnRegistration(emailbody emailvm);
    }
}
