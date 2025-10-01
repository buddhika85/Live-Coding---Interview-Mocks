namespace LiveCoding_Console;

internal class Mock8
{
    public Mock8()
    {
        var readings = new List<SensorReading>
        {
            new SensorReading { ReadingId = 1, SensorId = 101, Timestamp = new DateTime(2023, 9, 1, 10, 0, 0), Value = 100 },
            new SensorReading { ReadingId = 2, SensorId = 101, Timestamp = new DateTime(2023, 9, 1, 10, 5, 0), Value = 125 },
            new SensorReading { ReadingId = 3, SensorId = 101, Timestamp = new DateTime(2023, 9, 1, 10, 20, 0), Value = 110 },
            new SensorReading { ReadingId = 4, SensorId = 102, Timestamp = new DateTime(2023, 9, 1, 10, 0, 0), Value = 200 },
            new SensorReading { ReadingId = 5, SensorId = 102, Timestamp = new DateTime(2023, 9, 1, 10, 8, 0), Value = 210 }
        };

        List<AnomalousReadingGroup> anomalousReadingGroupList = GetAnomalousReadingGroupList(readings);
    }

    private List<AnomalousReadingGroup> GetAnomalousReadingGroupList(List<SensorReading> readings)
    {
        if (!readings.Any())
            return new List<AnomalousReadingGroup>();

        return (from reading in readings
                group reading by reading.SensorId into sensorGroup

                let sorted = sensorGroup.OrderBy(x => x.Timestamp)

                let anomalousIds = sorted.SelectMany((x, i) => sorted.Skip(i + 1)
                            .Where(y => (y.Timestamp - x.Timestamp).TotalMinutes <= 10 &&
                            ((y.Value - x.Value) / x.Value) * 100 > 20)).Select(x => x.ReadingId)
                            .Distinct()

                select new AnomalousReadingGroup
                {
                    SensorId = sensorGroup.Key,
                    ReadingIds = sensorGroup.Where(x => anomalousIds.Contains(x.ReadingId)).Select(x => x.ReadingId).ToList()
                }).ToList();
    }
}

public class SensorReading
{
    public int ReadingId { get; set; }
    public int SensorId { get; set; }
    public DateTime Timestamp { get; set; }
    public double Value { get; set; }
}

public class AnomalousReadingGroup
{
    public int SensorId { get; set; }
    public List<int> ReadingIds { get; set; }
}
