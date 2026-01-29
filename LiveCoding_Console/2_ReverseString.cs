namespace LiveCoding_Console
{
    internal class _2_ReverseString
    {
        public static void Test()
        {
            var str = "aBc123";
            
            Console.WriteLine($"{str} -> Reversed: {Reverse(str)}");
        }

        public static string Reverse(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            char[] chars = input.ToCharArray();
            int left = 0;
            int right = chars.Length - 1;

            while (left < right)
            {
                (chars[left], chars[right]) = (chars[right], chars[left]);
                left++;
                right--;
            }

            return new string(chars);
        }

    }
}
