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
    public class TrackDistributionRepository : ITrackDistributionRepository
    {
        private readonly AppDbContext _context;

        public TrackDistributionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TrackDistribution> CreateAsync(TrackDistribution distribution)
        {
            _context.TrackDistributions.Add(distribution);
            await _context.SaveChangesAsync();
            return distribution;
        }

        public async Task<IEnumerable<TrackDistribution>> GetByTrackIdAsync(Guid trackId)
        {
            return await _context.TrackDistributions
                .Where(td => td.TrackId == trackId)
                .Include(td => td.DSP)
                .ToListAsync();
        }
    }
}
