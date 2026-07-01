using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CSharpWorkbench.Tests.EF;

// ==============================================================================
// EXERCISE: Fix N+1 Query Problem in Entity Framework
// ==============================================================================
// Problem:
// OrderReportingBad fetches orders, then loops through them, accessing a navigation 
// property which forces EF to execute a new SELECT query for EVERY order.
//
// Smells:
// 1. N+1 Query Anti-Pattern: Executing 1 query for the parent, and N queries for the children.
// 2. Chatty I/O: This causes severe network latency and chokes the database server.
//
// Refactoring applied:
// 1. Used Eager Loading via `.Include()` to fetch parents and children in a single SQL query.
// ==============================================================================

// 1. BAD IMPLEMENTATION
public class OrderReportingBad
{
    public decimal CalculateTotalRevenue(SharedDbContext db)
    {
        decimal total = 0;
        
        // SMELL: This executes 1 query to get all orders.
        var orders = db.Orders.ToList();
        
        // Then we loop through them...
        foreach(var order in orders)
        {
            // SMELL: Accessing the database inside a loop!
            // EF Core will execute a separate query to fetch LineItems for this specific order.
            // If there are 1000 orders, this executes 1000 queries (N+1).
            
            // Note: In older EF (like EF6) or with Proxies enabled, just accessing `order.LineItems` triggers 
            // the lazy load. To demonstrate without relying on proxy packages, we explicitly load it here.
            // The result on the database is identical to a lazy load.
            db.Entry(order).Collection(o => o.LineItems).Load();
            
            // total += order.LineItems.Sum(i => i.Price);
            total += order.LineItems.Sum(i => i.Price);

        }
        
        return total;
    }
}

// ==============================================================================
// REFACTORED SOLUTION
// ==============================================================================
public class OrderReportingRefactored
{
    public decimal CalculateTotalRevenue(SharedDbContext db)
    {
        // WHY THIS IS BETTER:
        // By using .Include(), we tell EF Core to use a SQL JOIN 
        // to retrieve both the Orders and the LineItems in a SINGLE database round-trip.
        var orders = db.Orders
            .Include(o => o.LineItems)
            .ToList();
        
        return orders.Sum(o => o.LineItems.Sum(i => i.Price));
    }
}

// ==============================================================================
// TESTS
// ==============================================================================
public class NPlusOneEfTest : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public NPlusOneEfTest(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void BadReporting_CausesFourQueries_ForThreeOrders()
    {
        // Arrange
        using var db = _fixture.CreateContext();
        var report = new OrderReportingBad();
        
        _fixture.Interceptor.Reset(); // Reset the global query counter
        
        // Act
        var total = report.CalculateTotalRevenue(db);
        
        // Assert
        Assert.Equal(75m, total); // (10+5) + (20+5) + (30+5) = 75
        
        // PROOF OF N+1: 1 query for Orders + 3 queries for LineItems = 4 total queries!
        Assert.Equal(4, _fixture.Interceptor.QueryCount);
    }

    [Fact]
    public void RefactoredReporting_UsesExactlyOneQuery()
    {
        // Arrange
        using var db = _fixture.CreateContext();
        var report = new OrderReportingRefactored();
        
        _fixture.Interceptor.Reset(); // Reset the global query counter
        
        // Act
        var total = report.CalculateTotalRevenue(db);
        
        // Assert
        Assert.Equal(75m, total);
        
        // PROOF OF FIX: Exactly 1 query executed, fetching everything via JOIN.
        Assert.Equal(1, _fixture.Interceptor.QueryCount); 
    }
}
