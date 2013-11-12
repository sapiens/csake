using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var sw = new Stopwatch();
            sw.Start();
            _invoker.Invoke(string.Format("{0}.{1}", ScriptWrapper.ClassName, Name));
            sw.Stop();
            TimeTaken = sw.Elapsed;
        }

        public string Name { get; private set; }

        public IEnumerable<IExecuteTask> Dependencies { get; private set; }
        public TimeSpan TimeTaken { get; private set; }
    }
}