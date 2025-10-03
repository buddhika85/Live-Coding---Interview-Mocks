namespace LiveCoding_Console.Mock11;

internal class Mock11
{

    public Mock11()
    {
        var orders = new List<Order>
        {
            new Order { OrderId = 1, CustomerId = 101, Region = "NSW", IsActive = true, OrderDate = new DateTime(2023, 9, 1) },
            new Order { OrderId = 2, CustomerId = 102, Region = "NSW", IsActive = true, OrderDate = new DateTime(2023, 9, 5) },
            new Order { OrderId = 3, CustomerId = 103, Region = "NSW", IsActive = false, OrderDate = new DateTime(2023, 9, 10) },
            new Order { OrderId = 4, CustomerId = 104, Region = "VIC", IsActive = true, OrderDate = new DateTime(2023, 9, 2) },
            new Order { OrderId = 5, CustomerId = 105, Region = "QLD", IsActive = true, OrderDate = new DateTime(2023, 9, 3) }
        };

        List<RegionOrderSummary> summaries = await GetListOfSummaries(orders);
    }

    private async Task<List<RegionOrderSummary>> GetListOfSummaries(List<Order> orders)
    {
        if (orders == null || !orders.Any())
            return new List<RegionOrderSummary>();

        return await Task.Run(() =>
        {
            return (from order in orders
                    where order.IsActive

                    group order by order.Region into regionGroup

                    let activeCount = regionGroup.Count(x => x.IsActive)

                    where activeCount >= 2

                    let orderCount = regionGroup.Count()
                    let earliestOrderDate = regionGroup.Min(x => x.OrderDate)
                    let orderIds = regionGroup.OrderByDescending(x => x.OrderDate).Select(x => x.OrderId).ToList()

                    orderby regionGroup.Key, orderIds.Count() descending

                    select new RegionOrderSummary
                    {
                        Region = regionGroup.Key,
                        TotalOrders = orderCount,
                        EarliestOrderDate = earliestOrderDate,
                        OrderIds = orderIds
                    }).ToList();
        });
    }
}


public class Order
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public string Region { get; set; }
    public bool IsActive { get; set; }
    public DateTime OrderDate { get; set; }
}

public class RegionOrderSummary
{
    public string Region { get; set; }
    public int TotalOrders { get; set; }
    public DateTime EarliestOrderDate { get; set; }
    public List<int> OrderIds { get; set; }
}
