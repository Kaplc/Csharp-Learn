using System;

namespace Flight_chess
{
    
    internal class Program
    {
        public static void init()
        {
            int windowsWilde = 60;
            int windowsHight = 30;
            Console.WindowWidth = windowsWilde;
            Console.WindowHeight = windowsHight;
            Console.BufferWidth = windowsWilde;
            Console.BufferHeight = windowsHight;
            Console.CursorVisible = false;
        }
        public static void Main(string[] args)
        {
            init();
            Console.WriteLine("按下任意键继续");
            Console.ReadLine();
        }
    }
}