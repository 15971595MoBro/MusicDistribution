using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicDistribution.Application.DTOs;
using MusicDistribution.Application.Interfaces;
using MusicDistribution.Domain.Entities;

namespace MusicDistribution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TracksController : ControllerBase
    {
        private readonly ITrackRepository _trackRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IDspRepository _dspRepository;
        private readonly ITrackDistributionRepository _distributionRepository;

        public TracksController(
            ITrackRepository trackRepository,
            IArtistRepository artistRepository,
            IDspRepository dspRepository,
            ITrackDistributionRepository distributionRepository)
        {
            _trackRepository = trackRepository;
            _artistRepository = artistRepository;
            _dspRepository = dspRepository;
            _distributionRepository = distributionRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Track track)
        {
            if (string.IsNullOrWhiteSpace(track.Title))
                return BadRequest(new { error = "Title is required" });

            if (track.ArtistId == Guid.Empty)
                return BadRequest(new { error = "ArtistId is required" });

            var artist = await _artistRepository.GetByIdAsync(track.ArtistId);
            if (artist == null)
                return BadRequest(new { error = "Artist not found" });

            if (string.IsNullOrWhiteSpace(track.ISRC))
                return BadRequest(new { error = "ISRC is required" });

            var created = await _trackRepository.CreateAsync(track);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
    [FromQuery] string? artistId = null,
    [FromQuery] string? genre = null,
    [FromQuery] string? status = null)
        {
            TrackStatus? trackStatus = null;

            if (!string.IsNullOrEmpty(status))
            {
                // جرب parse الـ status string
                if (Enum.TryParse<TrackStatus>(status, true, out var parsed))
                {
                    trackStatus = parsed;
                }
                else
                {
                    // لو الـ parse فشل، جرب بالرقم
                    if (int.TryParse(status, out var statusInt) && Enum.IsDefined(typeof(TrackStatus), statusInt))
                    {
                        trackStatus = (TrackStatus)statusInt;
                    }
                }
            }

            var tracks = await _trackRepository.GetAllAsync(artistId, genre, trackStatus);

            var dtos = tracks.Select(t => new TrackDto
            {
                Id = t.Id,
                Title = t.Title,
                ArtistId = t.ArtistId,
                ArtistName = t.Artist?.Name ?? "Unknown",
                ISRC = t.ISRC,
                ReleaseDate = t.ReleaseDate,
                Genre = t.Genre,
                Status = t.Status.ToString()
            });

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var track = await _trackRepository.GetByIdAsync(id);
            if (track == null)
                return NotFound(new { error = "Track not found" });

            var dto = new TrackDto
            {
                Id = track.Id,
                Title = track.Title,
                ArtistId = track.ArtistId,
                ArtistName = track.Artist?.Name ?? "Unknown",
                ISRC = track.ISRC,
                ReleaseDate = track.ReleaseDate,
                Genre = track.Genre,
                Status = track.Status.ToString(),
                Distributions = track.Distributions?.Select(d => new DistributionDto
                {
                    Id = d.Id,
                    DspName = d.DSP?.Name ?? "Unknown",
                    SubmittedAt = d.SubmittedAt,
                    Status = d.Status.ToString()
                }).ToList() ?? new List<DistributionDto>()
            };

            return Ok(dto);
        }

        [HttpPost("{id}/distribute")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Distribute(Guid id, [FromBody] List<Guid> dspIds)
        {
            var track = await _trackRepository.GetByIdAsync(id);
            if (track == null)
                return NotFound(new { error = "Track not found" });

            if (dspIds == null || !dspIds.Any())
                return BadRequest(new { error = "At least one DSP is required" });

            foreach (var dspId in dspIds)
            {
                var dsp = await _dspRepository.GetByIdAsync(dspId);
                if (dsp == null)
                    return BadRequest(new { error = $"DSP with id {dspId} not found" });

                var distribution = new TrackDistribution
                {
                    TrackId = id,
                    DspId = dspId,
                    SubmittedAt = DateTime.UtcNow,
                    Status = DistributionStatus.Pending
                };

                await _distributionRepository.CreateAsync(distribution);
            }

            // Update track status to Submitted if it was Draft
            if (track.Status == TrackStatus.Draft)
            {
                track.Status = TrackStatus.Submitted;
                await _trackRepository.UpdateAsync(track);
            }

            return Ok(new { message = "Track distributed successfully" });
        }

        [HttpPatch("{id}/status")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] TrackStatus status)
        {
            var track = await _trackRepository.GetByIdAsync(id);
            if (track == null)
                return NotFound(new { error = "Track not found" });

            track.Status = status;
            await _trackRepository.UpdateAsync(track);

            return Ok(track);
        }
    }
}
