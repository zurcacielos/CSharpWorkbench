using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CSharpWorkbench.Tests.Refactoring;

// ==============================================================================
// EXERCISE: Introduce Caching and Pagination to a Slow Endpoint
// ==============================================================================
// Problem:
// ProductCatalogBad retrieves millions of products from the database on every call.
//
// Smells:
// 1. Missing Pagination: Returning huge datasets causes OutOfMemoryExceptions and 
//    unusable APIs.
// 2. Unnecessary Load: Hitting the DB for static data repeatedly.
//
// Refactoring applied:
// 1. Introduced an ICache dependency (e.g., MemoryCache or Redis).
// 2. Added Skip() and Take() to enforce pagination.
// ==============================================================================

// 1. BAD IMPLEMENTATION
public class ProductCatalogBad
{
    public IEnumerable<string> GetProducts()
    {
        // SMELL: This fetches EVERYTHING. No caching, no limits.
        Console.WriteLine("[DB] Executing expensive DB query for ALL products...");
        return new List<string> { "Prod1", "Prod2", "Prod3", "Prod4", "Prod5" };
    }
}

// ==============================================================================
// REFACTORED SOLUTION
// ==============================================================================

public interface ICache
{
    IEnumerable<string> Get(string key);
    void Set(string key, IEnumerable<string> data);
}

public class ProductCatalogRefactored
{
    private readonly ICache _cache;

    // WHY THIS IS BETTER:
    // We protect the database with a cache and protect memory/network with pagination.
    public ProductCatalogRefactored(ICache cache)
    {
        _cache = cache;
    }

    public IEnumerable<string> GetProducts(int page = 1, int pageSize = 2)
    {
        var cacheKey = "all_products";
        var products = _cache.Get(cacheKey);
        
        if (products == null)
        {
            Console.WriteLine("[DB] Cache miss. Fetching from DB...");
            products = new List<string> { "Prod1", "Prod2", "Prod3", "Prod4", "Prod5" };
            _cache.Set(cacheKey, products);
        }

        // Apply pagination
        return products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }
}

// ==============================================================================
// TESTS
// ==============================================================================
public class CachingPaginationTest
{
    private class FakeCache : ICache
    {
        public bool Hit { get; private set; }
        public IEnumerable<string> Get(string key) { Hit = true; return new List<string> { "P1", "P2", "P3" }; }
        public void Set(string key, IEnumerable<string> data) { }
    }

    [Fact]
    public void RefactoredService_PaginatesCorrectly()
    {
        var service = new ProductCatalogRefactored(new FakeCache());
        
        // Request Page 1, Size 2
        var page1 = service.GetProducts(page: 1, pageSize: 2);
        
        Assert.Equal(2, page1.Count());
        Assert.Contains("P1", page1);
        Assert.Contains("P2", page1);
    }
}
