namespace LiveCoding_Console
{
    // merge overlapping intervals
    public class Mock12
    {
        public Mock12()
        {
            var result = MergeOverlappingIntervals([[1,2], [3,5], [4,7], [6,8], [9,10]]);

            result = MergeOverlappingIntervals([
    [1, 22],
    [-20, 30]
  ]);
            foreach (var interval in result)
            {
                Console.WriteLine($"[{interval[0]}, {interval[1]}]");
            }
        }

        public int[][] MergeOverlappingIntervals(int[][] intervals)
        {
            intervals = intervals.OrderBy(x => x[0]).ToArray();
            List<int[]> merged = new List<int[]> { intervals[0] };
          
            for (int i = 1; i < intervals.Length; i++)
            {
                var current = intervals[i];
                if (merged.Last()[1] >= current[0])
                {
                    // merge
                    if (merged.Last()[1] < current[1])
                        merged.Last()[1] = current[1];
                }
                else
                {
                    merged.Add(current);
                }
            }
            return merged.ToArray();
        }
    }
}
