using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicDistribution.Application.DTOs;
using MusicDistribution.Application.Interfaces;
using MusicDistribution.Domain.Entities;

namespace MusicDistribution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistRepository _artistRepository;

        public ArtistsController(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Artist artist)
        {
            if (string.IsNullOrWhiteSpace(artist.Name))
                return BadRequest(new { error = "Name is required" });

            if (string.IsNullOrWhiteSpace(artist.Email) || !artist.Email.Contains("@"))
                return BadRequest(new { error = "Valid email is required" });

            var created = await _artistRepository.CreateAsync(artist);
            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var artists = await _artistRepository.GetAllAsync();
            var dtos = artists.Select(a => new ArtistDto
            {
                Id = a.Id,
                Name = a.Name,
                Email = a.Email,
                Country = a.Country
            });
            return Ok(dtos);
        }
    }
}
