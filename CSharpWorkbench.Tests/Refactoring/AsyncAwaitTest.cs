using System;
using System.Threading.Tasks;
using Xunit;

namespace CSharpWorkbench.Tests.Refactoring;

// ==============================================================================
// EXERCISE: Refactor Synchronous Code to Async/Await
// ==============================================================================
// Problem:
// DataFetcherBad needs to fetch data from an external asynchronous API, but it 
// exposes a synchronous method and uses `.Result` to wait for the task. 
// This triggers several industry code smells and anti-patterns:
//
// 1. "Sync-over-Async" Anti-pattern: 
//    Blocking on async code by calling `.Result` or `.Wait()` is extremely dangerous. 
// 2. Deadlocks:
//    In environments with a SynchronizationContext (like legacy ASP.NET or UI apps 
//    like WPF/WinForms), calling `.Result` will cause a classic deadlock. The task 
//    tries to resume on the original thread, but the original thread is blocked 
//    waiting for the task to complete.
// 3. Thread Pool Starvation:
//    Instead of releasing the thread back to the pool while waiting for the I/O 
//    network call to finish, the thread sits idle and blocked. Under high load, 
//    the server will run out of threads and crash or become completely unresponsive.
//
// Refactoring applied:
// 1. Adopted "Async All The Way Down" principle.
// 2. Changed method signatures to return `Task<T>` instead of `T`.
// 3. Replaced blocking `.Result` calls with `await`.
// ==============================================================================

// A mock external API client
public class ExternalApiClient
{
    public async Task<string> FetchUserDataAsync(int userId)
    {
        // Simulate a network delay of 50ms (I/O bound work)
        await Task.Delay(50);
        return $"UserData_For_{userId}";
    }
}

// 1. BAD IMPLEMENTATION (Sync-over-Async)
public class DataFetcherBad
{
    private readonly ExternalApiClient _apiClient = new ExternalApiClient();

    public string GetUserData(int userId)
    {
        // SMELL: Sync-over-Async.
        // Calling .Result blocks the calling thread until the Task completes.
        // This causes thread starvation and potential deadlocks.
        string data = _apiClient.FetchUserDataAsync(userId).Result;
        
        return data;
    }
}

// ==============================================================================
// REFACTORED SOLUTION
// ==============================================================================

public interface IExternalApiClient
{
    Task<string> FetchUserDataAsync(int userId);
}

// We use an Adapter here because we pretend ExternalApiClient is a 3rd party 
// class that we are not allowed to modify (e.g., to add an interface to it).
// NOTE: This adapter is intended for the PRODUCTION code to instantiate DataFetcherRefactored, 
// since it now strictly requires an injected IExternalApiClient dependency.
public class ExternalApiClientAdapter : IExternalApiClient
{
    private readonly ExternalApiClient _client = new ExternalApiClient();
    
    public Task<string> FetchUserDataAsync(int userId)
    {
        return _client.FetchUserDataAsync(userId);
    }
}

public class DataFetcherRefactored
{
    private readonly IExternalApiClient _apiClient;

    public DataFetcherRefactored(IExternalApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    // WHY THIS IS BETTER:
    // 1. The method signature is now explicitly async, returning a Task<string>.
    // 2. We use 'await', which frees up the calling thread to do other work 
    //    (like serving other HTTP requests) while waiting for the I/O operation.
    // 3. The risk of deadlocks is completely eliminated.
    public async Task<string> GetUserDataAsync(int userId)
    {
        // We 'await' the task instead of blocking with .Result
        string data = await _apiClient.FetchUserDataAsync(userId);
        
        return data;
    }
}

// ==============================================================================
// TESTS
// ==============================================================================
public class ExternalApiClientMock : IExternalApiClient
{
    public Task<string> FetchUserDataAsync(int userId)
    {
        // Mock returns immediately without any I/O delay simulation
        return Task.FromResult($"MockData_For_{userId}");
    }
}

public class AsyncAwaitTest
{
    [Fact]
    public void BadFetcher_ReturnsData_ButBlocksThread()
    {
        var fetcher = new DataFetcherBad();
        
        // This will work in xUnit (because xUnit doesn't have a UI SynchronizationContext 
        // that causes deadlocks), but it is a terrible practice in production code!
        var result = fetcher.GetUserData(100);
        
        Assert.Equal("UserData_For_100", result);
    }

    [Fact]
    public async Task RefactoredFetcher_ReturnsData_WithoutBlocking()
    {
        var fetcher = new DataFetcherRefactored(new ExternalApiClientMock());
        
        // WHY THIS IS BETTER:
        // Notice that the test method itself must be marked as `async Task`.
        // 'Async all the way down' spreads to the caller, ensuring no threads are blocked.
        var result = await fetcher.GetUserDataAsync(200);
        
        Assert.Equal("MockData_For_200", result);
    }
}
