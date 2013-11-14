using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using CSScriptLibrary;

namespace CSake.Internals
{
    class TaskExecutor : IExecuteTask
    {
        private readonly AsmHelper _invoker;

        public TaskExecutor(string name,AsmHelper invoker,params IExecuteTask[] deps)
        {
            _invoker = invoker;
            Name = name;
            Dependencies = deps;
        }

        public void Run()
        {
            foreach (var dep in Dependencies)
            {
                dep.Run();
            }
            var sw = new Stopwatch();
            "Executing '{0}'".WriteInfo(Name);
            sw.Start();
            try
            {
                _invoker.Invoke(string.Format("{0}.{1}", ScriptWrapper.ClassName, Name));
                sw.Stop();
                Console.WriteLine();
                TimeTaken = sw.Elapsed;
            }
            catch (ReflectionTypeLoadException ex)
            {
                foreach (var innerex in ex.LoaderExceptions)
                {
                    innerex.Message.WriteError();
                }
                throw;
            }
            
        }

        public string Name { get; private set; }

        public IEnumerable<IExecuteTask> Dependencies { get; private set; }
        public TimeSpan TimeTaken { get; private set; }
    }
}