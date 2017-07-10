using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadEventDemo01
{
    // TODO: Add comments.
    //
    // Program flow:
    //
    // 1. Start two threads.
    //
    // 2. Thread 1 monitors the file system to look out for a file. It operates in a loop.
    //
    // 2.1 When the file is found, m_event_file_arrived is set. Thread 1 breaks out of loop.
    //
    // 2.2 Else, Thread 1 blocks for 1 second and the loop reiterates.
    //
    // 3. Thread 2 waits on the event to be set. Once event is set, it reads from the file
    // and displays its contents on the console.
    class Program
    {
        static string filepath = @"D:\David\test\my_file.txt";

        static void Main(string[] args)
        {
            StartThread01();
            StartThread02();
            WaitThread01ToEnd();
            WaitThread02ToEnd();
        }

        static void StartThread01()
        {
            Console.WriteLine("Waiting for file...");

            while (true)
            {
                if (!File.Exists(filepath))
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    m_event_file_arrived.Set();
                    break;
                }
            }
        }

        static void StartThread02()
        {
            m_event_file_arrived.WaitOne();
            m_event_file_arrived.Reset();
            string str_FileContents = File.ReadAllText(filepath);
            Console.WriteLine(str_FileContents);
        }

        static void WaitThread01ToEnd()
        {
            m_thread_01.Join();
        }

        static void WaitThread02ToEnd()
        {
            m_thread_02.Join();
        }

        private static Thread m_thread_01 = null;
        private static Thread m_thread_02 = null;
        private static ManualResetEvent m_event_file_arrived = new ManualResetEvent(false);
    }
}
