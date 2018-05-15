using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Platformen
{
    public class Deelplatform
    {
        [Key]
        public int Id { get; set; }

        public string Naam { get; set; }

        public TimeSpan Historiek  { get; set; }

        public List<Gebruiker> Gebruikers { get; set; }

        public List<Gebruiker> Admins { get; set; }
    }
}
