using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolitiekeBarometer_MVC.Models
{
    public class ZoneViewModel
    {
        public string naam { get; set; }

        public List<ItemViewModel> items { get; set; }

        public int zoneId { get; set; }

        public int plaatsId { get; set; }
    }
}