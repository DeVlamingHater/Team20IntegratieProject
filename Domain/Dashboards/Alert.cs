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

        public DataType DataType { get; set; }

        public AlertStatus Status { get; set; }

        public DataConfig DataConfig { get; set; } 

        public TimeSpan Interval { get; set; }

        //Het aantal posts dat moet overschreden worden
        public double Waarde { get; set; }

        //Bepaal welke operator moet gebruikt worden bij het bepalen van een overschrijding
        [RegularExpression("[<,>][=]?")]
        public String Operator { get; set; }

        public Boolean ApplicatieMelding { get; set; }

        public Boolean BrowserMelding { get; set; }

        public Boolean EmailMelding { get; set; }

        public Dashboard Dashboard { get; set; }

        public List<Melding> Meldingen { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            //Er moet minstens één soort melding ingesteld zijn
            //if (!ApplicatieMelding && !EmailMelding && !BrowserMelding)
            //{
            //    string errormessage = "Er moet minstens één soort melding ingesteld zijn";
            //    errors.Add(new ValidationResult(errormessage,
            //        new string[] {"ApplicatieMelding","EmailMelding","BrowserMelding" }));
            //}

            return errors;
        }
    }
}
