using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }

    IQueryable<Order> IApplicationDbContext.Orders => Orders;

    public void Add(Order order)
    {
        Orders.Add(order);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(order => order.Id);

            entity.Property(order => order.TenantId)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(order => order.CustomerName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(order => order.TotalAmount)
                .HasPrecision(18, 2);

            entity.HasIndex(order => order.TenantId);
        });
    }
}
