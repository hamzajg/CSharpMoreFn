using System.Collections.Generic;
using CSharpMoreFn.Examples;
using Xunit;

namespace CSharpMoreFn.Tests
{
    public class OrderPipelineTests
    {
        [Fact]
        public void Process_GeneratesValidInvoiceWithDiscount()
        {
            var order = new Order("ORD-1", "CUST-1", new List<OrderItem>
            {
                new("SKU-1", "Book", 50.00m, 2), // 100
                new("SKU-2", "Pen", 10.00m, 5)   // 50 => subtotal 150
            }, "SAVE10");

            var result = OrderPipeline.Process(order, 0.10m); // 10% tax
            Assert.True(result.IsSuccess);

            var invoice = result.ValueOr(_ => null!);
            Assert.Equal("ORD-1", invoice.OrderId);
            Assert.Equal(150.00m, invoice.Subtotal);
            Assert.Equal(15.00m, invoice.Discount); // 10% of 150
            Assert.Equal(13.50m, invoice.Tax);      // 10% of (150 - 15 = 135)
            Assert.Equal(148.50m, invoice.Total);
        }

        [Fact]
        public void Process_FailsOnEmptyItems()
        {
            var order = new Order("ORD-2", "CUST-2", new List<OrderItem>(), null);
            var result = OrderPipeline.Process(order);
            Assert.True(result.IsFailure);
            Assert.Equal("Order must contain at least one item", result.Match(_ => "", err => err));
        }

        [Fact]
        public void Process_FailsOnMissingOrderId()
        {
            var order = new Order("", "CUST-3", new List<OrderItem> { new("SKU", "Item", 10m, 1) }, null);
            var result = OrderPipeline.Process(order);
            Assert.True(result.IsFailure);
        }
    }
}
