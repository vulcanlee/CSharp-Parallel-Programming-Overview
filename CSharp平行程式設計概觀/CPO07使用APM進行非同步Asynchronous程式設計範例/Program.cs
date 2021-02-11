using System;
using System.IO;
using System.Net;
using System.Text;

namespace CPO07使用APM進行非同步Asynchronous程式設計範例
{
    /// <summary>
    /// 說明 ： 使用APM Asynchronous Programming Model 進行非同步網頁內容取得計算
    /// 備註 ： 這是最早期的非同步API設計方式
    ///        透過 IAsyncResult 來進行各種非同步操作
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // 建立 HttpWebrequest 物件
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest
                .Create("http://www.google.com");

            #region 使用 callback 方式來取得非同步結果(BeginGetResponse 開始啟動非同步作業)
            // 若將 myHttpWebRequest 引數設定為 null來傳進方法內，會發生甚麼問題呢？
            IAsyncResult result =
              (IAsyncResult)myHttpWebRequest
              .BeginGetResponse(new AsyncCallback(ResponseCallback), null);
            #endregion

            #region 直接等待非同步的執行結果
            //IAsyncResult result =
            //  (IAsyncResult)myHttpWebRequest.BeginGetResponse(null, null);
            //HttpWebResponse response = myHttpWebRequest.EndGetResponse(result) as HttpWebResponse;
            //Stream ReceiveStream = response.GetResponseStream();
            //StreamReader reader = new StreamReader(ReceiveStream);
            //Console.WriteLine(reader.ReadToEnd().Length);
            #endregion

            Console.WriteLine("Press any key for continuing...");
            Console.ReadKey();
        }

        private static void ResponseCallback(IAsyncResult ar)
        {
            HttpWebResponse response = (ar.AsyncState as HttpWebRequest)
                .EndGetResponse(ar) as HttpWebResponse;

            Stream ReceiveStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(ReceiveStream);
            Console.WriteLine(reader.ReadToEnd().Length);
        }
    }
}
