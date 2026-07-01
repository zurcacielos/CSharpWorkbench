using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CSharpWorkbench.Tests.EF;

// A custom interceptor to intercept and mathematically prove how many SQL queries are executed
public class QueryCounterInterceptor : DbCommandInterceptor
{
    private int _queryCount;

    public int QueryCount => _queryCount;

    public void Reset() => _queryCount = 0;

    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command, 
        CommandEventData eventData, 
        InterceptionResult<DbDataReader> result)
    {
        Interlocked.Increment(ref _queryCount);
        return base.ReaderExecuting(command, eventData, result);
    }
    
    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command, 
        CommandEventData eventData, 
        InterceptionResult<DbDataReader> result, 
        CancellationToken cancellationToken = default)
    {
        Interlocked.Increment(ref _queryCount);
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }
}

// A central fixture to maintain the SQLite in-memory database across multiple tests
public class DatabaseFixture : IDisposable
{
    private readonly SqliteConnection _connection;
    public QueryCounterInterceptor Interceptor { get; }

    public DatabaseFixture()
    {
        Interceptor = new QueryCounterInterceptor();
        
        // SQLite in-memory databases only exist as long as the connection is open.
        // We open it here and keep it open for the lifetime of the fixture.
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        using var context = CreateContext();
        context.Database.EnsureCreated();
        
        SeedDatabase(context);
    }

    public SharedDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<SharedDbContext>()
            .UseSqlite(_connection)
            .AddInterceptors(Interceptor)
            .Options;

        return new SharedDbContext(options);
    }

    private void SeedDatabase(SharedDbContext context)
    {
        // Add 3 orders, each with 2 line items
        var orders = new List<Order>
        {
            new Order { 
                CustomerName = "Alice", 
                LineItems = new List<LineItem> { new LineItem { Price = 10 }, new LineItem { Price = 5 } } 
            },
            new Order { 
                CustomerName = "Bob", 
                LineItems = new List<LineItem> { new LineItem { Price = 20 }, new LineItem { Price = 5 } } 
            },
            new Order { 
                CustomerName = "Charlie", 
                LineItems = new List<LineItem> { new LineItem { Price = 30 }, new LineItem { Price = 5 } } 
            }
        };

        context.Orders.AddRange(orders);
        context.SaveChanges();
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }
}
