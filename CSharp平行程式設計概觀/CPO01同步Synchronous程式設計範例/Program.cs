using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace CPO01同步Synchronous程式設計範例
{
    /// <summary>
    /// 說明 ： 使用同步 Synchronous 方式來計算出指定數值內的所有質數數量
    /// 備註 ： 開始執行前，請打開工作管理員，觀察處理器使用效能趨勢圖
    ///        請根據本身電腦，調整成為適當數值大小，設定完成計算時間在 30 秒內
    ///        使用 Release 方案組態來進行建置與執行
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //// 指定僅使用單一邏輯處理核心來執行
            Process.GetCurrentProcess().ProcessorAffinity =
                (IntPtr)0b0001_0000;
            // 請根據本身電腦，調整成為適當的大小
            int lastNumber = 50000000;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            // 找出所有的質數
            var allPrimes = ComputeAllPrimeNumbers(2, lastNumber);
            stopwatch.Stop();
            Console.WriteLine("Primes : {0}\nTime: {1}", allPrimes.Count, stopwatch.ElapsedMilliseconds);
        }
        #region 顯示執行緒資訊
        static void ShowThread(string message)
        {
            Console.WriteLine($"{message} , Id={Thread.CurrentThread.ManagedThreadId}");
        }
        #endregion

        #region 找出兩數值間的所有質數
        static List<int> ComputeAllPrimeNumbers(int min, int max)
        {
            List<int> result = new List<int>();
            for (int i = min; i <= max; i++)
            {
                if (IsPrime(i))
                {
                    result.Add(i);
                }
            }
            return result;
        }
        #endregion

        #region 判斷指定數值是否為質數
        static bool IsPrime(int n)
        {
            if (n % 2 == 0)
            {
                return n == 2;
            }
            else
            {
                var topLimit = (int)Math.Sqrt(n);
                for (int i = 3; i <= topLimit; i += 2)
                {
                    if (n % i == 0) return false;
                }
                return true;
            }
        }
        #endregion
    }
}
