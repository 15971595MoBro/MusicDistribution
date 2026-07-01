using MusicDistribution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDistribution.Application.Interfaces
{
    public interface ITrackDistributionRepository
    {
        Task<TrackDistribution> CreateAsync(TrackDistribution distribution);
        Task<IEnumerable<TrackDistribution>> GetByTrackIdAsync(Guid trackId);
    }
}
