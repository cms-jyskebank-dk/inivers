using Microsoft.EntityFrameworkCore;
using CustomerEntity = InvestPlatform.Domain.Customer.Customer;

namespace InvestPlatform.Infrastructure.Data;

public class InvestPlatformDbContext : DbContext
{
    public InvestPlatformDbContext(DbContextOptions<InvestPlatformDbContext> options) : base(options)
    {
    }

    public DbSet<CustomerEntity> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CustomerEntity>(entity =>
        {
            entity.HasKey(e => e.CustomerID);
            
            entity.Property(e => e.CustomerID)
                .ValueGeneratedNever();
            
            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.Nationality)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(500);
            
            entity.Property(e => e.CreatedDate)
                .IsRequired();
            
            entity.Property(e => e.LastUpdatedDate)
                .IsRequired();

            entity.ToTable("Customers");
        });
    }
}