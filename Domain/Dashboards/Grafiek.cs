using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dashboards
{
    public class Grafiek : Item,IValidatableObject
    {
        public List<DataConfig> Dataconfigs { get; set; }

        [Required]
        public GrafiekType GrafiekType{ get; set; }

        public TimeSpan Tijdschaal { get; set; }

        public int AantalDataPoints { get; set; }

        public DataType DataType { get; set; }
                
        public List<Filter> Filters { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (Dataconfigs.Count == 0)
            {
                string errorMessage = "Er moet minstens één dataconfig zijn";
                errors.Add(new ValidationResult(errorMessage,
                    new string[]{"Dataconfigs"}));
            }
            if (AantalDataPoints <=0)
            {
                string errorMessage = "DataPoints moeten positief zijn";
                errors.Add(new ValidationResult(errorMessage,
                    new string[] { "AantalDataPoints" }));
            }
            return errors;
        }
    }
}
