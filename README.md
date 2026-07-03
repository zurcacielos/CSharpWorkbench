# C# .NET Workbench

Small C#/.NET projects for backend services, API design, data handling, validation, testing, and maintainable application code.

## Coverage

| Type | Priority | Weight |
|---|---:|---:|
| Practical business C# exercises | High | 60% |
| C# fundamentals, OOP, LINQ, testing | High | 25% |
| Easy CS / data-structure exercises | Medium | 15% |

## Practical C# Exercises

### Orders Summary

Build a component that receives a collection of orders and returns totals grouped by customer, status, and month. Include total revenue, cancelled orders, duplicate order IDs, and top customers by amount.

### Invoice Validation

Build a validator for invoice records. Validate required fields, invoice number format, dates, line items, totals, tax values, and invalid negative amounts. Return all validation errors.

### CSV Import

Build an importer that reads CSV rows, parses them into records, separates valid and invalid rows, and returns import statistics. Invalid rows should include clear error messages.

### Customer Mapper

Build a mapper that converts customer domain objects into response DTOs. Include customer data, active status, last order date, total orders, and derived display fields.

### Records Merge

Build a merge routine that combines two collections by ID. Detect new records, updated records, unchanged records, and conflicting records.

### Pagination Helper

Build a reusable pagination helper. It should support page number, page size, total count, total pages, previous/next flags, and safe handling of invalid page values.

### Pricing Rules

Build a rules component that calculates final price from base price, customer type, discounts, taxes, and special conditions. Return the calculated value and the applied rules.

### Job Queue Processor

Build a processor for queued jobs. Process pending jobs, skip invalid jobs, retry failed jobs, and return a summary of processed, skipped, failed, and retried items.

### Search Filters

Build a filtering component for a collection of records. Support search text, status, date range, sorting, pagination, and optional filters.

### Audit Log Formatter

Build a formatter that converts audit events into readable log entries. Include timestamp, user, action, target entity, previous value, new value, and severity.

### Duplicate Email Detection

Build a detector that finds duplicate email addresses. Normalize whitespace and casing before comparison. Return grouped duplicates and the affected records.

### Workflow Status Transitions

Build a component that validates workflow status changes. Define allowed transitions, rejected transitions, and transition history entries.

### Date Range Validator

Build a validator for date ranges. Check missing dates, inverted ranges, maximum allowed duration, future/past restrictions, and overlapping ranges.

### Failed Message Retry

Build a retry component for failed messages. Track retry count, maximum retries, last error, next retry time, and final failure state.

### Report Aggregator

Build an aggregator that receives raw records and produces report data grouped by category, status, date, or owner. Include totals, counts, averages, and top results.


## C# Fundamentals

- LINQ GroupBy
- LINQ SelectMany
- LINQ Join
- Dictionary Lookup
- HashSet Lookup
- DTO Mapping
- Nullable Reference Types
- Records
- Interfaces
- Dependency Injection
- Async / Await
- Task.WhenAll
- CancellationToken
- Exception Handling
- ValidationResult
- xUnit Tests
- ASP.NET Core Minimal API
- Controller API
- Swagger / OpenAPI
- EF Core Query
- Application Logging

## Easy CS / Data Structures

- Two Sum
- Contains Duplicate
- Valid Parentheses
- First Non-Repeating Character
- Merge Sorted Arrays
- Group Anagrams
- Top K Frequent Elements
- Binary Search
- Remove Duplicates from Sorted Array
- Move Zeroes
- Reverse Words in a String
- Best Time to Buy and Sell Stock
- Moving Average from Data Stream
- Implement Queue using Stacks
- Implement Stack using Queues
- Min Stack
- Merge Intervals
- Valid Palindrome
- Intersection of Two Arrays
- Find Missing Number

## API Exercises

- GET Patients
- GET Patient by ID
- POST Appointment
- GET Claims by Status
- GET Claims with Paging
- PUT Status Update
- DELETE Draft Record
- Health Check Endpoint
- Request Validation
- Error Response Model

## Refactoring

- Extract Interface and Apply Dependency Injection (Break tight coupling)
- Replace Complex Conditional logic with Strategy Pattern / Polymorphism
- Refactor Synchronous Code to Async/Await (Fix blocking calls & deadlocks)
- Decompose a "God Class" / Massive Controller (Single Responsibility Principle)
- Fix N+1 Query Problem in Entity Framework (Optimize data access)
- Move Business Logic from Controllers to Services or CQRS Handlers
- Refactor Error Handling to Use the Result Pattern or Global Middleware
- Extract Hardcoded Values into the Options Pattern (IOptions<T>)
- Add Seams and Unit Tests to Legacy, Untested Code
- Introduce Caching (e.g., Redis) and Pagination to a Slow Endpoint
- Encapsulate Business Logic into a Rich Domain Model (Fix Anemic Domain Model / God Class)

## Entity Framework
- Fix N+1 Query Problem in Entity Framework (Optimize data access)

## xUnit Pitfalls

- public async void MyTest()

## Testing Checklist

- Happy path
- Empty input
- Null input
- Invalid input
- Duplicate input
- Boundary dates
- Case-insensitive comparison
- Decimal precision
- Pagination limits
- Failed dependency

## Build Commands

```bash
dotnet build
dotnet test
dotnet run --project src/Workbench.Api
```

## Advanced Topics & Roadmap

- **Mocking & External Dependencies**: Implement fake repositories and API clients using `Moq` or `NSubstitute`. Build services simulating concurrent external calls with `Task.WhenAll`, handling timeouts and partial failures.
- **Architecture & Decision Logging**: Maintain decision logs for data structure choices (e.g., `HashSet` vs distributed caching like Redis). Emphasize cloud-native design patterns (Serverless Functions, Managed SQL).
- **Frontend & NoSQL Integration**: Expand API layer to support React clients and GraphQL endpoints. Explore document storage with MongoDB or Elasticsearch alongside relational databases.
