using System;

namespace Flight_chess
{
    enum E_SceneNum
    {
        StartScene = 1,
        GameScene = 2,
        EndScene = 3,
    }

    internal class Program
    {
        public static void InitConsole()
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
            InitConsole();
            Console.WriteLine("按下任意键继续");
            Console.ReadLine();
        }
    }
}