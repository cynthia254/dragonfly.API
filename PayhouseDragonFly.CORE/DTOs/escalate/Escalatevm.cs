﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.CORE.DTOs.escalate
{
    public class Escalatevm
    {
        public int ticketid { get; set; }
        public string status { get; set; }

        public string Escalation { get; set; }
    }
}
