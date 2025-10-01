using static System.Console;

// Define a sample array 
string[] words = [
    "First", "Second", "Third", "Forth", "Fifth",
    "Sixth", "Seventh", "Eighth", "Nineth"      
];


          

// The index-from-end operator or hat operator (^) indexes from the end of a sequence
WriteLine(words[^1]);                   // give me last index       , same as WriteLine(words[words.Length - 1]);
WriteLine(words[^2]);                   // give me second from last index



// The range operator (..) defines a range
string[] wordRange = words[2..5];           // give me index 2 to 5
WriteLine($"{string.Join(",", wordRange)}");

wordRange = words[..];                      // give me a copy of words
WriteLine($"{string.Join(",", wordRange)}");

wordRange = words[2..];                         // give me all after 2
WriteLine($"{string.Join(",", wordRange)}");

wordRange = words[..5];                         // give me all before 5
WriteLine($"{string.Join(",", wordRange)}");

// Indexes and ranges can be variables too
Index idx = ^4;                                 // give me 4th from last index
WriteLine(words[idx]);

Range rng = 3..^1;                              // 3rd to last
wordRange = words[rng]; 
WriteLine($"{string.Join(",", wordRange)}");