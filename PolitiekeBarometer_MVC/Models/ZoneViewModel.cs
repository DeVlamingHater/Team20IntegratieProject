using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolitiekeBarometer_MVC.Models
{
    public class ZoneViewModel
    {
        public string Naam { get; set; }

        public List<ItemViewModel> Items { get; set; }

        public int Id{ get; set; }

        public int plaatsId { get; set; }
    }
}