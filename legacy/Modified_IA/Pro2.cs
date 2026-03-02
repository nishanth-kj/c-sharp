using System;

public class ArmstrongNumberProgram
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter the starting number: ");
        int start = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Enter the ending number: ");
        int end = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Armstrong numbers between {0} and {1}:", start, end);
        for (int i = start; i <= end; i++)
        {
            if (IsArmstrongNumber(i))
            {
                Console.WriteLine(i);
            }
        }
    }

    // Function to check if a number is an Armstrong number
    static bool IsArmstrongNumber(int number)
    {
        int originalNumber, remainder, result = 0, n = 0;

        originalNumber = number;

        // Count the number of digits
        for (originalNumber = number; originalNumber != 0; originalNumber /= 10, ++n) ;

        originalNumber = number;

        // Calculate the sum of nth power of each digit
        while (originalNumber != 0)
        {
            remainder = originalNumber % 10;
            result += (int)Math.Pow(remainder, n);
            originalNumber /= 10;
        }

        // Check if the number is an Armstrong number
        return result == number;
    }
}
