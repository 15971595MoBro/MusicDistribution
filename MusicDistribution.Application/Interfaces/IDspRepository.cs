using MusicDistribution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDistribution.Application.Interfaces
{
    public interface IDspRepository
    {
        Task<IEnumerable<DSP>> GetAllAsync();
        Task<DSP?> GetByIdAsync(Guid id);
    }
}
