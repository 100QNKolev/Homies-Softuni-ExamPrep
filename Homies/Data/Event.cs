﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static Homies.Data.DataConstants;
using Type = Homies.Data.Type;

namespace Homies.Data
{
    public class Event
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(EventNameMaximumLentgth)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(EventDescriptionMaximumLentgth)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string OrganiserId { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(OrganiserId))]
        public int Organiser { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        [Required]
        public string TypeId { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(TypeId))]
        public Type Type { get; set; } = null!;

        [Required]
        public IEnumerable<EventParticipant> EventsParticipants = new List<EventParticipant>();
    }
}
