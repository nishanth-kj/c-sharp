using System;
public class Arthematic
{
    public static void Main(string[] args)
    {
        System.Console.WriteLine("Enter a First Number: ");
        int num1 = Convert.ToInt32(Console.ReadLine());
        System.Console.WriteLine("Enter a Second Number: ");
        int num2 = Convert.ToInt32(Console.ReadLine());
        System.Console.WriteLine("Addition      :{0} + {1} = {2}", num1, num2, num1 + num2);
        System.Console.WriteLine("Subtraction   :{0} - {1} = {2}", num1, num2, num1 - num2);
        System.Console.WriteLine("Multipication :{0} x {1} = {2}", num1, num2, num1 * num2);
        // Real division for float is performed using '/'
        System.Console.WriteLine("Real Divison       :{0} / {1} = {2}", num1, num2, num1 / num2);
        System.Console.WriteLine("Mod           :{0} mod {1} = {2}", num1, num2, num1 % num2);

        // Exponentiation
        System.Console.WriteLine("Enter a Base Number: ");
        float num11 = Convert.ToSingle(Console.ReadLine());

        System.Console.WriteLine("Enter an Exponent: ");
        float num22 = Convert.ToSingle(Console.ReadLine());


        double result = Math.Pow(num11, num22);
        System.Console.WriteLine("{0} ^ {1} = {2}", num11, num22, result);

        // Square root
        // Prompt the user to enter a number
        Console.WriteLine("Enter a number: ");
        double number = Convert.ToDouble(Console.ReadLine());

        // Check if the entered number is non-negative
        if (number >= 0)
        {
            // Calculate the square root
            double squareRoot = Math.Sqrt(number);

            // Display the result
            Console.WriteLine("The square root of {0} is: {1}", number, squareRoot);
        }
        else
        {
            Console.WriteLine("Cannot calculate the square root of a negative number.");
        }


    }

}

