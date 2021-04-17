using System;
using System.Threading;

namespace CPO19NET_Core_不支援委派使用_BeginInvoke_與_EndInvoke
{
    class Program
    {
        delegate void MyMethodDel(string action, int sleep);
        static void Main(string[] args)
        {
            MyMethodDel myMethodHandle;
            myMethodHandle = MyMethod;
            //myMethodHandle += MyMethod2;
            IAsyncResult result1 = myMethodHandle
                .BeginInvoke("我要 1 碗麵", 500, null, null);
            IAsyncResult result2 = myMethodHandle
                .BeginInvoke("我要 1 苦瓜排骨湯", 300, null, null);
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"我正在逛街[{i}] ");
            }
            myMethodHandle.EndInvoke(result1);
            myMethodHandle.EndInvoke(result2);
        }
        #region 顯示執行緒資訊
        static void ShowThread(string message)
        {
            Console.WriteLine($"{message} , Id={Thread.CurrentThread.ManagedThreadId}");
        }
        #endregion
        static void MyMethod(string action, int sleep)
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"{action} {i}");
                Thread.Sleep(sleep);
            }
        }
    }
}
