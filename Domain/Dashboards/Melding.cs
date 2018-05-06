using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dashboards
{
    public class Melding
    {
        [Key]
        public int MeldingId { get; set; }

        public Alert Alert { get; set; }

        public DateTime MeldingDateTime { get; set; }

        public bool IsActive { get; set; }

    }
}
