using Domain.Dashboards;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class DataConfig
    {
        [Key]
        public int DataConfiguratieId { get; set; }

        [Required]
        public Element Element { get; set; }

        public Element Vergelijking { get; set; }

        public List<Filter> Filters { get; set; }

        public string Label { get; set; }
    }
}
