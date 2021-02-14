using System;
using System.Threading;
using System.Timers;

namespace CPO11你已經孰悉Timer非同步程式設計範例
{
    /// <summary>
    /// 說明 ： 使用 Timer 類別做到非同步方面的應用開發
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            #region 準備 Timer 物件
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 5000;
            timer.AutoReset = false;
            timer.Elapsed += Timer_Elapsed;
            #endregion

            Console.WriteLine($"[我] : 老闆我要一碗麵");
            Console.WriteLine($"[老闆] : 沒問題，5秒後就會好");
            Console.WriteLine($"[我] : 好，那我等下過來拿");
            timer.Start();
            for (int i = 0; i < 6; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"[我] : 正在逛街中...");
            }

            Console.WriteLine("Press any key for continuing...");
            Console.ReadKey();
            Console.WriteLine($"");
            Console.WriteLine($"");
        }
        #region 顯示執行緒資訊
        static void ShowThread(string message)
        {
            Console.WriteLine($"{message} , Id={Thread.CurrentThread.ManagedThreadId}");
        }
        #endregion

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine($"[老闆] : 客人，你的麵做好了");
        }
    }
}
