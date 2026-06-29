using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace CSharpWorkbench.Tests.CSharpFundamentals;

/// <summary>
/// Comprehensive LINQ Practice Kata.
/// Review the examples, then try typing the equivalent logic in the "YOUR TURN" sections below each one
/// to build muscle memory for LINQ operations.
/// </summary>
public class LinqTest
{
    #region Sample Data Setup

    public record Product(int Id, string Name, string Category, decimal Price, int Stock);
    public record Customer(int Id, string Name, string City);
    public record Order(int Id, int CustomerId, DateTime OrderDate, decimal TotalAmount);

    private readonly List<Product> _products =
    [
        new(1, "Laptop", "Electronics", 1200.00m, 10),
        new(2, "Smartphone", "Electronics", 800.00m, 25),
        new(3, "Headphones", "Electronics", 150.00m, 50),
        new(4, "Desk", "Furniture", 300.00m, 5),
        new(5, "Chair", "Furniture", 120.00m, 15),
        new(6, "Notebook", "Stationery", 5.00m, 100),
        new(7, "Pen", "Stationery", 2.00m, 200),
        new(8, "Tablet", "Electronics", 450.00m, 0), // Out of stock
    ];

    private readonly List<Customer> _customers =
    [
        new(1, "Alice", "New York"),
        new(2, "Bob", "London"),
        new(3, "Charlie", "New York"),
        new(4, "Diana", "Paris")
    ];

    private readonly List<Order> _orders =
    [
        new(101, 1, new DateTime(2023, 1, 15), 1200.00m),
        new(102, 1, new DateTime(2023, 2, 20), 150.00m),
        new(103, 2, new DateTime(2023, 1, 10), 300.00m),
        new(104, 3, new DateTime(2023, 3, 5), 800.00m)
    ];

    #endregion

    [Fact]
    public void Practice_1_Filtering()
    {
        // ---------------------------------------------------------
        // 1. Where: Filters a sequence based on a predicate.
        var electronics = _products.Where(p => p.Category == "Electronics");

        // YOUR TURN:
        var electronics2 = _products.Where(p => p.Category == "Electronics");
        var e3 = _products.Where(p => p.Category == "Electronics");

        // ---------------------------------------------------------
        // 2. Where (with index): Filters based on condition AND index.
        var everySecondProduct = _products.Where((p, index) => index % 2 == 1);

        // YOUR TURN:
        var e2p = _products.Where((p, index) => index % 2 == 1);
        var ep23 = _products.Where((p, i) => i % 2 == 1);

        // ---------------------------------------------------------
        // 3. OfType: Filters elements of a specific type.
        object[] mixedData = { "Hello", 1, "World", 2, 3.5m };
        var stringsOnly = mixedData.OfType<string>();

        // YOUR TURN:
        var stringsO = mixedData.OfType<string>();
        var str3 = mixedData.OfType<int>();
        var str4 = mixedData.OfType<string>();
    }

    [Fact]
    public void Practice_2_Projection()
    {
        // ---------------------------------------------------------
        // 1. Select: Projects each element into a new form (e.g., extracting a property).
        var productNames = _products.Select(p => p.Name);

        // YOUR TURN:
        var pn = _products.Select(p => p.Name);

        // ---------------------------------------------------------
        // 2. Select (Anonymous types / Tuples): Creating new shapes of data.
        var productSummaries = _products.Select(p => new { p.Name, p.Price });

        // YOUR TURN:
        var ps = _products.Select(p => new { p.Name, p.Price });


        // ---------------------------------------------------------
        // 3. Select (with index): Map values using their index.
        var indexedNames = _products.Select((p, index) => $"{index + 1}. {p.Name}");

        // YOUR TURN:
        var indexedN = _products.Select((p, index) => $"{index + 1}. {p.Name}");

        // ---------------------------------------------------------
        // 4. SelectMany: Flattens a sequence of sequences.
        // Example: If a customer had a List<Order>, we could get ALL orders across all customers.
        List<List<int>> nestedLists = new() { new() { 1, 2 }, new() { 3, 4 } };
        var flattened = nestedLists.SelectMany(list => list);

        // YOUR TURN:
        var flat = nestedLists.SelectMany(list => list);

        // ---------------------------------------------------------
        // 5. Cast: Casts elements to a specific type (throws InvalidCastException if it fails).
        // Best used when you know 100% all elements match the type.
        var castedObjects = new object[] { 1, 2, 3 }.Cast<int>();

        // YOUR TURN:
        var co = new object[] { 1, 2, 3 }.Cast<int>();

    }

