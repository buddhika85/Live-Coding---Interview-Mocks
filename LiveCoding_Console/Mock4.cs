
namespace LiveCoding_Console.Mock4;

// 4:16
internal class Mock4
{
    public Mock4()
    {
        var bookings = new List<Booking>
        {
            new Booking { BookingId = 1, RoomId = 101, StartTime = new DateTime(2023, 9, 1, 9, 0, 0), EndTime = new DateTime(2023, 9, 1, 10, 0, 0) },
            new Booking { BookingId = 2, RoomId = 101, StartTime = new DateTime(2023, 9, 1, 9, 30, 0), EndTime = new DateTime(2023, 9, 1, 10, 30, 0) },
            new Booking { BookingId = 3, RoomId = 101, StartTime = new DateTime(2023, 9, 1, 11, 0, 0), EndTime = new DateTime(2023, 9, 1, 12, 0, 0) },
            new Booking { BookingId = 4, RoomId = 102, StartTime = new DateTime(2023, 9, 1, 9, 0, 0), EndTime = new DateTime(2023, 9, 1, 10, 0, 0) },
            new Booking { BookingId = 5, RoomId = 102, StartTime = new DateTime(2023, 9, 1, 10, 0, 0), EndTime = new DateTime(2023, 9, 1, 11, 0, 0) }
        };

        var result = FindOverlappingBookings(bookings);
    }


    private List<OverlappingBookingGroup> FindOverlappingBookings(List<Booking> bookings)
    {
        if (bookings == null || !bookings.Any())
            return new List<OverlappingBookingGroup>();

        return (from booking in bookings
                group booking by booking.RoomId into roomGroup

                let sorted = roomGroup.OrderBy(x => x.StartTime)

                let bookingIds = sorted.SelectMany((x, i) => sorted.Skip(i + 1).Where(y => x.StartTime < y.EndTime && x.EndTime > y.StartTime)).Select(x => x.BookingId).Distinct()

                where bookingIds.Any()

                select new OverlappingBookingGroup
                {
                    RoomId = roomGroup.Key,
                    BookingIds = roomGroup.Where(x => bookingIds.Contains(x.BookingId)).Select(x => x.BookingId).ToList()
                }).ToList();
    }
}


public class Booking
{
    public int BookingId { get; set; }
    public int RoomId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}

public class OverlappingBookingGroup
{
    public int RoomId { get; set; }
    public List<int> BookingIds { get; set; }
}
