using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dashboards
{
    class Grafiek : Item
    {
        [Key]
        public int Id { get; set; }

        public List<DataConfig> Dataconfig{ get; set; }
    }
}
