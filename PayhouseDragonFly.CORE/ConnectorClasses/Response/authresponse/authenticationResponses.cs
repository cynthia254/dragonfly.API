using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.CORE.ConnectorClasses.Response.authresponse
{
    public  class authenticationResponses
    {

        public string Code { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }

        public authenticationResponses(string code,string message, string token, string email)
        {
            Code=code;
            Message = message;
            Token =token;
            Email=email;
         
        }
    }
}
