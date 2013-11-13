using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CSake.Internals;
using CSScriptLibrary;


namespace CSake
{
    public class ScriptExecutor:IDisposable
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
            Current.Script = finfo;
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
            var l = new List<string>(refs);
            l.Add("NuGet.Core.dll");
            l.Add("System.ComponentModel.DataAnnotations.dll");
            var asm = CSScript.LoadCode(code, l.ToArray());
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
            task.Run();
            FillTimings(list,task);
            Timings = list;
            Current.Script = null;
        }

        static void FillTimings(List<ITaskExecuted> result, IExecuteTask current)
        {
            foreach (var dep in current.Dependencies)
            {
                FillTimings(result,dep);
            }
            result.Add(current);
        }

        public IEnumerable<ITaskExecuted> Timings
        {
            get; private set;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}