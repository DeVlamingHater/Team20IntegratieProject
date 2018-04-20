﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dashboards
{
    public class Grafiek : Item
    {
        [Key]
        public int Id { get; set; }

        public List<DataConfig> Dataconfigs{ get; set; }
    }
}