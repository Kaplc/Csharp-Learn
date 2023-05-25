using System;
using System.Threading;

namespace Tetris
{
    public class MoveTheard
    {
        private Thread moveThread;
        public Action action;

        // 单例模式
        private static MoveTheard instance = new MoveTheard();

        public static MoveTheard Instance
        {
            get => instance;
        }

        private MoveTheard()
        {
            moveThread = new Thread(Start);
            moveThread.IsBackground = true;
            moveThread.Start();
        }

        private void Start()
        {
            while (true)
            {
                if (action != null)
                    action.Invoke();
            }
        }

        public void Stop()
        {
            action = null;
        }
    }
}