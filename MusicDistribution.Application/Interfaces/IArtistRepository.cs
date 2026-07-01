using MusicDistribution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDistribution.Application.Interfaces
{
    public interface IArtistRepository
    {
        Task<Artist> CreateAsync(Artist artist);
        Task<IEnumerable<Artist>> GetAllAsync();
        Task<Artist?> GetByIdAsync(Guid id);
    }
}
