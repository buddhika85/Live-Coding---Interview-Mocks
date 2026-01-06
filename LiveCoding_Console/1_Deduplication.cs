using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace LiveCoding_Console
{
    public class _1_Deduplication
    {
        public static void Test()
        {
            List<User> users = [ new User { Email = "a@a.com" }, 
                new User { Email = "b@b.com"}, 
                new User { Email = "a@a.com" }];

            WriteLine(users[0].Equals(users[2]));   // true
            WriteLine(users[0].Equals(users[1]));   // false


            List<UserRecord> userRecords = [new UserRecord(1, "a@a.com", "a"), 
                new UserRecord(2, "b@b.com", "bb"), 
                new UserRecord(1, "a@a.com", "a")];

            WriteLine(userRecords[0].Equals(userRecords[2]));   // true
            WriteLine(userRecords[0].Equals(userRecords[1]));   // false
        }

    }

    public class User
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public string Name { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (obj is not User otherUser)
                return false;

            return StringComparer.OrdinalIgnoreCase.Equals(Email, otherUser.Email);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Email);
        }
    }


    // uing records - already built in value based comparison on all fields
    public record UserRecord(int Id, string Email, string Name);

}
