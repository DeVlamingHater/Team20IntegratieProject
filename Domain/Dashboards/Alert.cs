using Domain.Dashboards;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Alert
    {
        [Key]
        public int AlertId { get; set; }

        [Required]
        public AlertStatus Status { get; set; }

        [Required]
        public DataConfig DataConfig { get; set; }

        [Required]
        public double Waarde { get; set; }

        [Required]
        public String Operator { get; set; }

        public Boolean ApplicatieMelding { get; set; }

        public Boolean BrowserMelding { get; set; }

        public Boolean EmailMelding { get; set; }

        public Dashboard Dashboard { get; set; }
    }
}
