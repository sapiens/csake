using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using CSScriptLibrary;

namespace CSake.Internals
{
   class TaskExecutor : IExecuteTask
    {
       private readonly Assembly _asm;
       private readonly AsmHelper _invoker;

        public TaskExecutor(string name,Assembly asm,params IExecuteTask[] deps)
        {
            _asm = asm;

            _invoker = new AsmHelper(asm);
            Name = name;
            Dependencies = deps;
        }

       bool Skip()
       {

           var info = _asm.GetType(ScriptWrapper.ClassName).GetMethod(Name,BindingFlags.Static|BindingFlags.Public);
           info.Name.ToString().ToConsole();
            var skip = info.GetSingleAttribute<SkipIfAttribute>();
            if (skip != null)
            {
                if (skip.Always) return true;
                var field=info.DeclaringType.GetField(skip.FieldName, BindingFlags.DeclaredOnly|BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Static);
                var value = field.GetValue(null);
                if (skip.FieldValue != null)
                {
                    if (value == null) return false;
                    return skip.FieldValue.ToString() == value.ToString();
                }
            }
           return false;
       }

        public void Run()
        {
            var methodName = string.Format("{0}.{1}", ScriptWrapper.ClassName, Name);

            if (Skip())
            {
                "Skipping task {0}".WriteInfo(Name);
                return;
            }

            foreach (var dep in Dependencies)
            {
                dep.Run();
            }
            var sw = new Stopwatch();
            "Executing '{0}'".WriteInfo(Name);
            sw.Start();
            try
            {
                
                _invoker.Invoke(methodName);
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