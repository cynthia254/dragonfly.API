using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.CORE.Models.Stock
{
    public class AddProductDetails
    {
        [Key]
        public int BatchID { get; set; }
        public int ItemID { get; set; }
        public string SerialNumber { get; set; }
        public string BrandName { get; set; } = "nothing";
        public string ItemName { get; set; } = "nothing";
        public int IMEI1 { get; set; }
        public int IMEI2 { get; set; }
        public DateTime WarrantyStartDate { get; set; }
        public DateTime WarrantyEndDate { get; set;
        }
    }
}
