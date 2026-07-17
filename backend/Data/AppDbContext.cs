using Microsoft.EntityFrameworkCore;
using VehicleMaintenanceTracker.Entities;

namespace VehicleMaintenanceTracker.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<MaintenanceRecord> MaintenanceRecords => Set<MaintenanceRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasIndex(v => v.PlateNumber).IsUnique();
            entity.Property(v => v.PlateNumber).HasMaxLength(10);
            entity.Property(v => v.Make).HasMaxLength(50);
            entity.Property(v => v.Model).HasMaxLength(50);
        });

        modelBuilder.Entity<MaintenanceRecord>(entity =>
        {
            entity.Property(m => m.Description).HasMaxLength(500);
            entity.Property(m => m.Cost).HasPrecision(10, 2);

            entity.HasOne(m => m.Vehicle)
                  .WithMany(v => v.MaintenanceRecords)
                  .HasForeignKey(m => m.VehicleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}