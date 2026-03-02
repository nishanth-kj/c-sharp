using System;

public class Armstrong
{
    public static void Main(string[] args)
    {
        FindArmstrongNumbersInRange(1, 1000);
    }

    public static void FindArmstrongNumbersInRange(int start, int end)
    {
        for (int number = start; number <= end; number++)
        {
            if (IsArmstrongNumber(number))
            {
                Console.WriteLine(number);
            }
        }
    }

    public static bool IsArmstrongNumber(int num)
    {
        int originalNumber = num;
        int sum = 0;

        while (num > 0)
        {
            int digit = num % 10;
            sum += (int)Math.Pow(digit, CountDigits(originalNumber));
            num /= 10;
        }

        return originalNumber == sum;
    }

    public static int CountDigits(int num)
    {
        int count = 0;

        while (num > 0)
        {
            num /= 10;
            count++;
        }

        return count;
    }
}
