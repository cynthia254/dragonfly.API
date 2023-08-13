using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.CORE.Models.Stock
{
    public class SelectSerial
    {
        [Key]
        public int Id { get; set; }
        public int IssueID { get; set; }
        public string SerialNumber { get; set; }
        public string SerialStatus { get; set; } = "Not Issued";
       
    }
}
