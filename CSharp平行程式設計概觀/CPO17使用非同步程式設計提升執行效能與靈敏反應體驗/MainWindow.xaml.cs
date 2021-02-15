using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CPO17使用非同步程式設計提升執行效能與流暢反應體驗
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void 同步執行_Click(object sender, RoutedEventArgs e)
        {
            執行結果.Text = "執行中，請稍後...";
            ProgressBar.IsIndeterminate = true;
            GetAllPrimes();
            ProgressBar.IsIndeterminate = false;
        }

        private void 非同步執行會凍結_Click(object sender, RoutedEventArgs e)
        {
            執行結果.Text = "執行中，請稍後...";
            ProgressBar.IsIndeterminate = true;
            GetAllPrimesByThreadBlock();
            ProgressBar.IsIndeterminate = false;
        }

        private void 非同步執行有例外_Click(object sender, RoutedEventArgs e)
        {
            執行結果.Text = "執行中，請稍後...";
            ProgressBar.IsIndeterminate = true;
            GetAllPrimesByThread();
            //ProgressBar.IsIndeterminate = false;
        }

        private async void 非同步執行_Click(object sender, RoutedEventArgs e)
        {
            執行結果.Text = "執行中，請稍後...";
            ProgressBar.IsIndeterminate = true;
            await GetAllPrimesByTask();
            ProgressBar.IsIndeterminate = false;
        }

        private void 同步呼叫WebAPI_Click(object sender, RoutedEventArgs e)
        {

        }

        private void 非同步呼叫WebAPI_Click(object sender, RoutedEventArgs e)
        {

        }

        #region CPU Bound 同步與非同步計算
        void GetAllPrimes()
        {
            // 請根據本身電腦，調整成為適當的大小
            int lastNumber = 50000000;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            // 找出所有的質數
            var allPrimes = ComputeAllPrimeNumbers(2, lastNumber);
            stopwatch.Stop();
            Console.WriteLine("Primes : {0}\nTime: {1}", allPrimes.Count, stopwatch.ElapsedMilliseconds);
            執行結果.Text = $"耗時 {stopwatch.ElapsedMilliseconds} ms, 發現全部有 {allPrimes.Count} 質數";
        }
        void GetAllPrimesByThreadBlock()
        {
            // 請根據本身電腦，調整成為適當的大小
            int lastNumber = 50000000;
            #region 計算切割成為 n 個資料區塊的開始與結束數值
            int partition = 8;
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
            List<Thread> allThreads = new List<Thread>();
            ConcurrentBag<List<int>> allPrimes = new ConcurrentBag<List<int>>();
            for (int i = 0; i < partition; i++)
            {
                int idx = i;
                allThreads.Add(new Thread(_ =>
                {
                    allPrimes.Add(ComputeAllPrimeNumbers
                        (range[idx].begin, range[idx].end));
                }));
            }
            foreach (var thread in allThreads) { thread.Start(); }
            foreach (var item in allThreads) { item.Join(); }
            #endregion

            stopwatch.Stop();

            allPrimes.Sum(x => x.Count());
            Console.WriteLine("Primes : {0}\nTime: {1}",
                allPrimes.Sum(x => x.Count()), stopwatch.ElapsedMilliseconds);
            執行結果.Text = $"耗時 {stopwatch.ElapsedMilliseconds} ms, 發現全部有 {allPrimes.Sum(x => x.Count())} 質數";
        }
        void GetAllPrimesByThread()
        {
            // 請根據本身電腦，調整成為適當的大小
            int lastNumber = 50000000;
            #region 計算切割成為 n 個資料區塊的開始與結束數值
            int partition = 8;
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
            List<Thread> allThreads = new List<Thread>();
            ConcurrentBag<List<int>> allPrimes = new ConcurrentBag<List<int>>();
            for (int i = 0; i < partition; i++)
            {
                int idx = i;
                allThreads.Add(new Thread(_ =>
                {
                    allPrimes.Add(ComputeAllPrimeNumbers
                        (range[idx].begin, range[idx].end));
                    執行結果.Text = $"耗時 {stopwatch.ElapsedMilliseconds} ms, 發現全部有 {allPrimes.Sum(x => x.Count())} 質數";
                }));
            }
            foreach (var thread in allThreads) { thread.Start(); }
            //foreach (var item in allThreads) { item.Join(); }
            #endregion

            stopwatch.Stop();

            allPrimes.Sum(x => x.Count());
            Console.WriteLine("Primes : {0}\nTime: {1}",
                allPrimes.Sum(x => x.Count()), stopwatch.ElapsedMilliseconds);
            執行結果.Text = $"耗時 {stopwatch.ElapsedMilliseconds} ms, 發現全部有 {allPrimes.Sum(x => x.Count())} 質數";
        }
        async Task GetAllPrimesByTask()
        {
            // 請根據本身電腦，調整成為適當的大小
            int lastNumber = 50000000;
            #region 計算切割成為 n 個資料區塊的開始與結束數值
            int partition = 8;
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
            await Task.WhenAll(allTasks.ToArray());
            #endregion

            stopwatch.Stop();

            allPrimes.Sum(x => x.Count());
            Console.WriteLine("Primes : {0}\nTime: {1}",
                allPrimes.Sum(x => x.Count()), stopwatch.ElapsedMilliseconds);
            執行結果.Text = $"耗時 {stopwatch.ElapsedMilliseconds} ms, 發現全部有 {allPrimes.Sum(x => x.Count())} 質數";
        }
        #region 顯示執行緒資訊
        void ShowThread(string message)
        {
            Console.WriteLine($"{message} , Id={Thread.CurrentThread.ManagedThreadId}");
        }
        #endregion

        #region 找出兩數值間的所有質數
        List<int> ComputeAllPrimeNumbers(int min, int max)
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
        bool IsPrime(int n)
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

        #endregion
    }
}
