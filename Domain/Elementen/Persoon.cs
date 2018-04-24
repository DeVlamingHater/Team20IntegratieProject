using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Persoon : Element
    {
        public Organisatie Organisatie { get; set; }
       
        public string District { get; set; }

        public string Level { get; set; }

        public string Gender { get; set; }

        public string Twitter { get; set; }

        public string Site { get; set; }

        public string DateOfBirth { get; set; }

        public string Facebook { get; set; }

        public string Postal_code { get; set; }

        public string Position { get; set; }

        public string Organisation { get; set; }

        public string Town { get; set; }
    }
}
