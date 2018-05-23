using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dashboards
{
    public class Dashboard
    {
        [Key]
        public int DashboardId { get; set; }

        [Required]
        public Gebruiker Gebruiker { get; set; }

        public List<Zone> Zones { get; set; }

        public Deelplatform Deelplatform { get; set; }
    }
}
