using Domain.Dashboards;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Platformen
{
    public class DeelplatformDashboard
    {
        [Key]
        public int Id { get; set; }

        public Deelplatform Deelplatform { get; set; }

        public List<Item> Items { get; set; }
    }
}