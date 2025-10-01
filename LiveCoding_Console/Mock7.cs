namespace LiveCoding_Console;

internal class Mock7
{
    public Mock7()
    {
        var deliveries = new List<Delivery>
        {
            new Delivery { DeliveryId = 1, DriverName = "Sam", Latitude = -33.870, Longitude = 151.200, Timestamp = new DateTime(2023, 9, 1, 10, 0, 0) },
            new Delivery { DeliveryId = 2, DriverName = "Sam", Latitude = -33.871, Longitude = 151.201, Timestamp = new DateTime(2023, 9, 1, 10, 3, 0) },
            new Delivery { DeliveryId = 3, DriverName = "Sam", Latitude = -33.880, Longitude = 151.210, Timestamp = new DateTime(2023, 9, 1, 10, 10, 0) },
            new Delivery { DeliveryId = 4, DriverName = "Alex", Latitude = -33.870, Longitude = 151.200, Timestamp = new DateTime(2023, 9, 1, 10, 0, 0) },
            new Delivery { DeliveryId = 5, DriverName = "Alex", Latitude = -33.870, Longitude = 151.200, Timestamp = new DateTime(2023, 9, 1, 10, 6, 0) }
        };

        var result = GetNearByDeliveryGroups(deliveries);

    }

    private List<NearbyDeliveryGroup> GetNearByDeliveryGroups(List<Delivery> deliveries)
    {
        if (!deliveries.Any())
            return new List<NearbyDeliveryGroup>();


        return (from del in deliveries
                group del by del.DriverName into delGroup

                let sorted = delGroup.OrderBy(x => x.Timestamp)

                let nearByIds = sorted.SelectMany((x, i) => sorted.Skip(i + 1)
                                        .Where(y =>
                                            (y.Timestamp - x.Timestamp).TotalMinutes <= 5 &&
                                            (GetDistanceKm(y.Latitude, y.Longitude, x.Latitude, x.Longitude) <= 0.5))).Select(x => x.DeliveryId).Distinct().ToList()

                select new NearbyDeliveryGroup
                {
                    DriverName = delGroup.Key,
                    DeliveryIds = delGroup.Where(x => nearByIds.Contains(x.DeliveryId)).Select(x => x.DeliveryId).ToList(),

                }).ToList();
    }



    private static double GetDistanceKm(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371; // Earth radius in km
        var dLat = (lat2 - lat1) * Math.PI / 180;
        var dLon = (lon2 - lon1) * Math.PI / 180;
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }
}

public class Delivery
{
    public int DeliveryId { get; set; }
    public string DriverName { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime Timestamp { get; set; }
}

public class NearbyDeliveryGroup
{
    public string DriverName { get; set; }
    public List<int> DeliveryIds { get; set; }
}
