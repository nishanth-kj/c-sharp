using System;
public class Shape
{
public virtual void draw()
{
Console.WriteLine("Drawing Shape");
}
public virtual void erase()
{
Console.WriteLine("Erasing Shape");
}
}
public class Circle : Shape
{
public override void draw()
{
Console.WriteLine("Drawing Circle");
}
public override void erase()
{
Console.WriteLine("Erasing Circle");
}
}
public class Triangle : Shape
{
public override void draw()
{
Console.WriteLine("Drawing Triangle");
}
public override void erase()
{
Console.WriteLine("Erasing Triangle");
}
}
public class Square : Shape
{
public override void draw()
{
Console.WriteLine("Drawing Square");
}
public override void erase()
{
Console.WriteLine("Erasing Square");
}
}
public class Program
{
public static void Main()
{
Shape[] shapes = new Shape[3];
shapes[0] = new Circle();
shapes[1] = new Triangle();
shapes[2] = new Square();
foreach (Shape shape in shapes)
{
shape.draw(); shape.erase();
}
}
}