    [Fact]
    public void Practice_3_Partitioning()
    {
        // ---------------------------------------------------------
        // 1. Take: Gets the first N elements.
        var top3 = _products.Take(3);

        // YOUR TURN:


        // ---------------------------------------------------------
        // 2. Take (Range/Last N): Gets the last N elements (using index from end, C# 8+)
        // var last2 = _products.Take(^2..); // Or just _products.TakeLast(2);
        var last2 = _products.TakeLast(2);

        // YOUR TURN:


        // ---------------------------------------------------------
        // 3. TakeWhile: Takes elements as long as a condition is true, stops at the first failure.
        var untilExpensive = _products.TakeWhile(p => p.Price < 1000m);

        // YOUR TURN:


        // ---------------------------------------------------------
        // 4. Skip: Bypasses the first N elements.
        var skipFirst3 = _products.Skip(3);

        // YOUR TURN:


        // ---------------------------------------------------------
        // 5. SkipWhile: Bypasses elements as long as a condition is true, then returns the rest.
        var skipCheap = _products.SkipWhile(p => p.Price < 1000m);

        // YOUR TURN:


        // ---------------------------------------------------------
        // 6. Chunk: Splits elements into chunks of a given size. (Very useful for batch processing)
        var productBatches = _products.Chunk(3); // Yields arrays of max 3 products

        // YOUR TURN:


    }

    [Fact]
    public void Practice_4_Ordering()
    {
        // ---------------------------------------------------------
        // 1. OrderBy: Sorts ascending.
        var sortedByPrice = _products.OrderBy(p => p.Price);

        // YOUR TURN:


        // ---------------------------------------------------------
        // 2. OrderByDescending: Sorts descending.
        var mostExpensiveFirst = _products.OrderByDescending(p => p.Price);

        // YOUR TURN:


        // ---------------------------------------------------------
        // 3. ThenBy: Secondary sort ascending (used after OrderBy).
        var sortedByCategoryThenName = _products
            .OrderBy(p => p.Category)
            .ThenBy(p => p.Name);

        // YOUR TURN:


        // ---------------------------------------------------------
        // 4. ThenByDescending: Secondary sort descending.
        var sortedByCategoryThenPriceDesc = _products
            .OrderBy(p => p.Category)
            .ThenByDescending(p => p.Price);

        // YOUR TURN:


        // ---------------------------------------------------------
        // 5. Reverse: Reverses the sequence (not a sort, just flips the order).
        var reversedProducts = _products.AsEnumerable().Reverse();

        // YOUR TURN:


    }

    [Fact]
    public void Practice_5_Grouping()
    {
        // ---------------------------------------------------------
        // 1. GroupBy: Groups elements that share a common key. Returns IGrouping<TKey, TElement>
        var productsByCategory = _products.GroupBy(p => p.Category);

        // Example of using the result:
        // foreach(var group in productsByCategory) {
        //     Console.WriteLine(group.Key); // Category name
        //     foreach(var p in group) { ... }
        // }

        // YOUR TURN:


        // ---------------------------------------------------------
        // 2. GroupBy (with element selector): Groups elements, but transforms the elements in the group.
        var namesByCategory = _products.GroupBy(
            keySelector: p => p.Category,
            elementSelector: p => p.Name);

        // YOUR TURN:


        // ---------------------------------------------------------
        // 3. ToLookup: Like GroupBy, but executes immediately and returns a dictionary-like ILookup.
        var productLookup = _products.ToLookup(p => p.Category);
        var electronics = productLookup["Electronics"]; // Safe even if key doesn't exist (returns empty)

        // YOUR TURN:


    }

    [Fact]
    public void Practice_6_SetOperations()
    {
        var numbers1 = new[] { 1, 2, 3, 3, 4, 5 };
        var numbers2 = new[] { 4, 5, 6, 7 };

        // ---------------------------------------------------------
        // 1. Distinct: Removes duplicates.
        var uniqueNumbers = numbers1.Distinct(); // 1, 2, 3, 4, 5

        // YOUR TURN:


        // ---------------------------------------------------------
        // 2. DistinctBy: Removes duplicates based on a specific property (e.g., one product per category).
        var distinctCategories = _products.DistinctBy(p => p.Category);

        // YOUR TURN:


        // ---------------------------------------------------------
        // 3. Union: Combines sequences and removes duplicates.
        var union = numbers1.Union(numbers2); // 1, 2, 3, 4, 5, 6, 7

        // YOUR TURN:


        // ---------------------------------------------------------
        // 4. Intersect: Returns elements present in BOTH sequences.
        var intersection = numbers1.Intersect(numbers2); // 4, 5

        // YOUR TURN:


        // ---------------------------------------------------------
        // 5. Except: Returns elements from the first sequence that are NOT in the second.
        var except = numbers1.Except(numbers2); // 1, 2, 3

        // YOUR TURN:


        // ---------------------------------------------------------
        // 6. UnionBy, IntersectBy, ExceptBy: Same as above, but compares based on a specific key selector.
        var diffCustomers = _customers.ExceptBy(new[] { "Alice" }, c => c.Name);

        // YOUR TURN:


    }

