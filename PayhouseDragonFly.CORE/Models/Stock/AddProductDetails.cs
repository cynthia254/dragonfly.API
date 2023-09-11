﻿using System;
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
        public string IMEI1 { get; set; }
        public string IMEI2 { get; set; }
        public DateTime WarrantyStartDate { get; set; }
        public DateTime WarrantyEndDate { get; set;
        }
        public string ProductStatus { get; set; } = "INCOMPLETE";
        public string SerialStatus { get; set; } = "Not Issued";
        public string BatchNumber { get; set; }
        public string ItemStatus { get; set; } = "Okay";
        public int Quantity { get; set; } = 0;


    }
}
