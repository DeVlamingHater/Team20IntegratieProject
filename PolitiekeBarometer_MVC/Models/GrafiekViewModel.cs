using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolitiekeBarometer_MVC.Models
{
    public class GrafiekViewModel : ItemViewModel
    {
        public string[] labels { get; set; }

        public List<DatasetViewmodel> datasets { get; set; }

        public int GrafiekType { get; set; }

        public int DataType { get; set; }
    }
}