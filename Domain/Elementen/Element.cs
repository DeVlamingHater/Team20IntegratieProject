using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public abstract class Element : IComparable<Element>
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Naam { get; set; }
        public double Trend { get; set; }

        public int CompareTo(Element other)
        {
            return this.Trend.CompareTo(other.Trend);
        }
    }
}

