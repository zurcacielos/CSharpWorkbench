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

public enum PaymentError
{
    None = 0,
    AmountCannotBeNegative = 1
}

// 1. The Result Wrapper
public class Result<TValue, TError>
{
    public bool IsSuccess { get; }
    public TValue Value { get; }
    public TError Error { get; }

    private Result(bool isSuccess, TValue value, TError error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<TValue, TError> Success(TValue value) => new Result<TValue, TError>(true, value, default);
    public static Result<TValue, TError> Failure(TError error) => new Result<TValue, TError>(false, default, error);
}

// 2. Refactored Service
public class PaymentServiceRefactored
{
    // WHY THIS IS BETTER:
    // 1. No expensive exceptions are thrown for expected validation flows.
    // 2. The signature explicitly tells the caller they must handle success/failure.
    public Result<string, PaymentError> ProcessPayment(decimal amount)
    {
        if (amount < 0)
        {
            return Result<string, PaymentError>.Failure(PaymentError.AmountCannotBeNegative);
        }
        
        return Result<string, PaymentError>.Success("Transaction_ABC123");
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
        Assert.Equal(PaymentError.AmountCannotBeNegative, result.Error);
    }
}
