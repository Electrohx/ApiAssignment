using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiAssignment.Models
{
    public class MovieRequestDTO : IValidatableObject
    {
        [Required]
        public string Title { get; set; }
        public int? Year { get; set; }
        public PlotEnum? Plot { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return string.IsNullOrEmpty(Title) && !Year.HasValue && !Plot.HasValue ?
                new List<ValidationResult>() { new ValidationResult("While all properties are optional, one must always be added") } 
                :
                new List<ValidationResult>();
        }
    }

    public enum PlotEnum : int
    {
        @short = 0,
        full = 1
    }
}