using System;
using Xunit;

namespace CSharpWorkbench.Tests.Refactoring;

// ==============================================================================
// EXERCISE: Replace Complex Conditional Logic with Strategy Pattern
// ==============================================================================
// Problem:
// ShippingCalculatorBad uses a switch statement to determine how to calculate 
// shipping costs based on an enum. This triggers several industry code smells:
//
// 1. Violation of the Open/Closed Principle (OCP - The "O" in SOLID): 
//    Classes should be open for extension but closed for modification. Right now, 
//    to add a new shipping method, we have to modify the existing switch statement.
// 2. High Cyclomatic Complexity / "Switch Statement Smell": 
//    Large switch statements make the method hard to read, maintain, and test thoroughly.
// 3. "Feature Envy" or Blob Class: 
//    As each shipping calculation grows in complexity (e.g., needing to call external 
//    APIs for DHL or FedEx), the calculator class becomes a bloated "God Class" that 
//    knows too much about too many different domains.
//
// Refactoring applied:
// 1. Created an IShippingStrategy interface.
// 2. Extracted each shipping calculation into its own small, focused strategy class.
// 3. Refactored the calculator to accept an IShippingStrategy instead of an enum.
// ==============================================================================

public enum ShippingMethod
{
    Standard,
    Express,
    NextDay
}

public class Order
{
    public decimal WeightInKg { get; set; }
    public decimal DistanceInKm { get; set; }
}

// 1. BAD IMPLEMENTATION (Complex Conditional)
public class ShippingCalculatorBad
{
    public decimal CalculateShippingCost(Order order, ShippingMethod method)
    {
        // SMELL: This switch statement will grow indefinitely as new methods are added.
        switch (method)
        {
            case ShippingMethod.Standard:
                return order.WeightInKg * 1.5m + 5.00m;
                
            case ShippingMethod.Express:
                return order.WeightInKg * 2.5m + 15.00m;
                
            case ShippingMethod.NextDay:
                return order.WeightInKg * 5.0m + 30.00m + (order.DistanceInKm * 0.1m);
                
            default:
                throw new NotSupportedException("Unknown shipping method.");
        }
    }
}

// ==============================================================================
// REFACTORED SOLUTION
// ==============================================================================

// REFACTOR STEP 1: Extract the common behavior into an interface
public interface IShippingStrategy
{
    decimal CalculateCost(Order order);
}

// REFACTOR STEP 2: Create a concrete strategy for each behavior
public class StandardShippingStrategy : IShippingStrategy
{
    public decimal CalculateCost(Order order) => order.WeightInKg * 1.5m + 5.00m;
}

public class ExpressShippingStrategy : IShippingStrategy
{
    public decimal CalculateCost(Order order) => order.WeightInKg * 2.5m + 15.00m;
}

public class NextDayShippingStrategy : IShippingStrategy
{
    public decimal CalculateCost(Order order) => order.WeightInKg * 5.0m + 30.00m + (order.DistanceInKm * 0.1m);
}

// REFACTOR STEP 3: The Context class uses the strategy
public class ShippingCalculatorRefactored
{
    private readonly IShippingStrategy _strategy;

    // WHY THIS IS BETTER:
    // The calculator no longer needs to know HOW to calculate shipping. 
    // It delegates the work to the injected strategy. If we add "DroneDelivery", 
    // we just create a new DroneDeliveryStrategy class without touching this class! (OCP)
    public ShippingCalculatorRefactored(IShippingStrategy strategy)
    {
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    public decimal CalculateShippingCost(Order order)
    {
        return _strategy.CalculateCost(order);
    }
}

// ==============================================================================
// TESTS
// ==============================================================================
public class StrategyPatternTest
{
    private readonly Order _testOrder = new Order { WeightInKg = 10m, DistanceInKm = 100m };

    [Fact]
    public void BadCalculator_ReturnsCorrectValues()
    {
        var calculator = new ShippingCalculatorBad();
        
        Assert.Equal(20.00m, calculator.CalculateShippingCost(_testOrder, ShippingMethod.Standard));
        Assert.Equal(40.00m, calculator.CalculateShippingCost(_testOrder, ShippingMethod.Express));
        Assert.Equal(90.00m, calculator.CalculateShippingCost(_testOrder, ShippingMethod.NextDay));
    }

    [Fact]
    public void RefactoredCalculator_StandardShipping_ReturnsCorrectValue()
    {
        // Arrange
        var strategy = new StandardShippingStrategy();
        var calculator = new ShippingCalculatorRefactored(strategy);
        
        // Act
        var cost = calculator.CalculateShippingCost(_testOrder);
        
        // Assert
        Assert.Equal(20.00m, cost);
    }

    [Fact]
    public void RefactoredCalculator_NextDayShipping_ReturnsCorrectValue()
    {
        // Arrange
        var strategy = new NextDayShippingStrategy();
        var calculator = new ShippingCalculatorRefactored(strategy);
        
        // Act
        var cost = calculator.CalculateShippingCost(_testOrder);
        
        // Assert
        Assert.Equal(90.00m, cost);
    }
    
    // WHY THIS IS BETTER:
    // We can also easily test the strategies in isolation now, without needing to 
    // test the entire Calculator class!
    [Fact]
    public void ExpressStrategy_CalculatesCorrectly()
    {
        var strategy = new ExpressShippingStrategy();
        var cost = strategy.CalculateCost(_testOrder);
        Assert.Equal(40.00m, cost);
    }
}
