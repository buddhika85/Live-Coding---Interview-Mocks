using static System.Console;
namespace LiveCoding_Console
{
    public class _3_FirstNonRepeatingChar
    {
        public static void Test()
        {
            WriteLine(FindFirstNonRepeatChar("abc"));
            WriteLine(FindFirstNonRepeatChar("aabcd"));
            WriteLine(FindFirstNonRepeatChar("aabbc1ddd"));
            WriteLine(FindFirstNonRepeatChar("aabbca1212dddA"));
            WriteLine(FindFirstNonRepeatChar("aabbca1212dddAcC"));
        }

        // O(n) time conplexity - because there is a loop
        // O(n) space complexity - because additional DS a Dictionary used
        public static char? FindFirstNonRepeatChar(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;

            var dict = new Dictionary<char, int>();
            foreach (var item in str)
            {
                if (!dict.ContainsKey(item))
                    dict.Add(item, 1);
                else
                    dict[item]++;
            }
            return dict.FirstOrDefault(x => x.Value == 1).Key;
        }


        public static char? FindFirstNonRepeatCharBetter(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;

            var freq = new Dictionary<char, int>();

            // Count frequencies
            foreach (var ch in str)
                freq[ch] = freq.GetValueOrDefault(ch) + 1;

            // Find first non-repeating by scanning original order
            foreach (var ch in str)
                if (freq[ch] == 1)
                    return ch;

            return null;
        }
    }
}
