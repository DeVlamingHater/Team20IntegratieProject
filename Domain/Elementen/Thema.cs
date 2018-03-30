using Domain.Elementen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Thema : Element
    {
        public List<Keyword> Keywords { get; set; }
    }
}