    [Fact]
    public void Practice_7_ElementExtractors()
    {
        // ---------------------------------------------------------
        // 1. First: Gets first element, throws exception if sequence is empty.
        var firstProduct = _products.First();
        var firstFurniture = _products.First(p => p.Category == "Furniture");

        // YOUR TURN:


        // ---------------------------------------------------------
        // 2. FirstOrDefault: Gets first element, returns default (null for objects) if empty. SAFEST.
        var missingProduct = _products.FirstOrDefault(p => p.Name == "NonExistent"); // Returns null

        // YOUR TURN:


        // ---------------------------------------------------------
        // 3. Last / LastOrDefault: Gets the last element. Note: Can be slow if not a list/array.
        var lastProduct = _products.LastOrDefault();

        // YOUR TURN:


        // ---------------------------------------------------------
        // 4. Single: Gets the ONLY element matching. Throws if empty OR if > 1 match.
        var alice = _customers.Single(c => c.Name == "Alice");

        // YOUR TURN:


        // ---------------------------------------------------------
        // 5. SingleOrDefault: Gets the only match. Returns null if empty. Throws if > 1 match.
        var maybeAlice = _customers.SingleOrDefault(c => c.Name == "Alice");

        // YOUR TURN:


        // ---------------------------------------------------------
        // 6. ElementAt / ElementAtOrDefault: Gets element at specific index.
        var thirdProduct = _products.ElementAt(2); // Index 2
        var safeOob = _products.ElementAtOrDefault(100); // null

        // YOUR TURN:


    }

    [Fact]
    public void Practice_8_Quantifiers()
    {
        // ---------------------------------------------------------
        // 1. Any: Returns true if sequence has ANY elements.
        var hasProducts = _products.Any();

        // YOUR TURN:


        // ---------------------------------------------------------
        // 2. Any (with predicate): Returns true if any element matches.
        var hasOutOfStock = _products.Any(p => p.Stock == 0);

        // YOUR TURN:


        // ---------------------------------------------------------
        // 3. All: Returns true if ALL elements match the predicate.
        var allHavePrice = _products.All(p => p.Price > 0);

        // YOUR TURN:


        // ---------------------------------------------------------
        // 4. Contains: Returns true if sequence contains the specific element.
        var specificCustomer = _customers.First();
        var hasCustomer = _customers.Contains(specificCustomer);

        // YOUR TURN:


    }

    [Fact]
    public void Practice_9_Aggregation()
    {
        // ---------------------------------------------------------
        // 1. Count: Returns the number of elements.
        var totalCount = _products.Count; // Property on List
        var linqCount = _products.Count(); // Extension method
        var electronicsCount = _products.Count(p => p.Category == "Electronics"); // With predicate

        // YOUR TURN:


        // ---------------------------------------------------------
        // 2. Sum: Calculates the sum of numeric values.
        var totalStock = _products.Sum(p => p.Stock);
        var totalInventoryValue = _products.Sum(p => p.Stock * p.Price);

        // YOUR TURN:


        // ---------------------------------------------------------
        // 3. Min / Max: Gets the minimum / maximum scalar value.
        var cheapestPrice = _products.Min(p => p.Price);
        var mostExpensivePrice = _products.Max(p => p.Price);

        // YOUR TURN:


        // ---------------------------------------------------------
        // 4. MinBy / MaxBy: Gets the ACTUAL OBJECT that has the min/max value. (Super useful!)
        var cheapestProduct = _products.MinBy(p => p.Price); // Returns a Product
        var mostExpensiveProduct = _products.MaxBy(p => p.Price);

        // YOUR TURN:


        // ---------------------------------------------------------
        // 5. Average: Calculates the average of numeric values.
        var averagePrice = _products.Average(p => p.Price);

        // YOUR TURN:


        // ---------------------------------------------------------
        // 6. Aggregate: Custom folding/reduction (like Javascript's reduce).
        var namesCommaSeparated = _customers
            .Select(c => c.Name)
            .Aggregate((current, next) => current + ", " + next); // Alice, Bob, Charlie, Diana

        // YOUR TURN:


    }

