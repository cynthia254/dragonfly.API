using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.CORE.Models.Roles
{
    public class Roles
    {
        [Key]
        public int Roleid { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
    }
}
