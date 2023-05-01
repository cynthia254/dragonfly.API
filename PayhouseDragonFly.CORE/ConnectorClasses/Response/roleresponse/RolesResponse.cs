using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.CORE.ConnectorClasses.Response.roleresponse
{
    public class RolesResponse
    {
        public bool IsTrue { get; set; }
        public string Message { get; set; }
        public object Body { get; set; }

        public RolesResponse(bool istrue, string message, object body)
        {
            IsTrue = istrue;
            Message = message;

            Body = body;   

        }
    }
}
