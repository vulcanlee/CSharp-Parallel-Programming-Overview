using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CPO15用委派以非同步的方式呼叫同步方法
{
    /// <summary>
    /// 說明 ： 使用委派以非同步的方式呼叫同步方法
    /// 備註 ： 委派支援 APM 非同步方式呼叫
    ///        也就是使用 BeginInvoke , EndInvoke
    ///        .NET Core 不支援這樣的做法
    /// </summary>
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
