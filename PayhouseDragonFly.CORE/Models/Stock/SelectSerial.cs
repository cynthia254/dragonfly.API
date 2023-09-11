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
        public int IssuedIID { get; set; }
        public string SerialStatus { get; set; } = "Not Issued";
        public string ItemName { get; set; }
        public string BrandName { get; set; }
        public string IMEII1 { get; set; }
        public string IMEI2 { get; set; }
        public string clientName { get; set; }
        public string StockNeed { get; set; }
        public string Requisitioner { get; set; }
        public string CategoryName { get; set; }
       public string IssuedBy { get; set; }
        public string IssueStatus { get; set; } = "Issued";
        public DateTime DateIssued { get; set; } = DateTime.Now;
        public int Quantity { get; set; }
        public string Comments { get; set; } = "Working OK";
    }
}
