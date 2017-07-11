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
            m_thread_01 = new Thread(new ThreadStart(WorkerThreadMethod01));
            m_thread_01.Start();
        }

        static void StartThread02()
        {
            m_thread_02 = new Thread(new ThreadStart(WorkerThreadMethod02));
            m_thread_02.Start();
        }

        // This method represents the entry-point for Thread 01.
        //
        // Thread 01 monitors the file system for the existence of a
        // file. 
        //
        // In a while-loop, it checks for the existence of the
        // file. If the file is not found, the thread goes to
        // sleep for 1 second before the loop reiterates.
        //
        // If the file is found, the thread sets the m_event_file_arrived
        // event to signalled state and breaks out of the loop.
        static void WorkerThreadMethod01()
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

        // This method represents the entry-point for Thread 02.
        //
        // Thread 02 reads from the specified file upon its discovery.
        //
        // Firstly, it waits for the m_event_file_arrived event to be
        // signalled (i.e. when the file is found on the system).
        //
        // Once the event is signalled, the thread reads from the file
        // and displays its contents on the console.
        static void WorkerThreadMethod02()
        {
            // Much like Mutexes, the ManualResetEvent class
            // has a WaitOne() method. This method blocks the
            // calling thread until the event is set to
            // signalled state.
            m_event_file_arrived.WaitOne();

            // Note: While not absolutely necessary, it is a recommended 
            // practice to reset an event to non-signalled state after
            // waiting on them.
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

        // A ManualResetEvent object that signifies the arrival of a 
        // particular file.
        //
        // It is initially set to default (non-signalled) state.
        private static ManualResetEvent m_event_file_arrived = new ManualResetEvent(false);
    }
}
