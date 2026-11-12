using System;
using System.Collections.Generic;
using System.Linq;
using CSharpMoreFn.Functional;

namespace CSharpMoreFn.Examples
{
    public record OrderItem(string Sku, string Name, decimal Price, int Quantity);

    public record Order(string OrderId, string CustomerId, List<OrderItem> Items, string? PromoCode);

    public record Invoice(string OrderId, decimal Subtotal, decimal Discount, decimal Tax, decimal Total);

    public static class OrderPipeline
    {
        private static readonly Dictionary<string, decimal> PromoDiscounts = new(StringComparer.OrdinalIgnoreCase)
        {
            ["SAVE10"] = 0.10m,
            ["HALF"] = 0.50m
        };

        public static Result<Order, string> ValidateOrder(Order order)
        {
            if (string.IsNullOrWhiteSpace(order.OrderId))
                return Result<Order, string>.Fail("Order ID is required");

            if (string.IsNullOrWhiteSpace(order.CustomerId))
                return Result<Order, string>.Fail("Customer ID is required");

            if (order.Items == null || order.Items.Count == 0)
                return Result<Order, string>.Fail("Order must contain at least one item");

            if (order.Items.Any(i => i.Quantity <= 0 || i.Price < 0))
                return Result<Order, string>.Fail("Invalid line items found");

            return Result<Order, string>.Ok(order);
        }

        public static Option<decimal> LookupDiscount(string? promoCode)
        {
            if (string.IsNullOrWhiteSpace(promoCode))
                return Option<decimal>.None;

            return PromoDiscounts.TryGetValue(promoCode, out var rate)
                ? Option<decimal>.Some(rate)
                : Option<decimal>.None;
        }

        public static Invoice ProcessInvoice(Order order, decimal taxRate = 0.08m)
        {
            var subtotal = order.Items.Sum(i => i.Price * i.Quantity);
            var discountRate = LookupDiscount(order.PromoCode).GetOrElse(0m);
            var discount = subtotal * discountRate;
            var taxable = Math.Max(0m, subtotal - discount);
            var tax = Math.Round(taxable * taxRate, 2);
            var total = taxable + tax;

            return new Invoice(order.OrderId, subtotal, discount, tax, total);
        }

        public static Result<Invoice, string> Process(Order order, decimal taxRate = 0.08m)
        {
            return ValidateOrder(order)
                .Map(validOrder => ProcessInvoice(validOrder, taxRate));
        }
    }
}
