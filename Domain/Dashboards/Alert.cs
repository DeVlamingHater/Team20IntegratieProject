using Domain.Dashboards;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Alert : IValidatableObject
    {
        [Key]
        public int AlertId { get; set; }

        [Required]
        public AlertStatus Status { get; set; }

        [Required]
        public DataConfig DataConfig { get; set; }

        [Required]
        public double Waarde { get; set; }

        [Required]
        [RegularExpression("[<,>][=]?")]
        public String Operator { get; set; }

        public Boolean ApplicatieMelding { get; set; }

        public Boolean BrowserMelding { get; set; }

        public Boolean EmailMelding { get; set; }

        public Dashboard Dashboard { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            //Er moet minstens één soort melding ingesteld zijn
            if (!ApplicatieMelding && !EmailMelding && !BrowserMelding)
            {
                string errormessage = "Er moet minstens één soort melding ingesteld zijn";
                errors.Add(new ValidationResult(errormessage,
                    new string[] {"ApplicatieMelding","EmailMelding","BrowserMelding" }));
            }

            return errors;
        }
    }
}
