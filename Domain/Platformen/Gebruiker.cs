using Domain.Dashboards;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Platformen
{
    public class Gebruiker
    {
        [Key]
        public int GebruikerId { get; set; }
        [Required]
        public string Naam { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        
        public string Wachtwoord { get; set; }
    }
}
