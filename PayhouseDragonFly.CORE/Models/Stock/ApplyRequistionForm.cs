using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.CORE.Models.Stock
{
    public class ApplyRequistionForm
    {
        [Key]
        public int ID { get; set; }
        public string itemName { get; set; }
        public string BrandName { get; set; }
        public string Description { get; set; }
        public string stockNeed { get; set; }
        public int Quantity { get; set; }
        public string DeviceBeingRepaired { get; set; }
        public string clientName { get; set; }
        public string Department { get; set; }
        public string Purpose { get; set; }
        public string Requisitioner { get; set; }
        public string IssuedBy { get; set; } = "Unknown";
        public DateTime IssuedByDate { get; set; } = DateTime.Now;
        public string ApprovedBy { get; set; } = "Unknown";
        public string ApprovedStatus { get; set; }
        public DateTime ApprovedDate { get; set; } = DateTime.Now;
        public string RejectReason { get; set; } = "Unknown";
        public string selectedOption { get; set; } = "Unknown";
        public string useremail { get; set; }

    }
}
