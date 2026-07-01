using System;
using Xunit;

namespace CSharpWorkbench.Tests.Refactoring;

// ==============================================================================
// EXERCISE: Extract Hardcoded Values into the Options Pattern
// ==============================================================================
// Problem:
// RetryServiceBad has hardcoded configuration values (RetryCount, Delay) directly 
// inside the class.
//
// Smells:
// 1. Magic Numbers / Hardcoded configuration: If we move this to production, 
//    we have to recompile the code just to change the retry count.
// 2. Untestable behavior: We can't easily test what happens if RetryCount is 0.
//
// Refactoring applied:
// 1. Introduced the standard ASP.NET Core IOptions<T> pattern (simulated).
// 2. Extracted the settings into a strongly typed POCO class.
// ==============================================================================

// 1. BAD IMPLEMENTATION
public class RetryServiceBad
{
    public void Execute()
    {
        // SMELL: Magic numbers tightly coupled to the code.
        int maxRetries = 3;
        int delayMs = 500;
        
        Console.WriteLine($"Will retry {maxRetries} times with {delayMs}ms delay.");
    }
}

// ==============================================================================
// REFACTORED SOLUTION
// ==============================================================================

// Simulating Microsoft.Extensions.Options.IOptions<T>
public interface IOptions<out T> where T : class
{
    T Value { get; }
}
public class Options<T> : IOptions<T> where T : class { public T Value { get; set; } }

// 1. Strongly typed configuration class
public class RetrySettings
{
    public int MaxRetries { get; set; }
    public int DelayMs { get; set; }
}

// 2. Refactored Service
public class RetryServiceRefactored
{
    private readonly RetrySettings _settings;

    // WHY THIS IS BETTER:
    // Configurations can now be injected from appsettings.json via ASP.NET Core DI.
    // In unit tests, we can inject any configuration we want without modifying code.
    public RetryServiceRefactored(IOptions<RetrySettings> options)
    {
        _settings = options.Value;
    }

    public void Execute()
    {
        Console.WriteLine($"Will retry {_settings.MaxRetries} times with {_settings.DelayMs}ms delay.");
    }
}

// ==============================================================================
// TESTS
// ==============================================================================
public class OptionsPatternTest
{
    [Fact]
    public void RefactoredService_CanBeTested_WithCustomOptions()
    {
        // Arrange
        var mockOptions = new Options<RetrySettings> { 
            Value = new RetrySettings { MaxRetries = 10, DelayMs = 1000 }
        };
        var service = new RetryServiceRefactored(mockOptions);
        
        // Act & Assert (In a real scenario we'd assert the effect, here it just runs without throwing)
        service.Execute();
        Assert.NotNull(service);
    }
}
