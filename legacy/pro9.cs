using System;
public struct Complex
{
public int real;
public int imaginary;
public Complex(int real, int imaginary)
{
this.real = real;
this.imaginary = imaginary;
}
public static Complex operator +(Complex one, Complex two)
{
return new Complex(one.real + two.real, one.imaginary + two.imaginary);
}
public override string ToString()
{
return (String.Format("{0} + {1}i", real, imaginary));
}
}
public class VtuCode
{
public static void Main(string [] args)
{
Complex val1 = new Complex(7, 1);
Complex val2 = new Complex(2, 6);
Complex res = val1 + val2;
System.Console.WriteLine("First: {0}", val1);
System.Console.WriteLine("Second: {0}", val2);
System.Console.WriteLine("Result (Sum): {0}", res);
System.Console.ReadLine();
}
}