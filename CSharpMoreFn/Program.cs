using System;
using System.Collections.Generic;
using CSharpMoreFn.Examples;
using CSharpMoreFn.Functional;

namespace CSharpMoreFn
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== CSharpMoreFn: Functional C# in Action ===");

            // 1. Pipe syntax
            var greeting = "hello functional c#"
                .Pipe(s => s.ToUpperInvariant())
                .Pipe(s => $"[Pipeline Output]: {s}");
            Console.WriteLine(greeting);

            // 2. Currying and function composition
            Func<int, int, int> add = (a, b) => a + b;
            var curriedAdd = add.Curry();
            var addTen = curriedAdd(10);
            Console.WriteLine($"Curried 10 + 5 = {addTen(5)}");

            // 3. Option & Result Railway processing
            var order = new Order("ORD-101", "CUST-42", new List<OrderItem>
            {
                new("SKU-1", "Clean Code Book", 45.00m, 1),
                new("SKU-2", "Refactoring", 50.00m, 1)
            }, "SAVE10");

            var result = OrderPipeline.Process(order);
            var summary = result.Match(
                invoice => $"Invoice generated: Subtotal={invoice.Subtotal:C}, Discount={invoice.Discount:C}, Tax={invoice.Tax:C}, Total={invoice.Total:C}",
                error => $"Processing error: {error}");
            Console.WriteLine(summary);
        }
    }
}
