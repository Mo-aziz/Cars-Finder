using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Engine> Engines { get; set; }
    public DbSet<CarProfile> CarProfiles { get; set; }
    public DbSet<CarEngine> CarEngines { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure one-to-many relationship between Manufacturer and Car
        modelBuilder.Entity<Manufacturer>()
            .HasMany(m => m.Cars)
            .WithOne(c => c.Manufacturer)
            .HasForeignKey(c => c.ManufacturerId);

        // Configure one-to-one relationship between Car and CarProfile
        modelBuilder.Entity<Car>()
            .HasOne(c => c.CarProfile)
            .WithOne(cp => cp.Car)
            .HasForeignKey<CarProfile>(cp => cp.CarId);

        // Configure many-to-many relationship between Car and Engine via CarEngine
        modelBuilder.Entity<CarEngine>()
            .HasKey(ce => new { ce.CarId, ce.EngineId });

        modelBuilder.Entity<CarEngine>()
            .HasOne(ce => ce.Car)
            .WithMany(c => c.CarEngines)
            .HasForeignKey(ce => ce.CarId);

        modelBuilder.Entity<CarEngine>()
            .HasOne(ce => ce.Engine)
            .WithMany(e => e.CarEngines)
            .HasForeignKey(ce => ce.EngineId);

        // Seed data for Manufacturers
        modelBuilder.Entity<Manufacturer>().HasData(
            new Manufacturer { Id = 1, Name = "Toyota", Country = "Japan", Description = "Japanese automotive manufacturer" },
            new Manufacturer { Id = 2, Name = "Ford", Country = "USA", Description = "American automotive manufacturer" },
            new Manufacturer { Id = 3, Name = "BMW", Country = "Germany", Description = "German luxury automotive manufacturer" }
        );

        // Seed data for Cars
        modelBuilder.Entity<Car>().HasData(
            new Car { Id = 1, Brand = "Toyota", Model = "Camry", Year = 2022, ManufacturerId = 1 },
            new Car { Id = 2, Brand = "Ford", Model = "Mustang", Year = 2023, ManufacturerId = 2 },
            new Car { Id = 3, Brand = "BMW", Model = "X5", Year = 2023, ManufacturerId = 3 }
        );

        // Seed data for Engines
        modelBuilder.Entity<Engine>().HasData(
            new Engine { Id = 1, Type = "V6", HorsePower = 301 },
            new Engine { Id = 2, Type = "V8", HorsePower = 450 },
            new Engine { Id = 3, Type = "Inline-6", HorsePower = 335 }
        );
    }
}
