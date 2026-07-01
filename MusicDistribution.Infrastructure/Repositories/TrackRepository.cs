using Microsoft.EntityFrameworkCore;
using MusicDistribution.Application.Interfaces;
using MusicDistribution.Domain.Entities;
using MusicDistribution.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDistribution.Infrastructure.Repositories
{
    public class TrackRepository : ITrackRepository
    {
        private readonly AppDbContext _context;

        public TrackRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Track> CreateAsync(Track track)
        {
            _context.Tracks.Add(track);
            await _context.SaveChangesAsync();
            return track;
        }

        public async Task<IEnumerable<Track>> GetAllAsync(string? artistId = null, string? genre = null, TrackStatus? status = null)
        {
            var query = _context.Tracks
                .Include(t => t.Artist)
                .AsQueryable();

            if (!string.IsNullOrEmpty(artistId) && Guid.TryParse(artistId, out var artistGuid))
                query = query.Where(t => t.ArtistId == artistGuid);

            if (!string.IsNullOrEmpty(genre))
                query = query.Where(t => t.Genre.ToLower() == genre.ToLower());

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            return await query.OrderByDescending(t => t.ReleaseDate).ToListAsync();
        }

        public async Task<Track?> GetByIdAsync(Guid id)
        {
            return await _context.Tracks
                .Include(t => t.Artist)
                .Include(t => t.Distributions)
                    .ThenInclude(d => d.DSP)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Track> UpdateAsync(Track track)
        {
            _context.Tracks.Update(track);
            await _context.SaveChangesAsync();
            return track;
        }
    }
}
