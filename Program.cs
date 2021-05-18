using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Async_Flow
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("<<<<<<<<<<<<<<<<<<<<<< moving next within Pending Tasks>>>>>>>>>>>>>>>>>");
            // collection of tasks to be run one after another
            await runPendingtask();
            //custom threads
            Console.WriteLine("<<<<<<<<<<<<<<<<<<<<<< Custom Tasks>>>>>>>>>>>>>>>>>");
            Thread thread = new Thread(new ThreadStart(thread1));
            thread.Start();
            await doJob2();
            //multiple tasks
            Console.WriteLine("<<<<<<<<<<<<<<<<<<<<<< Multiple Tasks>>>>>>>>>>>>>>>>>");
            listOfTasks();
        }


       
        //pending tasks, running through a loop to complete tasks

        static async IAsyncEnumerable<int> pendingTask(int start, int iterator)
        {
            for(int i=0; i<iterator;i++)
            {
                //await Task.Delay(2000);
                if(i>2)
                {
                    Console.WriteLine($"Chill slow down you only needed 2 tasks not:{i}");
                }

                await Task.Run(async () =>
                {
                    //task1
                    await Task.Delay(1000);
                    //task2
                    await Task.Delay(1000);
                });

                yield return start =+i;
            }
        }

        // method to exe pending task
        static async Task runPendingtask()
        {
            IAsyncEnumerator<int> asyncEnumerator = pendingTask(1, 5).GetAsyncEnumerator();

            while (await asyncEnumerator.MoveNextAsync())
            {
                Console.WriteLine($"task being run: {(asyncEnumerator.Current)}");
            }
        }


        //doing custom tasks
        static async Task doJob1()
        {
            await Task.Run(async () => {
                await Task.Delay(1000);
                Console.WriteLine("Go to mall");
                await Task.Delay(2000);
                Console.WriteLine("Buy groceries");
            }

            );
        }
        //same method just using threading instead of task
        static async Task doJob2()
        {
            await Task.Run( () => {
                Thread.Sleep(1000);
                Console.WriteLine("Go to vodacom");
                Thread.Sleep(2000);
                Console.WriteLine("Buy new phone");
            }
            );   
        }

        static void thread1()
        {
            Console.WriteLine("dont forget wallet");
            Thread.Sleep(2000);
        }

        //list of tasks ,doing multiple tasks
        public static void listOfTasks()
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Go to vodacom");
                Thread.Sleep(2000);
                Console.WriteLine("Buy new phone");
            }));
            tasks.Add(Task.Run(async () =>
            {
                await Task.Delay(1000);
                Console.WriteLine("Go to mall");
                await Task.Delay(2000);
                Console.WriteLine("Buy groceries");
            }));

            Task.WaitAll(tasks.ToArray());

        }

    }
}
