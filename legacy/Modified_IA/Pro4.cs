using System;

public class Divide
{
    public static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Enter an integer:");
            string input = Console.ReadLine();
            int num1 = int.Parse(input);
            int result = num1 / int.Parse("0");
            Console.WriteLine(result);
        }
        catch (DivideByZeroException e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
        Console.ReadLine();
    }
}
