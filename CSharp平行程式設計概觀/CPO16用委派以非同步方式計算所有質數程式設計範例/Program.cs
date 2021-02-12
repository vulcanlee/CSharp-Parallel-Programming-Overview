using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPO16用委派以非同步方式計算所有質數程式設計範例
{
    /// <summary>
    /// 說明 ： 使用委派的BeginInvoke , EndInvoke進行非同步Asynchronous方式分批計算不同區間的所有質數數量
    /// 備註 ： 使用 Task.Factory.FromAsync 將 APM 程式碼封裝成為 TAP
    ///        .NET Core 不支援這樣的做法
    /// </summary>
    class Program
    {
        delegate List<int> ComputeAllPrimeNumbersDel(int min, int max);
        static void Main(string[] args)
        {
            // 請根據本身電腦，調整成為適當的大小
            int lastNumber = 20000000;
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
            List<List<int>> allPrimes = new List<List<int>>();
            ComputeAllPrimeNumbersDel computeAllPrimeNumbersHandle;
            computeAllPrimeNumbersHandle = ComputeAllPrimeNumbers;
            List<Task<List<int>>> allTasks = new List<Task<List<int>>>();
            for (int i = 0; i < partition; i++)
            {
                allTasks.Add(Task.Factory.FromAsync(
                    computeAllPrimeNumbersHandle
                    .BeginInvoke(range[i].begin, range[i].end, null, null),
                    computeAllPrimeNumbersHandle.EndInvoke));
            }
            Task.WaitAll(allTasks.ToArray());
            for (int i = 0; i < partition; i++)
            {
                allPrimes.Add(allTasks[i].Result);
            }
            #endregion

            stopwatch.Stop();

            allPrimes.Sum(x => x.Count());
            Console.WriteLine("Primes : {0}\nTime: {1}",
                allPrimes.Sum(x => x.Count()), stopwatch.ElapsedMilliseconds);
        }

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
