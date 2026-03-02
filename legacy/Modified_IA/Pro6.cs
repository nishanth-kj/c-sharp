using System;

namespace FloydTriangle
{
    class Program
    {
        static void PrintFloydsTriangle(int rows)
        {
            for (int i = 1; i <= rows; i++)
            {
                for (int j = 1; j <= i; j++)
                {
                    Console.Write("+ ");
                }
                Console.WriteLine("");
            }
        }

        static void Main(string[] args)
        {
            int row;

            Console.Write("Enter the number of rows: ");
            row = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("");

            PrintFloydsTriangle(row);
            PrintFloydsTriangle(row);

            Console.ReadKey();
        }
    }
}
