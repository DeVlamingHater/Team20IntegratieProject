using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dashboards
{
    public abstract class Item
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Size { get; set; }

    }
}
