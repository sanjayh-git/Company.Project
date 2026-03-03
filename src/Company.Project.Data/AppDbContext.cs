using Company.Project.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Company.Project.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<MeterReading> MeterReadings => Set<MeterReading>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(builder =>
        {
            builder.HasKey(a => a.Id);            
            builder.Property(a => a.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(a => a.LastName).HasMaxLength(50).IsRequired();
            builder.HasIndex(a => a.AccountId)
              .IsUnique();
        });

        modelBuilder.Entity<MeterReading>(builder =>
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.ReadingValue).HasMaxLength(5).IsRequired();
            builder.HasOne(m => m.Account)
                .WithMany(a => a.MeterReadings)
                .HasPrincipalKey(a => a.AccountId)
                .HasForeignKey(m => m.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
