using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;

namespace CSake
{
    public static class Extensions
    {
        /// <summary>
        /// Equivalent to Console.WriteLine(mystring,args)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="args"></param>
        public static void ToConsole(this string data, params object[] args)
        {
            Console.WriteLine(data, args);
        }

        /// <summary>
        /// Writes to console using red colour
        /// </summary>
        /// <param name="data"></param>
        /// <param name="args"></param>
        public static void WriteError(this string data, params object[] args)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(data, args);
            Console.ForegroundColor = old;
        }

        /// <summary>
        /// Writes to console using cyan colour
        /// </summary>
        /// <param name="data"></param>
        /// <param name="args"></param>
        public static void WriteInfo(this string data, params object[] args)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(data, args);
            Console.ForegroundColor = old;
        }

        /// <summary>
        /// Creates a directory
        /// </summary>
        /// <param name="dirName">Directory path</param>
        public static void DirCreate(this string dirName)
        {
            dirName.MustNotBeEmpty();
            Directory.CreateDirectory(dirName);
        }

        /// <summary>
        /// Deletes the specified directory
        /// </summary>
        /// <param name="dirName">Directory path</param>
        public static void DirDelete(this string dirName)
        {
            dirName.MustNotBeEmpty();
            Directory.Delete(dirName, true);
        }

        /// <summary>
        /// Empties the specified directory
        /// </summary>
        /// <param name="dirName">Directory path</param>
        public static void DirCleanup(this string dirName)
        {
            dirName.MustNotBeEmpty();
            Directory.Delete(dirName, true);
            Directory.CreateDirectory(dirName);
        }

        /// <summary>
        /// Run executable with specified arguments. 
        /// No new window is created and all output goes to console.
        /// </summary>
        /// <param name="file">Executable name</param>
        /// <param name="args">Arguments list</param>
        /// <returns>Process exit code</returns>
        public static int Exec(this string file, params string[] args)
        {
            using (var p = new Process())
            {
                p.StartInfo.FileName = file;
                if (args.Length > 0)
                {
                    p.StartInfo.Arguments = string.Join(" ", args);
                }
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();
                Console.WriteLine(p.StandardOutput.ReadToEnd());
                p.WaitForExit();
                return p.ExitCode; 
            }           
        }

        /// <summary>
        /// Invokes MSBuild.exe with Clean target and normal verbosity
        /// </summary>
        /// <param name="project">Project/solution file</param>
        /// <param name="configuration">Default is "Release"</param>
        public static void MsBuildClean(this string project, string configuration="Release")
        {
            var builder = new MsBuild(project, new NameValueCollection() {{"Configuration", configuration}});
            builder.Clean();
        }

        /// <summary>
        /// Invokes MSBuild.exe with Build target, Release configuration and no verbosity
        /// </summary>
        /// <param name="project">Project/solution file</param>
        public static void MsBuildRelease(this string project)
        {
            var builder = new MsBuild(project, MsBuild.ConfigurationRelease);
            builder.Build();
        }

        
}
}