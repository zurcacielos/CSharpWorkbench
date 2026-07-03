using System;
using System.Threading.Tasks;
using Xunit;



// ==============================================================================
// EXERCISE: Encapsulate Business Logic into a Rich Domain Model
// ==============================================================================
// Problem:
// ProcessOrder uses an "Anemic Domain Model" and is a "God Class". It mixes 
// data access, business rules, and side effects. The Order object is just a 
// dumb data bag manipulated by the service.
//
// Smells:
// 1. Anemic Domain Model: The entity (Order) has no behavior, only public setters.
// 2. God Class / Feature Envy: The service knows too much about the order's internals
//    and applies logic that should belong to the domain.
// 3. SRP Violation: Mixing DB access, business logic, and email sending.
//
// Refactoring applied:
// 1. Encapsulated the Order's state (made setters private/init-only).
// 2. Added behavior to the Order class via `ApplyDecision`.
// 3. Extracted business rules into an `ApprovalPolicy`.
// 4. Delegated DB access to a repository and emails to a notification service.
// ==============================================================================

namespace CSharpWorkbench.Tests.Refactoring.RichDomain.Bad 
{
    // --- Infrastructure Interfaces for Simulation ---
    public interface IDbContext
    {
        Task<Order> FindOrderAsync(int id);
        Task SaveChangesAsync();
    }

    public interface IEmailSender
    {
        Task SendAsync(string email, string message);
    }

    // 1. BAD IMPLEMENTATION (Anemic Model & God Class)
    public class Order
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public string CustomerEmail { get; set; }
    }

    public class OrderProcessor
    {
        private readonly IDbContext _db;
        private readonly IEmailSender _emailSender;

        public OrderProcessor(IDbContext db, IEmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
        }

        public async Task ProcessOrder(int orderId)
        {
            var order = await _db.FindOrderAsync(orderId);

            // SMELL: Business logic leaked into the service.
            // SMELL: Modifying entity state directly (Anemic Model)
            if (order.Total > 1000)
                order.Status = "NeedsApproval";
            else
                order.Status = "Approved";

            await _db.SaveChangesAsync();

            await _emailSender.SendAsync(order.CustomerEmail, "Order updated");
        }
    }
}

namespace CSharpWorkbench.Tests.Refactoring.RichDomain.Refactored 
{
    // New Enums & Records for Domain Language
    public enum OrderStatus
    {
        Pending,
        Approved,
        NeedsApproval
    }

    public record ApprovalDecision(OrderStatus Status, string Reason);

    // 1. Rich Domain Model
    public class Order
    {
        public int Id { get; init; }
        public decimal Total { get; init; }
        public string CustomerEmail { get; init; }
        
        // Encapsulated state: Status can only be changed via methods
        public OrderStatus Status { get; private set; }

        public Order(int id, decimal total, string customerEmail)
        {
            Id = id;
            Total = total;
            CustomerEmail = customerEmail;
            Status = OrderStatus.Pending;
        }

        // Behavior inside the entity
        public void ApplyDecision(ApprovalDecision decision)
        {
            Status = decision.Status;
        }
    }

    // 2. Extracted Policy (Business Rules)
    public interface IApprovalPolicy
    {
        ApprovalDecision Decide(Order order);
    }

    public class StandardApprovalPolicy : IApprovalPolicy
    {
        public ApprovalDecision Decide(Order order)
        {
            if (order.Total > 1000)
                return new ApprovalDecision(OrderStatus.NeedsApproval, "Order exceeds automatic approval limit");
            
            return new ApprovalDecision(OrderStatus.Approved, "Automatic approval");
        }
    }

    // 3. Infrastructure Abstractions
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int id);
        Task SaveAsync(Order order);
    }

    public interface INotificationService
    {
        Task NotifyOrderUpdated(Order order);
    }

    // 4. Refactored Service (Coordinator, not God Class)
    public class OrderProcessor
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IApprovalPolicy _approvalPolicy;
        private readonly INotificationService _notificationService;

        public OrderProcessor(
            IOrderRepository orderRepository,
            IApprovalPolicy approvalPolicy,
            INotificationService notificationService)
        {
            _orderRepository = orderRepository;
            _approvalPolicy = approvalPolicy;
            _notificationService = notificationService;
        }

        public async Task ProcessOrderAsync(int orderId)
        {
            // 1. Get State
            var order = await _orderRepository.GetByIdAsync(orderId);

            // 2. Calculate Business Rules (Pure Logic)
            var decision = _approvalPolicy.Decide(order);

            // 3. Mutate State (Inside Entity)
            order.ApplyDecision(decision);

            // 4. Save and Notify (Side Effects)
            await _orderRepository.SaveAsync(order);
            await _notificationService.NotifyOrderUpdated(order);
        }
    }
}

// ==============================================================================
// TESTS
// ==============================================================================
namespace CSharpWorkbench.Tests.Refactoring.RichDomain { public class RichDomainModelTest
{
    // Fake Repository for testing
    private class FakeOrderRepository : Refactored.IOrderRepository
    {
        public Refactored.Order SavedOrder { get; private set; }
        public Task<Refactored.Order> GetByIdAsync(int id) 
            => Task.FromResult(new Refactored.Order(id, 1500m, "test@example.com"));

        public Task SaveAsync(Refactored.Order order)
        {
            SavedOrder = order;
            return Task.CompletedTask;
        }
    }

    // Fake Notification for testing
    private class FakeNotificationService : Refactored.INotificationService
    {
        public bool WasNotified { get; private set; }
        public Task NotifyOrderUpdated(Refactored.Order order)
        {
            WasNotified = true;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task ProcessOrderAsync_HighTotal_NeedsApproval()
    {
        // Arrange
        var repo = new FakeOrderRepository();
        var policy = new Refactored.StandardApprovalPolicy(); // Testing the real policy
        var notifications = new FakeNotificationService();
        var processor = new Refactored.OrderProcessor(repo, policy, notifications);

        // Act
        await processor.ProcessOrderAsync(1); // Fake returns order with 1500 total

        // Assert
        Assert.NotNull(repo.SavedOrder);
        Assert.Equal(Refactored.OrderStatus.NeedsApproval, repo.SavedOrder.Status);
        Assert.True(notifications.WasNotified);
    }

    [Fact]
    public void Order_ApplyDecision_UpdatesStatus()
    {
        // Arrange
        var order = new Refactored.Order(1, 500, "test@example.com");
        var decision = new Refactored.ApprovalDecision(Refactored.OrderStatus.Approved, "Ok");

        // Act
        order.ApplyDecision(decision);

        // Assert
        Assert.Equal(Refactored.OrderStatus.Approved, order.Status);
    }
}

}

