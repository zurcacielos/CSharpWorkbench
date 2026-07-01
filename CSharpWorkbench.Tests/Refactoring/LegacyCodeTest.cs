using System;
using Xunit;

namespace CSharpWorkbench.Tests.Refactoring;

// ==============================================================================
// EXERCISE: Add Seams and Unit Tests to Legacy Code
// ==============================================================================
// Problem:
// LegacyGreetingService relies on static `DateTime.Now`, making it impossible 
// to write deterministic tests.
//
// Smells:
// 1. Static Cling: Relying directly on static framework methods (like DateTime.Now 
//    or Guid.NewGuid) hides dependencies and prevents mocking.
// 2. Non-deterministic Tests: A test written for "Good Morning" will fail if run 
//    at 2 PM.
//
// Refactoring applied:
// 1. Created a "Seam" by extracting an IDateTimeProvider interface.
// 2. Injected the provider, allowing tests to manipulate time freely.
// ==============================================================================

// 1. BAD IMPLEMENTATION
public class LegacyGreetingServiceBad
{
    public string GetGreeting()
    {
        // SMELL: Hidden static dependency.
        var currentHour = DateTime.Now.Hour;
        
        if (currentHour < 12) return "Good Morning";
        if (currentHour < 18) return "Good Afternoon";
        return "Good Evening";
    }
}

// ==============================================================================
// REFACTORED SOLUTION
// ==============================================================================

// REFACTOR STEP 1: Create the Seam
public interface IDateTimeProvider
{
    DateTime Now { get; }
}

// Used in production
public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
}

// REFACTOR STEP 2: Inject the seam
public class LegacyGreetingServiceRefactored
{
    private readonly IDateTimeProvider _timeProvider;

    // WHY THIS IS BETTER:
    // We now have total control over time inside our unit tests.
    public LegacyGreetingServiceRefactored(IDateTimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public string GetGreeting()
    {
        var currentHour = _timeProvider.Now.Hour;
        
        if (currentHour < 12) return "Good Morning";
        if (currentHour < 18) return "Good Afternoon";
        return "Good Evening";
    }
}

// ==============================================================================
// TESTS
// ==============================================================================
public class LegacyCodeTest
{
    private class FakeMorningProvider : IDateTimeProvider
    {
        public DateTime Now => new DateTime(2023, 1, 1, 9, 0, 0); // 9 AM
    }

    [Fact]
    public void RefactoredService_ReturnsMorning_WhenTimeIs9AM()
    {
        var service = new LegacyGreetingServiceRefactored(new FakeMorningProvider());
        
        var result = service.GetGreeting();
        
        Assert.Equal("Good Morning", result);
    }
}
