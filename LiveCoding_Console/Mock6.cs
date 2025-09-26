namespace LiveCoding_Console.Mock6;

internal class Mock6
{

    public Mock6()
    {
        var sessions = new List<LoginSession>
        {
            new LoginSession { SessionId = 1, Username = "alice", LoginTime = new DateTime(2023, 9, 1, 9, 0, 0), LogoutTime = new DateTime(2023, 9, 1, 10, 0, 0) },
            new LoginSession { SessionId = 2, Username = "alice", LoginTime = new DateTime(2023, 9, 1, 9, 30, 0), LogoutTime = new DateTime(2023, 9, 1, 10, 30, 0) },
            new LoginSession { SessionId = 3, Username = "alice", LoginTime = new DateTime(2023, 9, 1, 11, 0, 0), LogoutTime = new DateTime(2023, 9, 1, 12, 0, 0) },
            new LoginSession { SessionId = 4, Username = "bob", LoginTime = new DateTime(2023, 9, 1, 9, 0, 0), LogoutTime = new DateTime(2023, 9, 1, 10, 0, 0) },
            new LoginSession { SessionId = 5, Username = "bob", LoginTime = new DateTime(2023, 9, 1, 10, 0, 0), LogoutTime = new DateTime(2023, 9, 1, 11, 0, 0) }
        };

        List<ConcurrentLoginGroup> concurrentLogins = GetConcurrentLogins(sessions);
    }

    private List<ConcurrentLoginGroup> GetConcurrentLogins(List<LoginSession> sessions)
    {
        if (sessions == null || !sessions.Any())
            return [];

        return (from session in sessions
                group session by session.Username into userGroup

                let sorted = userGroup.OrderBy(x => x.LoginTime)

                let concurrentSessionIds = sorted.SelectMany((x, i) => sorted.Skip(i + 1).Where(y => x.LoginTime < y.LoginTime && x.LogoutTime > y.LogoutTime)).Select(x => x.SessionId).Distinct()

                select new ConcurrentLoginGroup
                {
                    Username = userGroup.Key,
                    SessionIds = sorted.Where(x => concurrentSessionIds.Contains(x.SessionId)).Select(x => x.SessionId).ToList(),
                }).ToList();
    }
}


public class LoginSession
{
    public int SessionId { get; set; }
    public string? Username { get; set; }
    public DateTime LoginTime { get; set; }
    public DateTime LogoutTime { get; set; }
}

public class ConcurrentLoginGroup
{
    public string? Username { get; set; }
    public List<int>? SessionIds { get; set; }
}