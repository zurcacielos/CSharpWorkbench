using System;
using Xunit;

namespace CSharpWorkbench.Tests.Refactoring;

// ==============================================================================
// EXERCISE: Move Business Logic out of Controllers to CQRS Handlers
// ==============================================================================
// Problem:
// UserControllerBad (a simulated API Controller) contains complex business logic 
// for creating a user. 
//
// Smells:
// 1. "Fat Controller": Controllers should only handle HTTP concerns (routing, 
//    model binding, status codes). Putting business logic here makes it untestable 
//    without spinning up an HTTP context.
// 2. SRP Violation: The controller knows how to create passwords and validate data.
//
// Refactoring applied:
// 1. Implemented the CQRS (Command Query Responsibility Segregation) pattern.
// 2. Created a 'CreateUserCommand' to hold the data.
// 3. Created a 'CreateUserCommandHandler' to hold the business logic.
// 4. The Controller now just maps the request to the command and sends it.
// ==============================================================================

// 1. BAD IMPLEMENTATION (Fat Controller)
public class UserControllerBad
{
    // Pretend this is an ASP.NET [HttpPost] endpoint
    public string RegisterUser(string username, string password)
    {
        // SMELL: Business logic lives in the controller!
        if (string.IsNullOrEmpty(username)) return "400 Bad Request";
        
        // Complex password hashing logic...
        var hashedPassword = $"HASHED_{password}_123";
        
        // Save to DB...
        Console.WriteLine($"Saved {username} with hash {hashedPassword}");
        
        return "200 OK";
    }
}

// ==============================================================================
// REFACTORED SOLUTION
// ==============================================================================

// 1. The Command (Data Transfer Object)
public class CreateUserCommand
{
    public string Username { get; set; }
    public string Password { get; set; }
}

// 2. The Handler (Business Logic)
public class CreateUserCommandHandler
{
    // WHY THIS IS BETTER:
    // This handler has ZERO knowledge of HTTP, Status Codes, or Controllers.
    // It can be unit tested perfectly in isolation. It only returns true/false (or a Result).
    public bool Handle(CreateUserCommand command)
    {
        if (string.IsNullOrEmpty(command.Username)) return false;
        
        var hashedPassword = $"HASHED_{command.Password}_123";
        Console.WriteLine($"Saved {command.Username} with hash {hashedPassword}");
        
        return true;
    }
}

// 3. The "Thin" Controller
public class UserControllerRefactored
{
    private readonly CreateUserCommandHandler _handler;

    public UserControllerRefactored(CreateUserCommandHandler handler)
    {
        _handler = handler;
    }

    public string RegisterUser(string username, string password)
    {
        var command = new CreateUserCommand { Username = username, Password = password };
        
        var success = _handler.Handle(command);
        
        return success ? "200 OK" : "400 Bad Request";
    }
}

// ==============================================================================
// TESTS
// ==============================================================================
public class CqrsHandlersTest
{
    [Fact]
    public void Handler_ContainsOnlyBusinessLogic()
    {
        var handler = new CreateUserCommandHandler();
        var command = new CreateUserCommand { Username = "Alice", Password = "pwd" };
        
        Assert.True(handler.Handle(command));
    }
}
