using System;
using System.Collections.Generic;
using System.Linq;
using CSharpWorkbench.PracticalCSharpExercises;
using Xunit;

namespace CodingChallenges.Tests.PracticalCSharpExercises
{
    public class OrdersSummaryTest
    {
        [Fact]
        public void SummarizeOrders_HappyPath_ShouldGroupAndSumCorrectly()
        {
            // Arrange
            var orders = new List<Order>
            {
                new("1", "CUST-A", 100m, OrderStatus.Delivered, new DateTime(2023, 10, 1)),
                new("2", "CUST-A", 50m, OrderStatus.Shipped, new DateTime(2023, 10, 5)),
                new("3", "CUST-B", 200m, OrderStatus.Pending, new DateTime(2023, 11, 10)),
                new("4", "CUST-C", 10m, OrderStatus.Cancelled, new DateTime(2023, 11, 12)) 
            };

            // Act
            var summary = OrdersSummary.SummarizeOrders(orders);

            // Assert
            Assert.Equal(350m, summary.TotalRevenue); 
            Assert.Equal(1, summary.CancelledOrdersCount);
            Assert.Empty(summary.DuplicateOrderIds);

            Assert.Equal(2, summary.RevenueByCustomer.Count);
            Assert.Equal(150m, summary.RevenueByCustomer["CUST-A"]);
            Assert.Equal(200m, summary.RevenueByCustomer["CUST-B"]);
            Assert.False(summary.RevenueByCustomer.ContainsKey("CUST-C")); 

            Assert.Equal(4, summary.CountByStatus.Count);
            Assert.Equal(1, summary.CountByStatus[OrderStatus.Delivered]);
            Assert.Equal(1, summary.CountByStatus[OrderStatus.Shipped]);
            Assert.Equal(1, summary.CountByStatus[OrderStatus.Pending]);
            Assert.Equal(1, summary.CountByStatus[OrderStatus.Cancelled]);

            Assert.Equal(2, summary.RevenueByMonth.Count);
            Assert.Equal(150m, summary.RevenueByMonth["2023-10"]);
            Assert.Equal(200m, summary.RevenueByMonth["2023-11"]);

            Assert.Equal(2, summary.TopCustomers.Count);
            Assert.Equal("CUST-B", summary.TopCustomers[0].Key); 
            Assert.Equal("CUST-A", summary.TopCustomers[1].Key); 
        }

        [Fact]
        public void SummarizeOrders_EmptyInput_ShouldReturnEmptyResults()
        {
            var orders = new List<Order>();
            var summary = OrdersSummary.SummarizeOrders(orders);
            Assert.Equal(0m, summary.TotalRevenue);
            Assert.Empty(summary.RevenueByCustomer);
            Assert.Empty(summary.CountByStatus);
        }

        [Fact]
        public void SummarizeOrders_NullInput_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => OrdersSummary.SummarizeOrders(null!));
        }

        [Fact]
        public void SummarizeOrders_InvalidInput_NegativeAmount_ShouldThrowArgumentException()
        {
            var orders = new List<Order> { new("1", "CUST-A", -100m, OrderStatus.Pending, DateTime.Now) };
            var ex = Assert.Throws<ArgumentException>(() => OrdersSummary.SummarizeOrders(orders));
            Assert.Contains("negative amount", ex.Message);
        }

        [Fact]
        public void SummarizeOrders_DuplicateInput_ShouldTrackAndIgnoreDuplicateIds()
        {
            var orders = new List<Order>
            {
                new("1", "CUST-A", 100m, OrderStatus.Delivered, DateTime.Now),
                new("1", "CUST-B", 50m, OrderStatus.Delivered, DateTime.Now), 
                new("2", "CUST-B", 200m, OrderStatus.Pending, DateTime.Now)
            };
            var summary = OrdersSummary.SummarizeOrders(orders);
            Assert.Equal(300m, summary.TotalRevenue); 
            Assert.Single(summary.DuplicateOrderIds);
            Assert.Contains("1", summary.DuplicateOrderIds);
        }

        [Fact]
        public void SummarizeOrders_DecimalPrecision_ShouldRetainPrecision()
        {
            var orders = new List<Order>
            {
                new("1", "CUST-A", 10.123456m, OrderStatus.Delivered, DateTime.Now),
                new("2", "CUST-A", 20.654321m, OrderStatus.Delivered, DateTime.Now)
            };
            var summary = OrdersSummary.SummarizeOrders(orders);
            Assert.Equal(30.777777m, summary.TotalRevenue);
        }
    }
}
