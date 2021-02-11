﻿using System;
using System.Net.Http;

namespace CPO09使用TAP進行非同步Asynchronous程式設計範例
{
    /// <summary>
    /// 說明 ： 使用TAP進行非同步Asynchronous網頁內容取得計算
    /// 備註 ： 這是從 .NET Framework 4.0 開始使用的非同步API設計方式
    ///        透過 TPL Task Parallel Library 來進行各種非同步操作
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient httpClient = new HttpClient();
            // 開始啟動非同步作業
            var task = httpClient.GetStringAsync("http://www.google.com");
            string result = task.Result;
            Console.WriteLine(result.Length);

            Console.WriteLine("Press any key for continuing...");
            Console.ReadKey();
        }
    }
}
