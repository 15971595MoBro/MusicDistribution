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
    public class DspRepository : IDspRepository
    {
        private readonly AppDbContext _context;

        public DspRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DSP>> GetAllAsync()
        {
            return await _context.DSPs.ToListAsync();
        }

        public async Task<DSP?> GetByIdAsync(Guid id)
        {
            return await _context.DSPs.FindAsync(id);
        }
    }
}
