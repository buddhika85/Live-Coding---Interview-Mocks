namespace LiveCoding_Console.Mock9;

internal class Mock9
{
    public Mock9()
    {
        var ticks = new List<PriceTick>
        {
            new PriceTick { TickId = 1, Symbol = "AAPL", Timestamp = new DateTime(2023, 9, 1, 10, 0, 0), Price = 150 },
            new PriceTick { TickId = 2, Symbol = "AAPL", Timestamp = new DateTime(2023, 9, 1, 10, 0, 30), Price = 158 },
            new PriceTick { TickId = 3, Symbol = "AAPL", Timestamp = new DateTime(2023, 9, 1, 10, 2, 0), Price = 160 },
            new PriceTick { TickId = 4, Symbol = "MSFT", Timestamp = new DateTime(2023, 9, 1, 10, 0, 0), Price = 300 },
            new PriceTick { TickId = 5, Symbol = "MSFT", Timestamp = new DateTime(2023, 9, 1, 10, 0, 50), Price = 303 }
        };

        List<RapidPriceChangeGroup> groups = GetRapidPriceChangeGroups(ticks);
    }

    private List<RapidPriceChangeGroup> GetRapidPriceChangeGroups(List<PriceTick> ticks)
    {
        if (ticks == null || !ticks.Any())
            return [];

        return (from tick in ticks
                group tick by tick.Symbol into symbolGroup

                let sorted = symbolGroup.OrderBy(x => x.Timestamp)

                let rapidIds = sorted.SelectMany((x, i) => sorted.Skip(i + 1)
                    .Where(y => (y.Timestamp - x.Timestamp).TotalMinutes <= 1 && (Math.Abs(y.Price - x.Price) / x.Price > 0.05m)))
                    .Select(x => x.TickId).Distinct()

                select new RapidPriceChangeGroup
                {
                    Symbol = symbolGroup.Key,
                    TickIds = symbolGroup.Where(x => rapidIds.Contains(x.TickId)).Select(x => x.TickId).ToList()
                }).ToList();
    }
}


public class PriceTick
{
    public int TickId { get; set; }
    public string Symbol { get; set; }
    public DateTime Timestamp { get; set; }
    public decimal Price { get; set; }
}

public class RapidPriceChangeGroup
{
    public string Symbol { get; set; }
    public List<int> TickIds { get; set; }
}
