using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter the number of rows for Pascal's Triangle: ");
        int numRows = int.Parse(Console.ReadLine());

        Console.Write("Pascal's Triangle:\n");
        PrintPascalsTriangle(numRows);
    }

    static void PrintPascalsTriangle(int numRows)
    {
        for (int i = 0; i < numRows; i++)
        {
            // Print leading spaces
            for (int j = 0; j < numRows - i - 1; j++)
            {
                Console.Write("  ");
            }

            // Print asterisks representing the numbers in Pascal's Triangle
            for (int j = 0; j <= i; j++)
            {
                Console.Write("*   ");
            }

            Console.WriteLine();  // Move to the next line for the next row
        }
    }
}
