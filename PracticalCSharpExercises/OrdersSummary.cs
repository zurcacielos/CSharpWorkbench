using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpWorkbench.PracticalCSharpExercises
{
    public enum OrderStatus
    {
        Pending, Processing, Shipped, Delivered, Cancelled
    }

    public record Order(string Id, string CustomerId, decimal Amount, OrderStatus Status, DateTime OrderDate);

    public class OrdersSummaryResult
    {
        public decimal TotalRevenue { get; set; }
        public int CancelledOrdersCount { get; set; }
        public List<string> DuplicateOrderIds { get; set; } = new();

        public Dictionary<string, decimal> RevenueByCustomer { get; set; } = new();
        public Dictionary<OrderStatus, int> CountByStatus { get; set; } = new();
        public Dictionary<string, decimal> RevenueByMonth { get; set; } = new();

        public List<KeyValuePair<string, decimal>> TopCustomers { get; set; } = new();
    }

    public class OrdersSummary
    {
        public static OrdersSummaryResult SummarizeOrders(IEnumerable<Order> orders, int topCustomerCount = 3)
        {
            ArgumentNullException.ThrowIfNull(orders);

            var firstNegative = orders.FirstOrDefault(o => o.Amount < 0);
            if (firstNegative != null)
            {
                throw new ArgumentException($"Order {firstNegative.Id} has a negative amount.", nameof(orders));
            }

            var groupedById = orders.GroupBy(o => o.Id).ToList();
            var validOrders = groupedById.Select(g => g.First()).ToList();
            var duplicateOrderIds = groupedById.SelectMany(g => g.Skip(1)).Select(o => o.Id).ToList();

            var revenueOrders = validOrders.Where(o => o.Status != OrderStatus.Cancelled).ToList();

            var revenueByCustomer = revenueOrders.GroupBy(o => o.CustomerId)
            .ToDictionary(g => g.Key, g => g.Sum(o => o.Amount));

            return new OrdersSummaryResult
            {
                TotalRevenue = revenueOrders.Sum(o => o.Amount),
                CancelledOrdersCount = validOrders.Count(o => o.Status == OrderStatus.Cancelled),
                DuplicateOrderIds = duplicateOrderIds,
                RevenueByCustomer = revenueByCustomer,
                CountByStatus = validOrders.GroupBy(o => o.Status)
                .ToDictionary(g => g.Key, g => g.Count()),
                RevenueByMonth = revenueOrders
                .GroupBy(o => o.OrderDate.ToString("yyyy-MM"))
                .ToDictionary(g => g.Key, g => g.Sum(o => o.Amount)),
                TopCustomers = revenueByCustomer
                .OrderByDescending(kvp => kvp.Value)
                .Take(topCustomerCount)
                .ToList()
            };
        }
    }
}
