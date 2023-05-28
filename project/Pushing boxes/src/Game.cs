using System;
using System.Collections.Generic;
using System.Threading;

namespace Pushing_boxes
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
        SelectMap,
        End
    }

    #endregion

    public class Game
    {
        private static int windowWide = 42;
        private static int windowHight = 30;
        public static MapInfos mapInfos;

        public static int WindowWide
        {
            get { return windowWide; }
        }

        public static int WindowHight
        {
            get { return windowHight; }
        }

        private static I_UpdateGameImage nowScene;


        public Game()
        {
            mapInfos = new MapInfos();
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
                case E_SceneType.SelectMap:
                    nowScene = new SelectMap();
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
    public abstract class StartEndSceneBase : I_UpdateGameImage
    {
        protected string title;
        protected string firstSelect;
        protected string secondSelect;
        protected int selectNum = 1; // 默认选择第一个选项
        protected int w = Game.WindowWide;
        protected int h = Game.WindowHight;

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
            Console.SetCursorPosition(w / 2  - title.Length, h / 4);
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
    public class StartScene : StartEndSceneBase
    {
        private string mapSelect;

        public StartScene() // 子类自动调用父类构造函数
        {
            title = "推 箱 子";
            firstSelect = "开始游戏";
            mapSelect = "关卡选择";
            secondSelect = "退出游戏";
            selectNum = 1;
        }

        protected override void EnterJ()
        {
            if (selectNum == 1)
            {
                Game.SceneChange(E_SceneType.Game);
            }
            else if (selectNum == 2)
            {
                Game.SceneChange(E_SceneType.SelectMap);
            }
            else
            {
                Environment.Exit(0);
            }
        }

        public override void UpdateGameImage()
        {
            Console.SetCursorPosition(w / 2 + 1 - title.Length, h / 4);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(title);

            ConsoleColor firstSelectColor = ConsoleColor.Red;
            ConsoleColor mapSelectColor = ConsoleColor.White;
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
                Console.ForegroundColor = mapSelectColor;
                if (selectNum == 2)
                {
                    Console.Write(mapSelect);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("  ←");
                }
                else
                {
                    Console.WriteLine(mapSelect + "      ");
                }

                Console.SetCursorPosition(w / 2 - 4, h / 3 + 6);
                Console.ForegroundColor = secondSelectColor;
                if (selectNum == 3)
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

                        selectNum--;
                        break;
                    case ConsoleKey.S:
                        if (selectNum == 3)
                        {
                            break;
                        }

                        selectNum++;
                        break;
                    case ConsoleKey.J:
                        EnterJ();
                        return;
                }

                switch (selectNum)
                {
                    case 1:
                        firstSelectColor = ConsoleColor.Red;
                        mapSelectColor = ConsoleColor.White;
                        secondSelectColor = ConsoleColor.White;
                        break;
                    case 2:
                        firstSelectColor = ConsoleColor.White;
                        mapSelectColor = ConsoleColor.Red;
                        secondSelectColor = ConsoleColor.White;
                        break;
                    case 3:
                        firstSelectColor = ConsoleColor.White;
                        mapSelectColor = ConsoleColor.White;
                        secondSelectColor = ConsoleColor.Red;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 关卡 
    /// </summary>
    public class MapIndex
    {

        public string title;
        public Position pos;


        public MapIndex(string title, int x, int y)
        {
            pos = new Position(x, y);
            this.title = title;
        }

        public void Draw()
        {
            Console.SetCursorPosition(pos.x, pos.y);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(title);
        }

        public void CurrDraw()
        {
            Console.SetCursorPosition(pos.x, pos.y);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(title);
        }
    }

    /// <summary>
    /// 选择关卡
    /// </summary>
    public class SelectMap : I_UpdateGameImage
    {
        private string title = "关卡选择";
        private List<MapIndex> mapIndex = new List<MapIndex>();
        public static int currMapIndex = 0;

        public SelectMap()
        {
            for (int i = 0; i < MapInfos.infos.Count; i++)
            {
                mapIndex.Add(new MapIndex((i + 1).ToString(), 13 + i * 4, 12));
            }
        }

        public void UpdateGameImage()
        {
            Console.SetCursorPosition(Game.WindowWide / 2 - title.Length, 8);
            Console.Write(title);

            for (int i = 0; i < mapIndex.Count; i++)
            {
                mapIndex[i].Draw();
            }

            while (true)
            {
                mapIndex[currMapIndex].CurrDraw();
                Console.SetCursorPosition(mapIndex[currMapIndex].pos.x, mapIndex[currMapIndex].pos.y + 1);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("A");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.A:
                        mapIndex[currMapIndex].Draw();
                        Console.SetCursorPosition(mapIndex[currMapIndex].pos.x, mapIndex[currMapIndex].pos.y + 1);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(" ");
                        currMapIndex--;
                        if (currMapIndex <= 0)
                        {
                            currMapIndex = 0;
                        }

                        break;
                    case ConsoleKey.D:
                        mapIndex[currMapIndex].Draw();
                        Console.SetCursorPosition(mapIndex[currMapIndex].pos.x, mapIndex[currMapIndex].pos.y + 1);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(" ");
                        currMapIndex++;
                        if (currMapIndex > mapIndex.Count - 1)
                        {
                            currMapIndex = mapIndex.Count - 1;
                        }

                        break;
                    case ConsoleKey.J:
                        Game.SceneChange(E_SceneType.Game);
                        return;
                }
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
            title = "成功通关";
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

    /// <summary>
    /// 游戏场景
    /// </summary>
    public class GameScene : I_UpdateGameImage
    {
        public int mapIndex = SelectMap.currMapIndex;
        public GameMap map;
        public MoveThread moveThread = MoveThread.Instance;

        public GameScene()
        {
            map = new GameMap(mapIndex);
            moveThread.ac += map.CheckKeyBroad; // 添加输入线程
        }

        public void UpdateGameImage()
        {
            Console.SetCursorPosition(16, Game.WindowHight-8);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("WASD:控制方向 ");
            Console.SetCursorPosition(10, Game.WindowHight-6);
            Console.WriteLine("N:重新开始, ESC: 返回主菜单");


            while (true)
            {
                lock (map)
                {
                    map.Draw();
                    if (map.GameOver())
                    {
                        SelectMap.currMapIndex++;
                        if (SelectMap.currMapIndex>MapInfos.infos.Count-1)
                        {
                            SelectMap.currMapIndex--;
                            Game.SceneChange(E_SceneType.End);
                            break;
                        }
                        Game.SceneChange(E_SceneType.Game);
                        break;
                    }

                    if (map.reStart)
                    {
                        Game.SceneChange(E_SceneType.Game);
                        break;
                    }

                    if (map.back)
                    {
                        Game.SceneChange(E_SceneType.Start);
                        break;
                    }
                }
            }
            moveThread.ac -= map.CheckKeyBroad; // 关闭输入线程
        }
    }
    
    /// <summary>
    /// 输入线程
    /// </summary>
    public class MoveThread
    {
        private Thread t;
        public Action ac;
        
        private static MoveThread instance = new MoveThread();

        public static MoveThread Instance
        {
            get => instance;
        }

        private MoveThread()
        {
            t = new Thread(Strat);
            t.Start();
        }

        public void Strat()
        {
            while (true)
            {
                if (ac== null)
                {
                    continue;
                }
                ac.Invoke();
            }
        }
    }
    
}