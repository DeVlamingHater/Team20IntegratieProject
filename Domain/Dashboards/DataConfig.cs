using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class DataConfig
    {
        [Key]
        public int DataConfiguratieId { get; set; }

        public DataType DataType { get; set; }

        [Required]
        public List<Element> Elementen { get; set; }

    }
}
