namespace LiveCoding_Console.Mock3;

internal class Mock3
{
    public Mock3()
    {
        var invoices = new List<Invoice>
        {
            new Invoice { InvoiceId = 1, CustomerName = "Acme", Amount = 500.00m, Date = new DateTime(2023, 9, 1) },
            new Invoice { InvoiceId = 2, CustomerName = "Acme", Amount = 500.00m, Date = new DateTime(2023, 9, 5) },
            new Invoice { InvoiceId = 3, CustomerName = "Acme", Amount = 500.00m, Date = new DateTime(2023, 9, 8) },
            new Invoice { InvoiceId = 3, CustomerName = "Acme", Amount = 500.00m, Date = new DateTime(2023, 9, 21) },
            new Invoice { InvoiceId = 4, CustomerName = "BetaCorp", Amount = 300.00m, Date = new DateTime(2023, 9, 2) },
            new Invoice { InvoiceId = 5, CustomerName = "BetaCorp", Amount = 300.00m, Date = new DateTime(2023, 9, 8) }
        };
        var duplicates = FilterDuplicates(invoices);
        DisplayDuplicates(duplicates);

        Console.WriteLine("\n\n");
        duplicates = FilterDuplicatesFixed(invoices);
        DisplayDuplicates(duplicates);
    }

    private static void DisplayDuplicates(List<DuplicateInvoiceGroup> duplicates)
    {
        foreach (var item in duplicates)
        {
            Console.WriteLine(item);
        }
    }

    // Write a method that returns a list of potential duplicates—defined as invoices with the same CustomerName and Amount, occurring within 7 days of each other.

    private List<DuplicateInvoiceGroup> FilterDuplicates(List<Invoice> invoices)
    {
        if (invoices == null || !invoices.Any())
            return new List<DuplicateInvoiceGroup>();

        return (from inv in invoices
                group inv by new { inv.CustomerName, inv.Amount } into customerGroup

                let customerName = customerGroup.Key.CustomerName ?? "Unknown"

                let allInvOfCustomer = customerGroup.OrderBy(x => x.Date)
                let firstInvoice = allInvOfCustomer.FirstOrDefault()
                let firstInvoiceDate = firstInvoice.Date
                let duplicatedInvoices = allInvOfCustomer.Where(x => (x.Date - firstInvoiceDate).TotalDays <= 7)

                let amount = Math.Round(duplicatedInvoices.First().Amount, 2)
                let invoiceIds = duplicatedInvoices.Select(x => x.InvoiceId).ToList()

                orderby duplicatedInvoices.Count() descending, amount descending, customerName

                select new DuplicateInvoiceGroup
                {
                    CustomerName = customerName,
                    Amount = amount,
                    InvoiceIds = invoiceIds
                }).ToList();
    }


    private List<DuplicateInvoiceGroup> FilterDuplicatesFixed(List<Invoice> invoices)
    {
        if (invoices == null || !invoices.Any())
            return new List<DuplicateInvoiceGroup>();

        return (from inv in invoices
                group inv by new { inv.CustomerName, inv.Amount } into customerGroup
                let sorted = customerGroup.OrderBy(x => x.Date).ToList()
                let closeInvoices = sorted
                    .SelectMany((x, i) => sorted.Skip(i + 1)
                        .Where(y => (y.Date - x.Date).TotalDays <= 7))
                    .Select(x => x.InvoiceId)
                    .Distinct()
                where closeInvoices.Any()
                select new DuplicateInvoiceGroup
                {
                    CustomerName = customerGroup.Key.CustomerName,
                    Amount = customerGroup.Key.Amount,
                    InvoiceIds = customerGroup
                        .Where(x => closeInvoices.Contains(x.InvoiceId))
                        .Select(x => x.InvoiceId)
                        .ToList()
                }).ToList();
    }
}


public class Invoice
{
    public int InvoiceId { get; set; }
    public string CustomerName { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}

public class DuplicateInvoiceGroup
{
    public string CustomerName { get; set; }
    public decimal Amount { get; set; }
    public List<int> InvoiceIds { get; set; }

    public override string ToString()
    {
        return $"{CustomerName}\t${Amount}\tInvoice IDs: {string.Join(',', InvoiceIds)}";
    }
}