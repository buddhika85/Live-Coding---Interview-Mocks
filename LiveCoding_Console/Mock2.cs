namespace LiveCoding_Console.Mock2;

// 4:17 - 4:23
// 4:30 - 4:34
// 4:37 - 4:46
internal class Mock2
{
    public Mock2()
    {
        var orders = new List<Order>
        {
            new Order { OrderId = 1, CustomerName = "Alice", Amount = 120.50m, Date = new DateTime(2023, 9, 1) },
            new Order { OrderId = 2, CustomerName = "Bob", Amount = 75.00m, Date = new DateTime(2023, 9, 2) },
            new Order { OrderId = 3, CustomerName = "Alice", Amount = 200.00m, Date = new DateTime(2023, 9, 5) },
            new Order { OrderId = 4, CustomerName = "Charlie", Amount = 50.00m, Date = new DateTime(2023, 9, 3) },
            new Order { OrderId = 5, CustomerName = "Bob", Amount = 125.00m, Date = new DateTime(2023, 9, 6) }
        };

        var list = GetRecentCustomerSummary(orders);
        var breakdown = GetMonthlyCustomerSpendBreakdown(orders);

    }


    private List<MonthlyCustomerSpend> GetMonthlyCustomerSpendBreakdown(List<Order> orders)
    {
        if (orders == null || !orders.Any()) return [];

        return (from order in orders
                group order by new { order.Date.Year, order.Date.Month, order.CustomerName } into orderGroup

                let customerName = orderGroup.Key.CustomerName ?? "Unknown Customer"
                let month = $"{orderGroup.Key.Year} - {orderGroup.Key.Month}"
                let total = Math.Round(orderGroup.Sum(x => x.Amount), 2)
                let orderCount = orderGroup.Count()


                orderby total descending, orderCount descending, customerName
                select new MonthlyCustomerSpend
                {
                    Month = month,
                    CustomerName = customerName,
                    TotalSpent = total,
                    OrderCount = orderCount
                }).ToList();
    }

    private List<CustomerSummary> GetRecentCustomerSummary(List<Order> orders)
    {
        if (orders == null || !orders.Any()) return new List<CustomerSummary>();

        return (from order in orders
                where (DateTime.Now - order.Date).Days <= 30
                group order by order.CustomerName ?? "Unknown" into orderGroup

                let totalSpent = Math.Round(orderGroup.Sum(x => x.Amount), 2)
                let orderCount = orderGroup.Count()
                let lastOrderDate = orderGroup.Max(x => x.Date)
                let averageSpendPerOrder = Math.Round(orderGroup.Average(x => x.Amount), 2)

                orderby totalSpent descending, averageSpendPerOrder descending, orderCount descending, orderGroup.Key, lastOrderDate

                select new CustomerSummary
                {
                    Name = orderGroup.Key,
                    TotalSpent = totalSpent,
                    OrderCount = orderCount,
                    LastOrderDate = lastOrderDate,
                    AverageSpendPerOrder = averageSpendPerOrder
                }).ToList();
    }
}

public class MonthlyCustomerSpend
{
    public string Month { get; set; }           // Format: "YYYY-MM"
    public string CustomerName { get; set; }
    public decimal TotalSpent { get; set; }
    public int OrderCount { get; set; }
}

public class Order
{
    public int OrderId { get; set; }
    public string? CustomerName { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}
public class CustomerSummary
{
    public string Name { get; set; }
    public decimal TotalSpent { get; set; }
    public int OrderCount { get; set; }
    public DateTime LastOrderDate { get; set; }
    public decimal AverageSpendPerOrder { get; set; }

}
