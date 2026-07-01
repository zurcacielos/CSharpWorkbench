using System;
using Xunit;

namespace CSharpWorkbench.Tests.Refactoring;

// ==============================================================================
// EXERCISE: Refactor Error Handling to Use the Result Pattern
// ==============================================================================
// Problem:
// PaymentServiceBad uses Exceptions for standard control flow (validation failures).
//
// Smells:
// 1. Exceptions for Control Flow: Throwing an exception is very expensive in C# 
//    due to stack trace collection. It should be reserved for EXCEPTIONAL events 
//    (e.g., database goes offline), not expected business validations (e.g., "invalid email").
// 2. Dishonest API: The caller doesn't know the method throws `ValidationException` 
//    unless they read the source code.
//
// Refactoring applied:
// 1. Created a generic `Result<T>` pattern.
// 2. The method now returns a Result indicating Success or Failure, explicitly 
//    informing the caller what to expect without throwing exceptions.
// ==============================================================================

// 1. BAD IMPLEMENTATION
public class PaymentServiceBad
{
    public string ProcessPayment(decimal amount)
    {
        // SMELL: Throwing exceptions for standard business logic / validation
        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative");
        }
        
        return "Transaction_ABC123";
    }
}

// ==============================================================================
// REFACTORED SOLUTION
// ==============================================================================

// 1. The Result Wrapper
public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string ErrorMessage { get; }

    private Result(bool isSuccess, T value, string errorMessage)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage;
    }

    public static Result<T> Success(T value) => new Result<T>(true, value, null);
    public static Result<T> Failure(string errorMessage) => new Result<T>(false, default, errorMessage);
}

// 2. Refactored Service
public class PaymentServiceRefactored
{
    // WHY THIS IS BETTER:
    // 1. No expensive exceptions are thrown for expected validation flows.
    // 2. The signature explicitly tells the caller they must handle success/failure.
    public Result<string> ProcessPayment(decimal amount)
    {
        if (amount < 0)
        {
            return Result<string>.Failure("Amount cannot be negative");
        }
        
        return Result<string>.Success("Transaction_ABC123");
    }
}

// ==============================================================================
// TESTS
// ==============================================================================
public class ResultPatternTest
{
    [Fact]
    public void RefactoredService_ReturnsFailure_WithoutThrowing()
    {
        var service = new PaymentServiceRefactored();
        
        var result = service.ProcessPayment(-50);
        
        Assert.False(result.IsSuccess);
        Assert.Equal("Amount cannot be negative", result.ErrorMessage);
    }
}
