using System;
using System.Threading;

namespace Greedy_Snake
{
    #region 枚举定义
    /// <summary>
    /// 场景枚举
    /// </summary>
    enum E_SceneType
    {
        Start,
        Game,
        End
    }
    

    #endregion

    #region 结构体定义

    

    #endregion
    
    /// <summary>
    /// 游戏类
    /// </summary>
    class Game
    {
        private int windowWide;
        private int windowHight;
        public E_SceneType sceneType;

        public Game(int windowWide, int windowHight)
        {
            this.windowWide = windowWide;
            this.windowHight = windowHight;
            sceneType = E_SceneType.Start;
            InitConsole();
            StartGame();
        }

        private void InitConsole()
        {
            Console.WindowWidth = windowWide;
            Console.WindowHeight = windowHight;
            Console.BufferWidth = windowWide;
            Console.BufferHeight = windowHight;
            Console.CursorVisible = false;
        }

        private void StartGame()
        {
            while (true)
            {
                switch (sceneType)
                {
                    case E_SceneType.Start:
                        Console.WriteLine("开始场景");
                        break;
                    case E_SceneType.Game:
                        break;
                    case E_SceneType.End:
                        break;
                }
            }
        }

        private void SceneChange(E_SceneType changeSceneType)
        {
            this.sceneType = changeSceneType;
        }
    }
    
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            Game game = new Game(90,30);
        }
    }
}