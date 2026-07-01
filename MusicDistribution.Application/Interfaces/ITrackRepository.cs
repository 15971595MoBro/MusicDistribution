using MusicDistribution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDistribution.Application.Interfaces
{
    public interface ITrackRepository
    {
        Task<Track> CreateAsync(Track track);
        Task<IEnumerable<Track>> GetAllAsync(string? artistId = null, string? genre = null, TrackStatus? status = null);
        Task<Track?> GetByIdAsync(Guid id);
        Task<Track> UpdateAsync(Track track);
    }
}
