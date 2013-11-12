using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CSake.Internals;

namespace CSake
{
    class Program
    {
        static void WriteCSakeHeader()
        {
            var version = Assembly.GetExecutingAssembly().Version();
            "CSake (C# make) version {0}".ToConsole(string.Format("{0}.{1}.{2}",version.Major,version.Minor,version.Build));
            "Copyright (c) 2013 Mihai Mogosanu".ToConsole();
            Console.WriteLine();
        }
        static int Main(string[] args)
        {
            WriteCSakeHeader();

            if (args.Length == 0)
            {
                "Usage: csake script_name [task_name]".ToConsole();
                return 0;
            }

            var file = args[0];
            if (!File.Exists(file))
            {
                "File '{0}' couldn't be found".WriteError(file);
                return 1;
            }

            string task = "";
            if (args.Length > 1)
            {
                task = args[1];
            }

            try
            {
                var executor = new ScriptExecutor(file);
                executor.Run(task);
                WriteTimes(executor.Timings);
            }
            catch (Exception ex)
            {
                ex.ToString().WriteError();
                return 2;
            }
            return 0;
        }

        static void WriteTimes(IEnumerable<ITaskExecuted> times)
        {
            @"
---------------------------------------------
Build Time Report
---------------------------------------------".ToConsole();
            var longest = times.OrderByDescending(t => t.Name.Length).First().Name.Length;
            longest = Math.Max("Name".Length, longest);
            Console.WriteLine("{0,-"+longest+"}  {1}","Name","Duration");
            Console.WriteLine("{0,-"+longest+"}  {1}","----","--------");
            TimeSpan total=new TimeSpan();
            
            foreach (var time in times)
            {
                Console.WriteLine("{0,-"+longest+"}  {1}",time.Name,time.TimeTaken.ToString());
                total += time.TimeTaken;
            }
            var old = Console.ForegroundColor;
            Console.ForegroundColor=ConsoleColor.Magenta;
            Console.WriteLine("{0}  {1}","Total:".PadRight(longest),total);
            Console.ForegroundColor = old;
            Console.WriteLine();
        }
    }
}
