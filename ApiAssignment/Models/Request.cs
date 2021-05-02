using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiAssignment.Models
{
    public class Request : IValidatableObject
    {
        public string MovieTitle { get; set; }
        public int? MovieYear { get; set; }
        public PlotEnum? MoviePlot { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(MovieTitle) && !MovieYear.HasValue && !MoviePlot.HasValue)
            {
                return new List<ValidationResult>() { new ValidationResult("Some prop was invalid") };
            }

            return new List<ValidationResult>();
        }
    }
}