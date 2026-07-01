using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDistribution.Domain.Entities
{
    public class DSP
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigation Property
        public ICollection<TrackDistribution> TrackDistributions { get; set; } = new List<TrackDistribution>();
    }
}
