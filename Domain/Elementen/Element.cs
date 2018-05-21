using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public abstract class Element
    {
        public static Comparison<Element> compareByNaam = delegate (Element element1, Element element2)
        {
            return element1.Naam.CompareTo(element2.Naam);
        };
        [Key]
        public int Id { get; set; }
        [Required]
        public string Naam { get; set; }
        public double Trend { get; set; }
        public int TrendingPlaats { get; set; }
        public Deelplatform Deelplatform{ get; set; }

        public override bool Equals(object obj)
        {
            var element = obj as Element;
            return element != null &&
                   Naam == element.Naam;
        }

        public override int GetHashCode()
        {
            return -1386946022 + EqualityComparer<string>.Default.GetHashCode(Naam);
        }
    }
}

