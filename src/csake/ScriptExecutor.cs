using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CSake.Internals;
using CSScriptLibrary;

namespace CSake
{
    public class ScriptExecutor
    {
        private TasksManager _tasks;

        public ScriptExecutor(string filename)
        {
            var asm = CreateAssembly(filename);
            _tasks = new TasksManager(asm);
        }

        private static Assembly CreateAssembly(string filename)
        {
            var wrapper = new ScriptWrapper();
            string code = "";
            using (var file = new FileReader(filename))
            {
                code = wrapper.Wrap(file);
            }

            var finfo = new FileInfo(filename);
            var workingdir = finfo.DirectoryName;
            var refs = wrapper.ReferencedAssemblies.Select(d =>
            {
                var dll = Path.Combine(workingdir, d);
                if (File.Exists(dll))
                {
                    return dll;
                }
                return d;
            }).ToArray();
            var asm = CSScript.LoadCode(code, refs);
            return asm;
        }

        public void Run(string taskName)
        {
            IExecuteTask task;
            if (taskName.IsNullOrEmpty())
            {
                task = _tasks.GetDefaultTask();
            }
            else
            {
                task = _tasks.GetTask(taskName);
            }
            var list = new List<ITaskExecuted>();
            foreach (var dep in task.Dependencies)
            {
                "Executing '{0}'".WriteInfo(dep.Name);
                dep.Run();
                list.Add(dep);
                Console.WriteLine();
            }
            
            "Executing '{0}'".WriteInfo(task.Name);
            task.Run();
            Console.WriteLine();
            list.Add(task);
            Timings = list;
        }

        public IEnumerable<ITaskExecuted> Timings
        {
            get; private set;
        }
    }
}