using System.Collections.Generic;
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
            foreach(var task in Dependencies) task.Run();
            _invoker.Invoke(string.Format("{0}.{1}", ScriptWrapper.ClassName, Name));
        }

        public string Name { get; private set; }

        public IEnumerable<IExecuteTask> Dependencies { get; private set; }
    }
}