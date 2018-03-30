using Domain.Elementen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Post
    {
        public String Source { get; set; }
        [Key]
        public long PostId { get; set; }

        public Persoon Persoon { get; set; }

        public List<Keyword> Keywords { get; set; }

        public List<Parameter> Parameters { get; set; }

        public DateTime Date { get; set; }
    }
}
