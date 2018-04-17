﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dashboards
{
    public class Zone
    {
        public int Id { get; set; }

        public Dashboard Dashboard { get; set; }

        public string Naam { get; set; }

        public List<Item> Items { get; set; }

    }
}
