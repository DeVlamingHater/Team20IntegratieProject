using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Parameter
    {
        [Key]
        public int ParameterId { get; set; }

        public String Naam { get; set; }

        public ParameterType ParameterType { get; set; }

        public List<Waarde> Waarden { get; set; }

        public Post Post { get; set; }
    }
}
