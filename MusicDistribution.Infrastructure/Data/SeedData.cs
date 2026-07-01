using MusicDistribution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDistribution.Infrastructure.Data
{
    public static class SeedData
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Artists.Any())
            {
                var artists = new List<Artist>
            {
                new() { Id = Guid.NewGuid(), Name = "Amr Diab", Email = "amr@diab.com", Country = "Egypt" },
                new() { Id = Guid.NewGuid(), Name = "Bahaa Sultan", Email = "bahaa@sultan.com", Country = "Saudi" },
                new() { Id = Guid.NewGuid(), Name = "Hussain Al Jassmi", Email = "hussain@jassmi.com", Country = "UAE" }
            };
                context.Artists.AddRange(artists);
                context.SaveChanges();

                var dsps = new List<DSP>
            {
                new() { Id = Guid.NewGuid(), Name = "Spotify" },
                new() { Id = Guid.NewGuid(), Name = "Apple Music" },
                new() { Id = Guid.NewGuid(), Name = "YouTube Music" }
            };
                context.DSPs.AddRange(dsps);
                context.SaveChanges();

                var tracks = new List<Track>
            {
                new() {
                    Id = Guid.NewGuid(),
                    Title = "Tamally Maak",
                    ArtistId = artists[0].Id,
                    ISRC = "EG-ABC-23-00001",
                    ReleaseDate = new DateTime(2023, 1, 15),
                    Genre = "Pop",
                    Status = TrackStatus.Distributed
                },
                new() {
                    Id = Guid.NewGuid(),
                    Title = "Ya Ana Ya La",
                    ArtistId = artists[0].Id,
                    ISRC = "EG-ABC-23-00002",
                    ReleaseDate = new DateTime(2023, 3, 20),
                    Genre = "Pop",
                    Status = TrackStatus.Submitted
                },
                new() {
                    Id = Guid.NewGuid(),
                    Title = "Ya Tara Ya Habiby",
                    ArtistId = artists[1].Id,
                    ISRC = "LB-XYZ-22-00001",
                    ReleaseDate = new DateTime(2022, 6, 10),
                    Genre = "Pop",
                    Status = TrackStatus.Distributed
                },
                new() {
                    Id = Guid.NewGuid(),
                    Title = "Ana Ghaltan",
                    ArtistId = artists[1].Id,
                    ISRC = "LB-XYZ-22-00002",
                    ReleaseDate = new DateTime(2022, 9, 5),
                    Genre = "Pop",
                    Status = TrackStatus.Draft
                },
                new() {
                    Id = Guid.NewGuid(),
                    Title = "Boshret Kheir",
                    ArtistId = artists[2].Id,
                    ISRC = "UAE-123-21-00001",
                    ReleaseDate = new DateTime(2021, 5, 1),
                    Genre = "Khaliji",
                    Status = TrackStatus.Distributed
                },
                new() {
                    Id = Guid.NewGuid(),
                    Title = "Ahebbak",
                    ArtistId = artists[2].Id,
                    ISRC = "UAE-123-21-00002",
                    ReleaseDate = new DateTime(2021, 8, 15),
                    Genre = "Khaliji",
                    Status = TrackStatus.Submitted
                },
                new() {
                    Id = Guid.NewGuid(),
                    Title = "Nour El Ein",
                    ArtistId = artists[0].Id,
                    ISRC = "EG-ABC-20-00003",
                    ReleaseDate = new DateTime(2020, 2, 14),
                    Genre = "Pop",
                    Status = TrackStatus.Distributed
                },
                new() {
                    Id = Guid.NewGuid(),
                    Title = "Mashy",
                    ArtistId = artists[1].Id,
                    ISRC = "LB-XYZ-24-00003",
                    ReleaseDate = new DateTime(2024, 1, 1),
                    Genre = "Dance",
                    Status = TrackStatus.Draft
                }
            };
                context.Tracks.AddRange(tracks);
                context.SaveChanges();
            }
        }
    }
}
