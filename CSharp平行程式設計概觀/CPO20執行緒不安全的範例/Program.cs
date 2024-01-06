

using System.Diagnostics;

namespace CPO20執行緒不安全的範例
{
    /// <summary>
    /// 執行緒不安全的範例 - 多執行緒的情況下，會發生資料不一致的情況
    /// </summary>
    internal class Program
    {
        // 最後計數器的數值
        static int count = 0;
        // 迴圈的次數
        static int maxLoop = 100_000_000;
        // 鎖定物件
        static object lockObj = new object();
        // 是否使用鎖定
        static bool isLock = true;
        static async Task Main(string[] args)
        {
            // 計時器  
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            // 建立兩個工作，分別執行 PlusOnes 與 MinusOnes
            var taskPlusOnes = Task.Run(() =>
            {
                PlusOnes();
            });
            var taskMinusOnes = Task.Run(() =>
            {
                MinusOnes();
            });

            // 等候兩個工作執行完畢
            await Task.WhenAll(taskPlusOnes, taskMinusOnes);

            // 停止計時器
            stopwatch.Stop();
            // 顯示結果
            Console.WriteLine($"花費時間: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"count = {count}");
        }

        /// <summary>
        /// 減一的方法
        /// </summary>
        private static void MinusOnes()
        {
            for (int i = 0; i < maxLoop; i++)
            {
                // 如果 isLock 為 true，則使用 lock 來鎖定
                if (isLock)
                {
                    // 鎖定 lockObj
                    lock (lockObj)
                    {
                        count--;
                    }
                }
                else
                {
                    // 不使用鎖定
                    count--;
                }
            }
        }

        /// <summary>
        /// 加一的方法
        /// </summary>
        private static void PlusOnes()
        {
            for (int i = 0; i < maxLoop; i++)
            {
                // 如果 isLock 為 true，則使用 lock 來鎖定
                if (isLock)
                {
                    // 鎖定 lockObj
                    lock (lockObj)
                    {
                        count++;
                    }
                }
                else
                {
                    // 不使用鎖定
                    count++;
                }
            }
        }
    }
}
