using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CSharpWorkbench.Tests.EF;

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    
    // Navigation property
    public List<LineItem> LineItems { get; set; } = new List<LineItem>();
}

public class LineItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public decimal Price { get; set; }
    
    public Order Order { get; set; }
}

public class SharedDbContext : DbContext
{
    public SharedDbContext(DbContextOptions<SharedDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<LineItem> LineItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasMany(o => o.LineItems)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId);
    }
}
