using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.CORE.Models.Stock
{
    public class UploadPOFile
    {
        [Key]
        public int ID { get; set; }
        public string PONumber { get; set; }
        public int Warranty { get; set; }
        public DateTime WarrantyStartDate { get; set; }
        public string Amount { get; set; }
        public string Rate { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public DateTime WarrantyEndDate { get; set; }
        public int AjustedQuantity { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public int OpeningStock { get; set; }
        public int AvailableStock { get; set; }
        public int StockOut { get; set; }
        public string FileName { get; set; } = "unknown";
        public int TotalStockIn { get; set; }
        //public string Status { get; set; } = "Incomplete";
        //public string Reference_Number { get; set; }
        
    }
}
