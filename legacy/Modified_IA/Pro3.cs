using System;
using System.Linq;

public class Substrings
{
    // Function to check if a string contains numeric characters
    public static bool ContainsNumeric(string str)
    {
        return str.Any(char.IsDigit);
    }

    public static void find_substrings(string input_string)
    {
        // Check if the input string contains numeric characters
        if (ContainsNumeric(input_string))
        {
            Console.WriteLine("Invalid input. The input string should not contain numeric characters.");
            return;
        }

        // Iterate through all possible substrings
        for (int i = 1; i <= input_string.Length; i++)
        {
            for (int j = 0; j <= input_string.Length - i; j++)
            {
                string substring = input_string.Substring(j, i);

                // Print the substring
                Console.WriteLine(substring);
            }
        }
    }

    public static void Main(string[] args)
    {
        string input_string;
        Console.WriteLine("Enter a String:");
        input_string = Console.ReadLine();
        find_substrings(input_string);
    }
}
