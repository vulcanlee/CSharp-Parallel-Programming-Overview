using System;
using System.Net;

namespace CPO08使用EAP進行非同步Asynchronous程式設計範例
{
    /// <summary>
    /// 說明 ： 使用EAP Event-based Asynchronous Pattern 進行非同步Asynchronous網頁內容取得計算
    /// 備註 ： 這是從 .NET Framework 2.0 開始使用的非同步API設計方式
    ///        透過 綁定 Callback 委派方法 來進行各種非同步操作
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            WebClient wc = new WebClient();
            // 指定當 在非同步資源下載作業完成時發生 ，要執行的委派方法
            wc.DownloadStringCompleted += (s, e) =>
            {
                if (e.Error != null)
                    Console.WriteLine(e.Error);
                else if (e.Cancelled)
                    Console.WriteLine("取消");
                else
                {
                    Console.WriteLine(e.Result.Length);
                }
            };

            // 開始啟動非同步作業
            wc.DownloadStringAsync(new Uri("http://www.google.com"));

            Console.WriteLine("Press any key for continuing...");
            Console.ReadKey();
        }
    }
}
