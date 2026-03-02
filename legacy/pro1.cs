using System;
public class Arthematic
{
public static void Main(string [] args)
{
System.Console.WriteLine("Enter a First Number: ");
int num1 = Convert.ToInt32(Console.ReadLine());
System.Console.WriteLine("Enter a Second Number: ");
int num2 = Convert.ToInt32(Console.ReadLine());
System.Console.WriteLine("{0} + {1} = {2}", num1, num2, num1 + num2);
System.Console.WriteLine("{0} - {1} = {2}", num1, num2, num1 - num2);
System.Console.WriteLine("{0} x {1} = {2}", num1, num2, num1 * num2);
System.Console.WriteLine("{0} / {1} = {2}", num1, num2, num1 / num2);
System.Console.WriteLine("{0} mod {1} = {2}", num1, num2, num1 % num2);
}
}
