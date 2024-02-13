using static Homies.Data.DataConstants;

namespace Homies.Models
{
    public class EventInfoViewModel
    {
        public EventInfoViewModel(int id, string name, DateTime startingTime, string type, string organiser)
        {
            this.Id = id;      
            this.Name = name;
            this.Start = startingTime.ToString(EventDateTimeFormat);
            this.Type = type;
            this.Organiser = organiser;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Start { get; set; }
        public string Type { get; set; }
        public string Organiser { get; set; }

    }
}
