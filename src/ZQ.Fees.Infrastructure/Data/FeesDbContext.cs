using Microsoft.EntityFrameworkCore;
using ZQ.Fees.Domain.Models;

namespace ZQ.Fees.Infrastructure.Data;

public class FeesDbContext : DbContext
{
    public FeesDbContext(DbContextOptions<FeesDbContext> options) : base(options)
    {
    }

    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Method).HasConversion<int>();
        });
    }
}
