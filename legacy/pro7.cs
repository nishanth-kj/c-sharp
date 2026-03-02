using System;
using System.IO;
public class Program
{
public static void Main(string[] args)
{
string file = @"C:\Users\braun\OneDrive\Desktop\braham.txt";
System.Console.WriteLine("Reading File using File.ReadAllText()");
if (File.Exists(file))
{
string str = File.ReadAllText(file);
System.Console.WriteLine(str);
}
System.Console.WriteLine();
System.Console.WriteLine("Reading File using File.ReadAllLines()");
if (File.Exists(file))
{
string[] lines = File.ReadAllLines(file);
foreach (string ln in lines)
System.Console.WriteLine(ln);
}
System.Console.WriteLine();
Console.WriteLine("Reading File using StreamReader");
if (File.Exists(file))
{
StreamReader Textfile = new StreamReader(file);
string line;
while ((line = Textfile.ReadLine()) != null)
{
System.Console.WriteLine(line);
}
Textfile.Close();
System.Console.ReadKey();
}
System.Console.WriteLine();
}
}