namespace LiveCoding_Console.Mock2;

// 4:17 - 4:23
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

        var list = GetCustomersSuymmary(orders);
    }


    private List<CustomerSummary> GetCustomersSuymmary(List<Order> orders)
    {
        return (from order in orders
                group order by order.CustomerName ?? "Unknown" into orderGroup

                let totalSpent = Math.Round(orderGroup.Sum(x => x.Amount), 2)
                let orderCount = orderGroup.Count()
                let lastOrderDate = orderGroup.Max(x => x.Date)

                orderby totalSpent descending, orderCount descending, orderGroup.Key, lastOrderDate

                select new CustomerSummary
                {
                    Name = orderGroup.Key,
                    TotalSpent = totalSpent,
                    OrderCount = orderCount,
                    LastOrderDate = lastOrderDate
                }).ToList();
    }
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
}
