﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CPO06使用工作做到平行非同步程式設計範例
{
    /// <summary>
    /// 說明 ： 使用工作 Task 做到平行Parallelism非同步 asynchronous 方式分批計算不同區間的所有質數數量
    /// 備註 ： 開始執行前，請打開工作管理員，觀察處理器使用效能趨勢圖
    ///        透過執行緒功能，做到非同步 CPU Bound 集中的處理能力
    ///        使用 Release 方案組態來進行建置與執行
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
            for (int i = 1; i <= partition; i++)
            {
                int begin = part * (i - 1) + 1;
                int end = part * i;
                if (begin < 2) begin = 2;
                if (i == partition) end = lastNumber;
                range.Add((begin, end));
            }
            #endregion

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            #region 找出所有的質數
            ConcurrentBag<List<int>> allPrimes = new ConcurrentBag<List<int>>();
            // 儲存所有非同步工作
            List<Task> allTasks = new List<Task>();
            for (int i = 0; i < partition; i++)
            {
                int idx = i;
                allTasks.Add(Task.Run(() =>
                {
                    allPrimes.Add(ComputeAllPrimeNumbers
                        (range[idx].begin, range[idx].end));
                }));
            }

            // 等候 所有非同步工作 完成
            Task.WaitAll(allTasks.ToArray());
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
