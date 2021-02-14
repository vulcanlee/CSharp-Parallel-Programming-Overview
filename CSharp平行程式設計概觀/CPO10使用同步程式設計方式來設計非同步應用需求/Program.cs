using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CPO10使用同步程式設計方式來設計非同步應用需求
{
    /// <summary>
    /// 說明 ： 使用async/await進行非同步Asynchronous網頁內容取得計算
    /// 備註 ： 這是從 C# 5.0 / .NET Framework 4.0 開始使用的非同步API設計方式
    ///        讓開發人員可以使用同步程式設計方式來設計非同步應用需求
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient httpClient = new HttpClient();
            Console.WriteLine($"Thread Id : {Thread.CurrentThread.ManagedThreadId}");
            // 開始啟動非同步作業
            var task = httpClient.GetStringAsync("http://www.google.com");
            string result = await task;
            Console.WriteLine(result.Length);
            Console.WriteLine($"Thread Id : {Thread.CurrentThread.ManagedThreadId}");

            Console.WriteLine("Press any key for continuing...");
            Console.ReadKey();
        }
        #region 顯示執行緒資訊
        static void ShowThread(string message)
        {
            Console.WriteLine($"{message} , Id={Thread.CurrentThread.ManagedThreadId}");
        }
        #endregion
    }
}
