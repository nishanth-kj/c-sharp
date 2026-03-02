using System;
public class Substrings
{
public static void find_substrings(string input_string)
{
int j = 0;
int i = 0;
for (i = 1; i <= input_string.Length; i++)
{
for (j = 0; j <= input_string.Length - i; j++)
{
System.Console.WriteLine(input_string.Substring(j, i));
}
}
}
public static void Main(string [] args)
{
string input_string;
System.Console.WriteLine("Enter a String :");
input_string = System.Console.ReadLine();
find_substrings(input_string);
}
}