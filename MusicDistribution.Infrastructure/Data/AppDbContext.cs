using MusicDistribution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicDistribution.Domain.Entities;

namespace MusicDistribution.Infrastructure.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Artist> Artists => Set<Artist>();
        public DbSet<Track> Tracks => Set<Track>();
        public DbSet<DSP> DSPs => Set<DSP>();
        public DbSet<TrackDistribution> TrackDistributions => Set<TrackDistribution>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Artist Configurations
            modelBuilder.Entity<Artist>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Country).HasMaxLength(100);
            });

            // Track Configurations
            modelBuilder.Entity<Track>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ISRC).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.ISRC).IsUnique();
                entity.Property(e => e.Genre).HasMaxLength(100);

                entity.HasOne(e => e.Artist)
                      .WithMany(a => a.Tracks)
                      .HasForeignKey(e => e.ArtistId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // DSP Configurations
            modelBuilder.Entity<DSP>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            // TrackDistribution Configurations
            modelBuilder.Entity<TrackDistribution>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Track)
                      .WithMany(t => t.Distributions)
                      .HasForeignKey(e => e.TrackId);

                entity.HasOne(e => e.DSP)
                      .WithMany(d => d.TrackDistributions)
                      .HasForeignKey(e => e.DspId);
            });
        }
    }
}
