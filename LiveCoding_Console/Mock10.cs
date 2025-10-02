namespace LiveCoding_Console.Mock10;

internal class Mock10
{
    public Mock10()
    {
        var logs = new List<AccessLog>
        {
            new AccessLog { LogId = 1, UserId = 1001, Timestamp = new DateTime(2023, 9, 1, 10, 0, 0) },
            new AccessLog { LogId = 2, UserId = 1001, Timestamp = new DateTime(2023, 9, 1, 10, 5, 0) },
            new AccessLog { LogId = 3, UserId = 1001, Timestamp = new DateTime(2023, 9, 1, 10, 9, 0) },
            new AccessLog { LogId = 4, UserId = 1001, Timestamp = new DateTime(2023, 9, 1, 10, 20, 0) },
            new AccessLog { LogId = 5, UserId = 1002, Timestamp = new DateTime(2023, 9, 1, 10, 0, 0) },
            new AccessLog { LogId = 6, UserId = 1002, Timestamp = new DateTime(2023, 9, 1, 10, 15, 0) }
        };

        List<AccessBurstGroup> groups = GetAccessBurstGroups(logs);
    }

    private List<AccessBurstGroup> GetAccessBurstGroups(List<AccessLog> logs)
    {
        if (logs == null || !logs.Any())
            return new List<AccessBurstGroup>();

        return (from log in logs
                group log by log.UserId into userGroup

                where userGroup.Count() >= 3

                let sorted = userGroup.OrderBy(x => x.Timestamp)

                //let burstIds = sorted.SelectMany((x, i) => sorted.Skip(i + 1).Where(y => (y.Timestamp - x.Timestamp).TotalMinutes <= 10)).Select(x => x.LogId).Distinct()

                let burstIds = sorted
                            .Select((x, i) => sorted
                                .Skip(i)
                                .TakeWhile(y => (y.Timestamp - x.Timestamp).TotalMinutes <= 10)
                                .Select(y => y.LogId))
                            .Where(currGroup => currGroup.Count() >= 3)
                            .SelectMany(currGroup => currGroup)
                            .Distinct()


                //where burstIds.Count() >= 3

                select new AccessBurstGroup
                {
                    UserId = userGroup.Key,
                    LogIds = userGroup.Where(x => burstIds.Contains(x.LogId)).Select(x => x.LogId).ToList()
                }).ToList();
    }
}

public class AccessLog
{
    public int LogId { get; set; }
    public int UserId { get; set; }
    public DateTime Timestamp { get; set; }
}

public class AccessBurstGroup
{
    public int UserId { get; set; }
    public List<int> LogIds { get; set; }
}
