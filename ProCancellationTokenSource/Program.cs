using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProCancellationTokenSource
{
    class Program
    {
        static async Task Main(string[] args)
        {
            {
                var firstTask = new Task<int>(() => TaskMethod("第一个任务", 3));
                await firstTask.ContinueWith(
                    t =>
                        Console.WriteLine(
                            "第一个任务的返回结果为 {0}. Thread id {1}, 是否是线程池线程: {2}",
                            t.Result,
                            Thread.CurrentThread.ManagedThreadId,
                            Thread.CurrentThread.IsThreadPoolThread
                        ),
                    TaskContinuationOptions.OnlyOnRanToCompletion
                );
                firstTask.Start();
            }

            static int TaskMethod(string name, int seconds)
            {
                Console.WriteLine(
                    "Task Method : Task {0} is running on a thread id {1}. Is thread pool thread: {2}",
                    "yusong",
                    Thread.CurrentThread.ManagedThreadId,
                    Thread.CurrentThread.IsThreadPoolThread
                );
                Thread.Sleep(TimeSpan.FromSeconds(10000));
                return 42 * seconds;
            }

            {
                var t = new Task(() => { });

                var tr = Task.Run(() => { });
                var tf = Task.Factory.StartNew(() => { }, TaskCreationOptions.LongRunning);
            }

            {
                //CancellationTokenSource tokenSource = new CancellationTokenSource();
                //CancellationToken cancellationToken = tokenSource.Token;
                //cancellationToken.Register(() => System.Console.WriteLine($"被取消了.{Thread.CurrentThread.ManagedThreadId}"));
                //tokenSource.CancelAfter(5000);
                //Task.Run(() =>
                //{
                //    System.Console.WriteLine($"阻塞之前{Thread.CurrentThread.ManagedThreadId}");
                //    cancellationToken.WaitHandle.WaitOne();
                //    System.Console.WriteLine($"阻塞取消,执行到了.{Thread.CurrentThread.ManagedThreadId}");
                //});
                //System.Console.WriteLine($"执行到了这里{Thread.CurrentThread.ManagedThreadId}");
            }
            {
                //设置3000毫秒(即3秒)后取消
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(3000);
                CancellationToken cancellationToken = cancellationTokenSource.Token;
                cancellationToken.Register(() => System.Console.WriteLine("我被取消了."));
                System.Console.WriteLine("先等五秒钟.");
                await Task.Delay(5000);
                System.Console.WriteLine("手动取消.");
                //cancellationTokenSource.Cancel();
            }
            {
                //CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                //CancellationToken cancellationToken = cancellationTokenSource.Token;
                //cancellationToken.Register(() => System.Console.WriteLine("取消了？？？"));
                //cancellationToken.Register(() => System.Console.WriteLine("取消了！！！"));
                //cancellationToken.Register(state => System.Console.WriteLine($"取消了。。。{state}"), "啊啊啊");



                //System.Console.WriteLine("做了点别的,然后取消了.");
            }

            {
                //CancellationTokenSource cts = new CancellationTokenSource();

                //Task<int> t = new Task<int>(n => sum(cts.Token, 10000), cts.Token);
                //t.Start();

                //cts.CancelAfter(1000);

                //Console.WriteLine("This sum is:" + t.Result);
            }
        }

        private static int sum(CancellationToken ct, int i)
        {
            int sum = 0;
            for (; i > 0; i--)
            {
                if (ct.IsCancellationRequested)
                {
                    Console.WriteLine("此令牌是否已请求取消66666：" + ct.IsCancellationRequested);
                    return sum;
                }

                ct.ThrowIfCancellationRequested();
                Console.WriteLine("此令牌是否是取消状态：" + ct.CanBeCanceled);
                Console.WriteLine("此令牌是否已请求取消：" + ct.IsCancellationRequested);
                checked
                {
                    sum += i;
                }
            }

            return sum;
        }
    }
}
