using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dashboards
{
    public class Filter
    {
        [Key]
        public int FilterId { get; set; }

        public double Waarde { get; set; }

        public FilterType Type { get; set; }

        [RegularExpression("[<,>][=]?")]
        public string Operator { get; set; }

        public Grafiek Grafiek { get; set; }
    }

    public enum FilterType
    {
        SINCE, 
        UNTIL,
        SENTIMENT,
        AGE,
        RETWEET
    }
}
