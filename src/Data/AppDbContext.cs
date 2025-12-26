using Microsoft.EntityFrameworkCore;
using TariffCalculator.Api.Entities;

namespace TariffCalculator.Api.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<CalculationRecord> Calculations => Set<CalculationRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Optional: configure table and column mappings explicitly
        modelBuilder.Entity<CalculationRecord>(entity =>
        {
            entity.ToTable("calculation_records");
            entity.Property(e => e.CountryOfOrigin).HasMaxLength(100);
            entity.Property(e => e.HtsCode).HasMaxLength(50);
            entity.Property(e => e.TariffType).HasMaxLength(50);
        });
    }
}
