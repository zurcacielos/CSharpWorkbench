using System;
using Xunit;

namespace CSharpWorkbench.Tests.Refactoring;

// ==============================================================================
// EXERCISE: Decompose a "God Class" (Single Responsibility Principle)
// ==============================================================================
// Problem:
// UserRegistrationServiceBad is a "God Class". It handles user validation, 
// database persistence, and email notification all in one place.
//
// Smells:
// 1. Violation of SRP (Single Responsibility Principle): The class has multiple 
//    reasons to change (e.g., if validation rules change, if the DB changes, 
//    or if the email template changes).
// 2. High Coupling & Low Cohesion: It does too many unrelated things.
// 3. Testing Difficulty: Testing this class requires mocking out the DB and the 
//    email client just to test validation logic.
//
// Refactoring applied:
// 1. Extracted validation logic into an IValidator<User>.
// 2. Extracted persistence into an IUserRepository.
// 3. Extracted notifications into an IEmailService.
// 4. UserRegistrationServiceRefactored now acts as a coordinator, delegating work.
// ==============================================================================

public class User
{
    public string Email { get; set; }
    public string Password { get; set; }
}

// 1. BAD IMPLEMENTATION
public class UserRegistrationServiceBad
{
    public bool RegisterUser(User user)
    {
        // 1. Validation Logic
        if (string.IsNullOrEmpty(user.Email) || !user.Email.Contains("@"))
            return false;
        
        if (string.IsNullOrEmpty(user.Password) || user.Password.Length < 6)
            return false;

        // 2. Database Logic (Simulated)
        Console.WriteLine($"[DB] Saving {user.Email} to database...");
        
        // 3. Email Logic (Simulated)
        Console.WriteLine($"[Email] Sending welcome email to {user.Email}...");

        return true;
    }
}

// ==============================================================================
// REFACTORED SOLUTION
// ==============================================================================

public interface IUserValidator
{
    bool IsValid(User user);
}

public interface IUserRepository
{
    void Save(User user);
}

public interface IEmailService
{
    void SendWelcomeEmail(string emailAddress);
}

// Concrete Implementations (for DI container)
// WHY THIS IS BETTER (Single Responsibility Principle):
// By extracting the validation logic into its own class, this class now has exactly 
// ONE reason to change (if business rules for validation change). It knows nothing 
// about databases or emails, and we can unit test it in total isolation.
public class UserValidator : IUserValidator
{
    public bool IsValid(User user)
    {
        if (string.IsNullOrEmpty(user.Email) || !user.Email.Contains("@")) return false;
        if (string.IsNullOrEmpty(user.Password) || user.Password.Length < 6) return false;
        return true;
    }
}

public class UserRegistrationServiceRefactored
{
    private readonly IUserValidator _validator;
    private readonly IUserRepository _repository;
    private readonly IEmailService _emailService;

    // WHY THIS IS BETTER:
    // This class is now a pure coordinator. If validation rules change, we don't 
    // touch this class. We can test validation completely isolated from the DB.
    public UserRegistrationServiceRefactored(
        IUserValidator validator, 
        IUserRepository repository, 
        IEmailService emailService)
    {
        _validator = validator;
        _repository = repository;
        _emailService = emailService;
    }

    public bool RegisterUser(User user)
    {
        if (!_validator.IsValid(user))
            return false;

        _repository.Save(user);
        _emailService.SendWelcomeEmail(user.Email);

        return true;
    }
}

// ==============================================================================
// TESTS
// ==============================================================================
public class GodClassTest
{
    private class DummyValidator : IUserValidator { public bool IsValid(User u) => true; }
    private class DummyRepo : IUserRepository { public void Save(User u) { } }
    private class DummyEmail : IEmailService { public void SendWelcomeEmail(string e) { } }

    [Fact]
    public void RefactoredService_DelegatesCorrectly()
    {
        var service = new UserRegistrationServiceRefactored(
            new DummyValidator(), new DummyRepo(), new DummyEmail());

        var result = service.RegisterUser(new User { Email = "test@test.com" });
        Assert.True(result);
    }
}
