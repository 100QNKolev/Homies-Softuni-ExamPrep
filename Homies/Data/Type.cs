using System.ComponentModel.DataAnnotations;

using static Homies.Data.DataConstants;


namespace Homies.Data
{
    public class Type
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(TypeNameMaximumLentgth)]
        public string Name { get; set; }

        public IEnumerable<Event> Events = new List<Event>();    
    }
}
