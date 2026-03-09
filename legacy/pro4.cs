using System;
public class Divide
{
    public static void Main(string[] args)
    {
        try
        {

            int result = 15 / int.Parse("0");
            System.Console.WriteLine(result);
        }
        catch (DivideByZeroException e)
        {
            System.Console.WriteLine(e.Message);
            System.Console.ReadLine();
        }
    }
}
