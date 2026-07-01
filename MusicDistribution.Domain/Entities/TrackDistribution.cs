using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDistribution.Domain.Entities
{
    public class TrackDistribution
    {
        public Guid Id { get; set; }
        public Guid TrackId { get; set; }
        public Guid DspId { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DistributionStatus Status { get; set; } = DistributionStatus.Pending;

        // Navigation Properties
        public Track Track { get; set; } = null!;
        public DSP DSP { get; set; } = null!;
    }

    public enum DistributionStatus
    {
        Pending,
        Live,
        Rejected
    }
}
