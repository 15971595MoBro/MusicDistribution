using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDistribution.Domain.Entities
{
   

    public class Track
    {

        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid ArtistId { get; set; }
        public string ISRC { get; set; } = string.Empty; // Unique
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; } = string.Empty;
        public TrackStatus Status { get; set; } = TrackStatus.Draft;

        // Navigation Properties
        public Artist Artist { get; set; } = null!;
        public ICollection<TrackDistribution> Distributions { get; set; } = new List<TrackDistribution>();
    }

    public enum TrackStatus
    {
        Draft,
        Submitted,
        Distributed
    }
}
