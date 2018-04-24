using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Platformen
{
    public class Platform
    {
        [Key]
        public int Id { get; set; }

        public TimeSpan Historiek { get; set; }
    }
}
