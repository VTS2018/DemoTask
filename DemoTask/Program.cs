using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace DemoTask
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Console.WriteLine("主线程启动");
            
            //线程池方式：普通的
            //ThreadPool.QueueUserWorkItem(StartCode, 5);

            //任务的方式：Task
            //参数：方法名字 参数对象
            new Task(StartCode, 5).Start();
            Console.WriteLine("主线程运行到此！");
            Thread.Sleep(1000);
            */


            /*
            //1. 最简单的使用
            
            //创建任务不启动
            var task1 = new Task(() => { Run1(); });
            
            //创建并启动任务
            var task2 = Task.Factory.StartNew(() => { Run2(); });
            Console.WriteLine("调用start之前****************************\n");

            //调用start之前的“任务状态”
            Console.WriteLine("task1的状态:{0}", task1.Status);
            Console.WriteLine("task2的状态:{0}", task2.Status);
            task1.Start();
            
            //主线程等待任务执行完
            Task.WaitAll(task1, task2);

            Console.WriteLine("\n任务执行完后的状态****************************");
           
            //调用start之前的“任务状态”
            Console.WriteLine("\ntask1的状态:{0}", task1.Status);
            Console.WriteLine("task2的状态:{0}", task2.Status);
            Console.Read();
            */

            /*
            
            //2. 取消任务
            //准备一个取消标记
            var cts = new CancellationTokenSource();
            var ct = cts.Token;

            //传入取消标记
            Task task1 = new Task(() => { Run1(ct); }, ct);

            Task task2 = new Task(Run2);

            try
            {
                //启动任务
                task1.Start();
                task2.Start();

                //模拟运行一段时间后
                Thread.Sleep(1000);

                //传送取消请求
                cts.Cancel();

                //等待任务执行完成
                Task.WaitAll(task1, task2);
            }
            catch (AggregateException ex)
            {
                //捕获异常对象
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("\nhi,我是OperationCanceledException：{0}\n", e.Message);
                }

                //task1是否取消
                Console.WriteLine("task1是不是被取消了？ {0}", task1.IsCanceled);
                Console.WriteLine("task2是不是被取消了？ {0}", task2.IsCanceled);
            }
            Console.Read();
            */

            /*
            
            //3. 获取任务的返回值
           
            //执行task1
            //使用泛型版本 传递参数类型
            var t1 = Task.Factory.StartNew<List<string>>(() => { return Run1(); });

            //等待任务执行完毕
            t1.Wait();

            var t2 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("t1集合中返回的个数：" + string.Join(",", t1.Result));
            });

            Console.Read();
            */

            /*
            //采用ContinueWith方法
            
            //执行task1
            var t1 = Task.Factory.StartNew<List<string>>(() => { return Run1(); });

            //创建一个在目标 System.Threading.Tasks.Task<TResult> 完成时异步执行的延续任务。
            var t2 = t1.ContinueWith((i) =>
            {
                Console.WriteLine("t1集合中返回的个数：" + string.Join(",", i.Result));
            });
            Console.Read();
            */

            /*
            //4：ContinueWith结合WaitAll来玩一把
            ConcurrentStack<int> stack = new ConcurrentStack<int>();
            
            //t1先串行
            var t1 = Task.Factory.StartNew(() =>
            {
                stack.Push(1);
                stack.Push(2);
            });

            //t2,t3并行执行
            var t2 = t1.ContinueWith(t =>
            {
                int result;

                stack.TryPop(out result);
            });

            //t2,t3并行执行
            var t3 = t1.ContinueWith(t =>
            {
                int result;

                stack.TryPop(out result);
            });

            //等待t2和t3执行完
            Task.WaitAll(t2, t3);


            //t4串行执行
            var t4 = Task.Factory.StartNew(() =>
            {
                stack.Push(1);
                stack.Push(2);
            });

            //t5,t6并行执行
            var t5 = t4.ContinueWith(t =>
            {
                int result;

                stack.TryPop(out result);
            });

            //t5,t6并行执行
            var t6 = t4.ContinueWith(t =>
            {
                int result;

                //只弹出，不移除
                stack.TryPeek(out result);
            });

            //临界区：等待t5，t6执行完
            Task.WaitAll(t5, t6);

            //t7串行执行
            var t7 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("当前集合元素个数：" + stack.Count);
            });

            Console.Read();
            */

            //第三天

        }

        public static ConcurrentDictionary<int, Student> LoadData()
        {
            ConcurrentDictionary<int, Student> dic = new ConcurrentDictionary<int, Student>();

            //预加载1500w条记录
            Parallel.For(0, 15000000, (i) =>
            {
                var single = new Student()
                {
                    ID = i,
                    Name = "hxc" + i,
                    Age = i % 151,
                    CreateTime = DateTime.Now.AddSeconds(i)
                };
                dic.TryAdd(i, single);
            });

            return dic;
        }

        /*
        static void Run1(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            Console.WriteLine("我是任务1");
            Thread.Sleep(2000);

            ct.ThrowIfCancellationRequested();

            //你会观察到 这个一句没有输出 表示取消效果已经实现了
            Console.WriteLine("我是任务1的第二部分信息");
        }

        static void Run2()
        {
            Console.WriteLine("我是任务2");
        }
        */

        /*
        static void Run1()
        {
            Thread.Sleep(1000);
            Console.WriteLine("\n我是任务1");
        }

        static void Run2()
        {
            Thread.Sleep(2000);
            Console.WriteLine("我是任务2");
        }
        */

        /*
        private static void StartCode(object i)
        {
            Console.WriteLine("开始执行子线程...{0}", i);
            Thread.Sleep(1000);//模拟代码操作    
            Console.WriteLine("结束执行子线程...{0}", i);
        }
        */

        /*
        static List<string> Run1()
        {
            return new List<string> { "1", "4", "8" };
        }
        */
    }



    public class Student
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime CreateTime { get; set; }
    }
}