    [Fact]
    public void Practice_10_Joining()
    {
        // ---------------------------------------------------------
        // 1. Join: Inner join two sequences on a matching key.
        var orderDetails = _orders.Join(
            inner: _customers,
            outerKeySelector: order => order.CustomerId,
            innerKeySelector: cust => cust.Id,
            resultSelector: (order, cust) => new { cust.Name, order.TotalAmount, order.OrderDate }
        );

        // YOUR TURN:


        // ---------------------------------------------------------
        // 2. GroupJoin: Left Outer Join equivalent (groups matching inner elements for each outer).
        var customerOrders = _customers.GroupJoin(
            inner: _orders,
            outerKeySelector: cust => cust.Id,
            innerKeySelector: order => order.CustomerId,
            resultSelector: (cust, ordersForCust) => new { cust.Name, OrderCount = ordersForCust.Count() }
        );

        // YOUR TURN:


        // ---------------------------------------------------------
        // 3. Zip: Merges two sequences like a zipper (index 0 with index 0, index 1 with index 1).
        var numbers = new[] { 1, 2, 3 };
        var words = new[] { "One", "Two", "Three" };
        var zipped = numbers.Zip(words, (n, w) => $"{n} = {w}"); // "1 = One", etc.

        // YOUR TURN:


    }

    [Fact]
    public void Practice_11_Generation()
    {
        // ---------------------------------------------------------
        // 1. Enumerable.Range: Generates a sequence of integral numbers.
        var oneToTen = Enumerable.Range(1, 10);

        // YOUR TURN:


        // ---------------------------------------------------------
        // 2. Enumerable.Repeat: Generates a sequence that contains one repeated value.
        var tenAs = Enumerable.Repeat("A", 10);

        // YOUR TURN:


        // ---------------------------------------------------------
        // 3. Enumerable.Empty: Generates an empty sequence of a specific type (great for avoiding null returns).
        var emptyStrings = Enumerable.Empty<string>();

        // YOUR TURN:


        // ---------------------------------------------------------
        // 4. DefaultIfEmpty: Returns elements, or a default value (usually null/0) if the sequence is empty.
        var emptyList = new List<int>();
        var result = emptyList.DefaultIfEmpty(-1); // Returns a sequence containing [-1]

        // YOUR TURN:


    }

    [Fact]
    public void Practice_12_Conversion_And_Execution()
    {
        // ---------------------------------------------------------
        // Note: Methods like Where, Select, OrderBy use "Deferred Execution". 
        // They don't DO anything until you iterate over them or convert them.

        var query = _products.Where(p => p.Price > 100); // Hasn't executed yet.

        // 1. ToList: Executes query immediately and returns a List<T>.
        var list = query.ToList();

        // YOUR TURN:


        // ---------------------------------------------------------
        // 2. ToArray: Executes query immediately and returns a T[].
        var array = query.ToArray();

        // YOUR TURN:


        // ---------------------------------------------------------
        // 3. ToDictionary: Executes and creates a dictionary using a key selector.
        // Careful: Throws if there are duplicate keys!
        var dictionary = _products.ToDictionary(p => p.Id);
        var specificProduct = dictionary[1];

        // YOUR TURN:


        // ---------------------------------------------------------
        // 4. ToDictionary (Key and Value selector):
        var nameToPrice = _products.ToDictionary(
            keySelector: p => p.Name,
            elementSelector: p => p.Price);

        // YOUR TURN:


        // ---------------------------------------------------------
        // 5. ToHashSet: Executes and creates a HashSet (great for fast O(1) lookups and distinct values).
        var categorySet = _products.Select(p => p.Category).ToHashSet();

        // YOUR TURN:


    }

    [Fact]
    public void Practice_13_Complex_Chaining_Tricks()
    {
        // ---------------------------------------------------------
        // Challenge 1: Find the total revenue from customers in "New York"
        var nyRevenue = _customers
            .Where(c => c.City == "New York")
            .Join(_orders,
                  c => c.Id,
                  o => o.CustomerId,
                  (c, o) => o.TotalAmount)
            .Sum();

        // YOUR TURN:


        // ---------------------------------------------------------
        // Challenge 2: Find the most expensive product in each category
        var topProductsByCategory = _products
            .GroupBy(p => p.Category)
            .Select(g => new
            {
                Category = g.Key,
                MostExpensive = g.MaxBy(p => p.Price)
            });

        // YOUR TURN:


        // ---------------------------------------------------------
        // Challenge 3: Get a paginated list of products (Page 2, 3 items per page), sorted by name
        int pageNumber = 2;
        int pageSize = 3;
        var paginated = _products
            .OrderBy(p => p.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // YOUR TURN:


    }
}
