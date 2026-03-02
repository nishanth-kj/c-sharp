using System;
namespace FloydTriangle
{
    class Program
    {
        static void Main(string[] args)
        {
            int row, i, j, num = 1; Console.Write("Enter the number of rows: ");
            row = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("");
            for (i = 1; i <= row; i++)
            {
                for (j = 1; j <= i; j++)
                {
                    Console.Write(num + " ");
                    num++;
                }
                Console.WriteLine("");
            }
            Console.ReadKey();
        }
    }
}