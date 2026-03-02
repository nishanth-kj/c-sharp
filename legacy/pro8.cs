using System;

namespace ConsoleApplication
{
    public class Stack
    {
        public static void Main(string[] args)
        {
            StackImplementation stack = new StackImplementation();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nStack MENU(size -- 10)");
                Console.WriteLine("1. Add an element");
                Console.WriteLine("2. See the Top element.");
                Console.WriteLine("3. Remove top element.");
                Console.WriteLine("4. Display stack elements.");
                Console.WriteLine("5. Exit");
                Console.Write("Select your choice: ");
                int choice;
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("Enter an Element : ");
                            stack.Push(Console.ReadLine());
                            break;
                        case 2:
                            Console.WriteLine("Top element is: {0}", stack.Peek());
                            break;
                        case 3:
                            Console.WriteLine("Element removed: {0}", stack.Pop());
                            break;
                        case 4:
                            stack.Display();
                            break;
                        case 5:
                            Environment.Exit(1);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }

                Console.ReadKey();
            }
        }
    }

    public interface IStackADT
    {
        bool isEmpty();
        void Push(object element);
        object Pop();
        object Peek();
        void Display();
    }

    public class StackImplementation : IStackADT
    {
        private const int StackSize = 10;
        private int top;
        private readonly object[] items;

        public StackImplementation()
        {
            items = new object[StackSize];
            top = -1;
        }

        public bool isEmpty()
        {
            return top == -1;
        }

        public void Push(object element)
        {
            if (top == (StackSize - 1))
            {
                Console.WriteLine("Stack is full!");
            }
            else
            {
                items[++top] = element;
                Console.WriteLine("Item pushed successfully!");
            }
        }

        public object Pop()
        {
            if (isEmpty())
            {
                Console.WriteLine("Stack is empty!");
                return "No elements";
            }
            else
            {
                return items[top--];
            }
        }

        public object Peek()
        {
            if (isEmpty())
            {
                Console.WriteLine("Stack is empty!");
                return "No elements";
            }
            else
            {
                return items[top];
            }
        }

        public void Display()
        {
            for (int i = top; i > -1; i--)
            {
                Console.WriteLine("Item {0}: {1}", (i + 1), items[i]);
            }
        }
    }
}
