using Domain.Dashboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolitiekeBarometer_MVC.Models
{
    public class GrafiekViewModel : ItemViewModel
    {
        public Dictionary<string, Dictionary<string, double>> datasets { get; set; }

        public GrafiekType GrafiekType { get; set; }

        public int DataType { get; set; }
    }
}