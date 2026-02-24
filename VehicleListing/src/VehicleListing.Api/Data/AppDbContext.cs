using Microsoft.EntityFrameworkCore;
using VehicleListing.Api.Models;

namespace VehicleListing.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Dealer> Dealers => Set<Dealer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Dealer>(entity =>
        {
            entity.HasMany(d => d.Vehicles)
                  .WithOne(v => v.Dealer)
                  .HasForeignKey(v => v.DealerId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.Property(d => d.CreatedAt)
                  .HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.Property(v => v.Price)
                  .HasPrecision(10, 2);

            entity.Property(v => v.Status)
                  .HasMaxLength(50);

            entity.Property(v => v.CreatedAt)
                  .HasDefaultValueSql("GETUTCDATE()");
        });
    }
}
