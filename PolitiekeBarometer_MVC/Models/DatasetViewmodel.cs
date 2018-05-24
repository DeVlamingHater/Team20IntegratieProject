using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolitiekeBarometer_MVC.Models
{
    public class DatasetViewmodel
    {
        public List<string> labels { get; set; }
        public List<double> waarden { get; set; }
    }
}