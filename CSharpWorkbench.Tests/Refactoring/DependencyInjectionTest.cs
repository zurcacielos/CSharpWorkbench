namespace CSharpWorkbench.Tests.Refactoring;

// ==============================================================================
// EXERCISE: Extract Interface and Apply Dependency Injection
// ==============================================================================
// Problem:
// OrderProcessorBad is tightly coupled to the concrete class SqlDatabase.
// This triggers several well-known industry code smells and anti-patterns:
// 1. "New is Glue"
// This is a very popular catchphrase in the .NET community. It means that every time you use 
// the 'new' keyword to instantiate a service or component, you are permanently "gluing" your 
// class to that specific concrete implementation. 
//
// 2. Tight Coupling (High Coupling)
// Because of the 'new' keyword, OrderProcessorBad is tightly coupled to SqlDatabase. 
// You cannot use OrderProcessorBad anywhere without dragging the SQL database dependency along 
// with it. A core goal of software architecture is achieving Loose Coupling.
//
// 3. Hidden Dependencies (or Dishonest API)
// By instantiating the database inside the method, the dependency is hidden. If another developer 
// looks at 'new OrderProcessorBad()', they have no idea that calling ProcessOrder will attempt 
// to connect to a database. Injecting via the constructor makes the API "honest".
//
// 4. Violation of the Dependency Inversion Principle (DIP)
// This is the "D" in SOLID. DIP states that high-level modules (your business logic) should not 
// depend on low-level modules (like database access). Instead, both should depend on abstractions.
//
// 5. Violation of the Single Responsibility Principle (SRP)
// This is the "S" in SOLID. A class should only have one reason to change. Right now, 
// OrderProcessorBad has two responsibilities: validating/processing the order, and knowing 
// how/when to create the database connection object.
//
// 6. "Untestable Code" (Testing Smell)
// A true Unit Test must run in memory, be incredibly fast, and have zero side effects. Because 
// the class forces the creation of a real SqlDatabase, you cannot write a Unit Test for it. You 
// are forced to write an Integration Test, which is slower, harder to set up, and prone to failures.
//
// Refactoring applied:
// 1. Extracted an interface (IDatabase) from SqlDatabase.
// 2. Refactored OrderProcessorRefactored so it accepts this dependency via constructor injection.
// 3. Wrote tests for OrderProcessorRefactored by injecting a fake/mock implementation.
// ==============================================================================

// 1. ORIGINAL CONCRETE DEPENDENCY
public class SqlDatabase : IDatabase
{
    // Pretend this actually connects to a SQL Server
    public bool SaveOrder(string orderId, decimal amount)
    {
        // Simulate a slow database call
        Console.WriteLine($"[DB] Saving order {orderId} for ${amount} to SQL Server...");
        return true;
    }
}

// 2. BAD IMPLEMENTATION (Tightly Coupled)
public class OrderProcessorBad
{
    public bool ProcessOrder(string orderId, decimal amount)
    {
        // TIGHT COUPLING: We are instantiating the concrete dependency directly.
        // It's impossible to test this without running the actual SqlDatabase logic.
        var db = new SqlDatabase();

        if (amount <= 0) return false;

        return db.SaveOrder(orderId, amount);
    }
}

// ==============================================================================
// REFACTORED SOLUTION
// ==============================================================================

// REFACTOR STEP 1: Extract an interface to decouple the processor from the concrete DB
public interface IDatabase
{
    bool SaveOrder(string orderId, decimal amount);
}

// REFACTOR STEP 2: Use Constructor Injection for the dependency
public class OrderProcessorRefactored
{
    private readonly IDatabase _database;

    // WHY THIS IS BETTER: 
    // By injecting IDatabase, we invert the control. The caller decides which 
    // implementation of IDatabase to provide. In production, we provide SqlDatabase.
    // In unit tests, we provide a fake/mock.
    public OrderProcessorRefactored(IDatabase database)
    {
        _database = database ?? throw new ArgumentNullException(nameof(database));
    }

    public bool ProcessOrder(string orderId, decimal amount)
    {
        if (amount <= 0) return false;

        // WHY THIS IS BETTER:
        // We rely on the abstraction (_database) instead of the concrete implementation.
        return _database.SaveOrder(orderId, amount);
    }
}

// ==============================================================================
// TESTS
// ==============================================================================
public class DependencyInjectionTest
{
    [Fact]
    public void BadProcessor_AlwaysHitsRealDatabase()
    {
        var processor = new OrderProcessorBad();
        var result = processor.ProcessOrder("ORD-123", 150.00m);

        Assert.True(result);
        // Note: We can't easily verify if SaveOrder was actually called without hitting the real DB!
    }

    // A fake implementation of IDatabase just for testing
    private class FakeDatabase : IDatabase
    {
        public bool WasCalled { get; private set; }
        public string LastOrderId { get; private set; }

        public bool SaveOrder(string orderId, decimal amount)
        {
            WasCalled = true;
            LastOrderId = orderId;
            return true; // Simulate success without hitting a real DB
        }
    }

    [Fact]
    public void RefactoredProcessor_UsesInjectedDependency()
    {
        // 1. Arrange: Create our fake database and inject it
        var fakeDb = new FakeDatabase();
        var processor = new OrderProcessorRefactored(fakeDb);

        // 2. Act
        var result = processor.ProcessOrder("ORD-456", 200.00m);

        // 3. Assert
        Assert.True(result);

        // WHY THIS IS BETTER:
        // We can now verify the exact behavior of our processor without side effects!
        Assert.True(fakeDb.WasCalled);
        Assert.Equal("ORD-456", fakeDb.LastOrderId);
    }
}
