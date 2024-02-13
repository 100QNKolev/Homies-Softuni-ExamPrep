using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Homies.Data.DataConstants;

namespace Homies.Models
{
    public class EventFormViewModel
    {

        [Required(ErrorMessage = RequireErrorMessage)]
        [StringLength(EventNameMaximumLentgth, MinimumLength = EventNameMininmumLentgth, ErrorMessage = StringLengthErrorMessage)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = RequireErrorMessage)]
        [StringLength(EventDescriptionMaximumLentgth, MinimumLength = EventDescriptionMininmumLentgth, ErrorMessage = StringLengthErrorMessage)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = RequireErrorMessage)]
        public string Start { get; set; } = string.Empty;
        [Required(ErrorMessage = RequireErrorMessage)]
        public string End { get; set; } = string.Empty;

        [Required(ErrorMessage = RequireErrorMessage)]
        public int TypeId { get; set; }

        public IEnumerable<TypeViewModel> Types { get; set; } = new List<TypeViewModel>();
    }
}
