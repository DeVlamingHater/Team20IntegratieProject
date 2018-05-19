using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Domain.Platformen
{
    public class Platform
    {
        [Key]
        public int Id { get; set; }

        public TimeSpan Historiek { get; set; }

        public static Timer refreshTimer = new Timer();

        public static TimeSpan interval = new TimeSpan(0,1,0,0,0);

        public static DateTime lastUpdate = DateTime.Now.AddDays(-7);
    }
}
