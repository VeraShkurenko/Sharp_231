using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.AsyncProgramming
{
    internal class Threading
    {
        public void Run()
        {
            Thread t10;
            CancellationTokenSource cts = new();
            try
            {
                t10 = new Thread(ThreadActivity);
                t10.Start(new ThreadData(10, cts.Token));
                new Thread(ThreadActivity).Start("Msg");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Press s key");
            Console.ReadKey();
            //t10.Abort();       not supported
            cts.Cancel();
        }

        private void ThreadActivity(Object? arg)
        {
            try
            {
                if (arg is ThreadData data)
                {
                    String res = "";
                    StringBuilder sb = new();
                    for (int i = 0; i < data.N; i += 1)
                    {
                        Thread.Sleep(1000);
                        //res += i;             // bad bad bad!
                        sb.Append(i);
                        Console.WriteLine($"processed {i}");
                        data.CancellationToken.ThrowIfCancellationRequested();
                    }
                    res = sb.ToString();
                    Console.WriteLine(res);
                }
                else
                {
                    //throw new ArgumentException("arg should be int");
                    Console.WriteLine("Wrong call: arg should be int");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Виняток у потоці: " + ex.Message);
            }
        }
    }

    internal record ThreadData(int N, CancellationToken CancellationToken) { }
}
