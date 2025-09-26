
namespace LiveCoding_Console.Mock5;

internal class Mock5
{

    public Mock5()
    {
        var transactions = new List<Transaction>
        {
            new Transaction { TransactionId = 1, AccountId = 1001, Amount = 999.99m, Timestamp = new DateTime(2023, 9, 1, 10, 0, 0) },
            new Transaction { TransactionId = 2, AccountId = 1001, Amount = 999.99m, Timestamp = new DateTime(2023, 9, 1, 10, 1, 30) },
            new Transaction { TransactionId = 3, AccountId = 1001, Amount = 999.99m, Timestamp = new DateTime(2023, 9, 1, 10, 5, 0) },
            new Transaction { TransactionId = 4, AccountId = 1002, Amount = 500.00m, Timestamp = new DateTime(2023, 9, 1, 11, 0, 0) },
            new Transaction { TransactionId = 5, AccountId = 1002, Amount = 500.00m, Timestamp = new DateTime(2023, 9, 1, 11, 3, 0) }
        };

        List<SuspiciousTransactionGroup> suspiciousTransactionGroups = FindSuspiciousTransactionGroups(transactions);

    }

    private List<SuspiciousTransactionGroup> FindSuspiciousTransactionGroups(List<Transaction> transactions)
    {
        if (transactions == null || !transactions.Any())
            return new List<SuspiciousTransactionGroup>();

        return (from tr in transactions
                group tr by new { tr.AccountId, tr.Amount } into accGroup

                let sorted = accGroup.OrderByDescending(tr => tr.Timestamp)

                let amount = accGroup.FirstOrDefault()?.Amount ?? 0

                // get suspicious transaction Ids with in group
                let suspiciousIds = sorted.SelectMany((x, i) => sorted.Skip(i + 1).Where(y => (y.Timestamp - x.Timestamp).TotalMinutes <= 2)).Select(x => x.TransactionId).Distinct()

                where suspiciousIds.Any()

                select new SuspiciousTransactionGroup
                {
                    AccountId = accGroup.Key.AccountId,
                    Amount = amount,
                    TransactionIds = accGroup.Where(x => suspiciousIds.Contains(x.TransactionId)).Select(x => x.TransactionId).ToList()
                }).ToList();
    }
}


public class Transaction
{
    public int TransactionId { get; set; }
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
}

public class SuspiciousTransactionGroup
{
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public List<int> TransactionIds { get; set; }
}
