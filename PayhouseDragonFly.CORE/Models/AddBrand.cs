﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.CORE.Models.Stock
{
    public class AddBrand
    {
        [Key]
        public int BrandId { get; set; }
        public string BrandName { get; set; }
    }
}