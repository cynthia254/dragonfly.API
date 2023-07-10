﻿

using System.ComponentModel.DataAnnotations;

namespace PayhouseDragonFly.CORE.Models.Stock
{
    public class InvoiceLines
    {
        [Key]
        public int BatchID { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public string Currency { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public int Warranty { get; set; }
        public DateTime WarrantyStartDate { get; set; }
        public DateTime WarrantyEndDate
        {
            get; set;
        }
    }
}
