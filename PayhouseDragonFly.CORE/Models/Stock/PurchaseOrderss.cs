﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.CORE.Models.Stock
{
    public class PurchaseOrderss
    {
        [Key]
        public int PurchaseOrderID { get; set; }
        public string PONumber { get; set; }
        public DateTime PODate { get; set; }
        public string Vendor { get; set; }
    }
}