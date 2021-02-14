using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace CPO12你已經孰悉BackgroundWorker非同步程式設計範例
{
    /// <summary>
    /// 說明 ： 使用BackgroundWorker進行非同步Asynchronous方式分批計算不同區間的所有質數數量
    /// 備註 ： 這是從 .NET Framework 2.0 開始使用的非同步API設計方式
    ///        目的在於在使用同執行緒上執行作業，解決 GUI 應用程式的螢幕凍結問題
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // 請根據本身電腦，調整成為適當的大小
            int lastNumber = 50000000;
            #region 計算切割成為 n 個資料區塊的開始與結束數值
            int partition = 4;
            int part = lastNumber / partition;
            List<(int begin, int end)> range = new List<(int begin, int end)>();
            List<AutoResetEvent> autoResetEvents = new List<AutoResetEvent>();
            List<BackgroundWorker> allBackgroundWorkers = new List<BackgroundWorker>();
            for (int i = 1; i <= partition; i++)
            {
                int begin = part * (i - 1) + 1;
                int end = part * i;
                if (begin < 2) begin = 2;
                if (i == partition) end = lastNumber;
                range.Add((begin, end));
                autoResetEvents.Add(new AutoResetEvent(false));
                allBackgroundWorkers.Add(new BackgroundWorker());
            }
            #endregion

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            #region 找出所有的質數
            ConcurrentBag<List<int>> allPrimes = new ConcurrentBag<List<int>>();
            Console.WriteLine($"Thread Id : {Thread.CurrentThread.ManagedThreadId}");
            for (int i = 0; i < partition; i++)
            {
                int idx = i;
                allBackgroundWorkers[idx].DoWork += ((s, e) =>
                {
                    Console.WriteLine($"DoWork Thread Id : {Thread.CurrentThread.ManagedThreadId}");
                    allPrimes.Add(ComputeAllPrimeNumbers
                        (range[idx].begin, range[idx].end));
                    autoResetEvents[idx].Set();
                });
                // 開始啟動非同步作業
                allBackgroundWorkers[idx].RunWorkerAsync();
            }

            WaitHandle.WaitAll(autoResetEvents.ToArray());
            #endregion

            stopwatch.Stop();

            allPrimes.Sum(x => x.Count());
            Console.WriteLine("Primes : {0}\nTime: {1}",
                allPrimes.Sum(x => x.Count()), stopwatch.ElapsedMilliseconds);
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
