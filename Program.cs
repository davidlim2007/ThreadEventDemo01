using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadEventDemo01
{
    // This program starts 2 simple threads.
    // And then waits for the threads to end.
    // The purpose of this program is to demonstrate
    // the use of the ManualResetEvent class to
    // wait for an event.
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
        private static ManualResetEvent m_event_file_arrived = null;
    }
}
