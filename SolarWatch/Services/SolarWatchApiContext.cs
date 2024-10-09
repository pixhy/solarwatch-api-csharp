using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SolarWatch.Services;

public class SolarWatchApiContext : DbContext
{
    public SolarWatchApiContext(DbContextOptions<SolarWatchApiContext> options)
        : base(options)
    {
    }

    public DbSet<City> Cities { get; set; }
    public DbSet<SunriseAndSunset> SunriseAndSunsets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Sunrise property
        modelBuilder.Entity<SunriseAndSunset>()
            .Property(e => e.Sunrise)
            .HasConversion(
                v => v.ToTimeSpan(), // TimeOnly to TimeSpan conversion
                v => TimeOnly.FromTimeSpan(v)) // TimeSpan to TimeOnly conversion
            .HasColumnType("time");

        // Configure Sunset property
        modelBuilder.Entity<SunriseAndSunset>()
            .Property(e => e.Sunset)
            .HasConversion(
                v => v.ToTimeSpan(),
                v => TimeOnly.FromTimeSpan(v))
            .HasColumnType("time");

        // Configure Date property
        modelBuilder.Entity<SunriseAndSunset>()
            .Property(e => e.Date)
            .HasConversion(
                v => v.ToDateTime(TimeOnly.MinValue), // DateOnly to DateTime
                v => DateOnly.FromDateTime(v)) // DateTime to DateOnly
            .HasColumnType("date");
        
        base.OnModelCreating(modelBuilder);
    }
}