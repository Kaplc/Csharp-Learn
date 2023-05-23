using System;

namespace Tetris
{
    #region 接口

    /// <summary>
    /// 游戏更新帧接口
    /// </summary>
    public interface I_UpdateGameImage
    {
        void UpdateGameImage();
    }

    #endregion

    #region 枚举

    /// <summary>
    /// 场景枚举
    /// </summary>
    public enum E_SceneType
    {
        Start,
        Game,
        End
    }

    #endregion

    public class Game
    {
        private static int windowWide = 90;
        private static int windowHight = 30;
        public static int WindowWide
        {
            get
            {
                return windowWide;
            }
        }

        public static int WindowHight
        {
            get
            {
                return windowHight;
            }
        }
        
        private static I_UpdateGameImage nowScene;


        public Game()
        {
            SceneChange(E_SceneType.Start);
            InitConsole();
        }

        private void InitConsole()
        {
            Console.WindowWidth = windowWide + 1;
            Console.WindowHeight = windowHight;
            Console.BufferWidth = windowWide + 1;
            Console.BufferHeight = windowHight;
            Console.CursorVisible = false;
        }

        public void StartGame()
        {
            // 游戏主循环
            while (true)
            {
                if (nowScene != null) // 当场景对象不为空
                {
                    nowScene.UpdateGameImage(); // 更新场景帧
                }
            }
        }

        public static void SceneChange(E_SceneType changeSceneType)
        {
            Console.Clear(); // 每次切换场景清屏
            switch (changeSceneType)
            {
                case E_SceneType.Start: // 开始场景
                    nowScene = new StartScene();
                    break;
                case E_SceneType.Game:
                    nowScene = new GameScene();
                    break;
                case E_SceneType.End:
                    nowScene = new EndScene();
                    break;
            }
        }
    }

    /// <summary>
    /// 开始结束场景基类
    /// </summary>
    abstract class StartEndSceneBase : I_UpdateGameImage
    {
        protected string title;
        protected string firstSelect;
        protected string secondSelect;
        protected int selectNum = 1; // 默认选择第一个选项
        private int w = Game.WindowWide;
        private int h = Game.WindowHight;

        protected StartEndSceneBase()
        {
            // this.currSceneType = currSceneType;
            title = "";
            firstSelect = "";
            secondSelect = "";
        }

        protected virtual void EnterJ()
        {
        }

        public virtual void UpdateGameImage()
        {
            Console.SetCursorPosition(w / 2 - title.Length, h / 4);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(title);
            
            ConsoleColor firstSelectColor = ConsoleColor.Red;
            ConsoleColor secondSelectColor = ConsoleColor.White;
            // 处理选择
            while (true)
            {
                Console.SetCursorPosition(w / 2 - 4, h / 3 + 2);
                Console.ForegroundColor = firstSelectColor;
                if (selectNum == 1)
                {
                    Console.Write(firstSelect);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("  ←");
                }
                else
                {
                    Console.WriteLine(firstSelect + "      ");
                }

                Console.SetCursorPosition(w / 2 - 4, h / 3 + 4);
                Console.ForegroundColor = secondSelectColor;
                if (selectNum == 2)
                {
                    Console.Write(secondSelect);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("  ←");
                }
                else
                {
                    Console.WriteLine(secondSelect + "      ");
                }

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                        if (selectNum == 1)
                        {
                            break;
                        }

                        selectNum = 1;
                        firstSelectColor = ConsoleColor.Red;
                        secondSelectColor = ConsoleColor.White;
                        break;
                    case ConsoleKey.S:
                        if (selectNum == 2)
                        {
                            break;
                        }

                        selectNum = 2;
                        firstSelectColor = ConsoleColor.White;
                        secondSelectColor = ConsoleColor.Red;
                        break;
                    case ConsoleKey.J:
                        EnterJ();
                        return;
                }
            }
        }
    }

    /// <summary>
    /// 开始场景类
    /// </summary>
    class StartScene : StartEndSceneBase
    {
        public StartScene() // 子类自动调用父类构造函数
        {
            title = "俄罗斯方块";
            firstSelect = "开始游戏";
            secondSelect = "退出游戏";
        }

        protected override void EnterJ()
        {
            if (selectNum == 1)
            {
                Game.SceneChange(E_SceneType.Game);
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }

    /// <summary>
    /// 结束场景类
    /// </summary>
    class EndScene : StartEndSceneBase
    {
        public EndScene()
        {
            title = "游戏结束";
            firstSelect = "返回菜单";
            secondSelect = "退出游戏";
        }

        protected override void EnterJ()
        {
            if (selectNum == 1)
            {
                Game.SceneChange(E_SceneType.Start);
            }
            else
            {
                Environment.Exit(0);
            }
        }

        
    }

    class GameScene : I_UpdateGameImage
    {
        public void UpdateGameImage()
        {
            
        }
    }
}