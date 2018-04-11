using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dashboards
{
  public class Zone
  {
    [Key]
    public int ZoneId { get; set; }
    public String Naam { get; set; }
    public int Locatie { get; set; }
    public Dashboard Dashboard { get; set; }
  }
}
