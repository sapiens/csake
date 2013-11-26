﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSScriptLibrary;

namespace CSake.Internals
{
    public class TasksManager
    {
        private Assembly _asm;
        private IEnumerable<MethodInfo> _methods;
      //  private AsmHelper _helper;

        public TasksManager(Assembly asm)
        {
            _asm = asm;
            var wrapper = _asm.GetType(ScriptWrapper.ClassName);
            _methods =
                wrapper.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly |
                                   BindingFlags.IgnoreCase).Where(m=>m.GetParameters().Length==0);
            
        }

        public IExecuteTask GetTask(string name)
        {
            var method = _methods.FirstOrDefault(d => d.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (method == null)
            {
                throw new InvalidOperationException("There's no task named {0}".ToFormat(name));
            }
            var deps = method.GetSingleAttribute<DependsAttribute>();
            IExecuteTask[] depTasks=new IExecuteTask[0];
            if (deps != null && deps.TaskNames.Length>0)
            {
                //depTasks = deps.TaskNames.Select(d => CreateSimpleTask(d)).ToArray();
                depTasks = deps.TaskNames.Select(GetTask).ToArray();
            }
            
            return new TaskExecutor(method.Name,_asm,depTasks);
        }

        //IExecuteTask CreateSimpleTask(string name)
        //{
        //    return new TaskExecutor(name, _helper);
        //}

        public IExecuteTask GetDefaultTask()
        {
            var method = _methods.FirstOrDefault(d => d.HasCustomAttribute<DefaultAttribute>());
            if (method == null)
            {
                throw new NoDefaultTaskDefinedException();
            }
            return GetTask(method.Name);
        }

        public IEnumerable<string> GetTasksNames()
        {
            
            return _methods.Select(m=>m.Name);
        }
    }
}