using System;
using System.Collections.Generic;

namespace CSake.Internals
{
    public interface ITaskExecuted
    {
        string Name { get; }
        TimeSpan TimeTaken { get; }
    }

    public interface IExecuteTask : ITaskExecuted
    {
        /// <summary>
        /// Doesn't run dependencies
        /// </summary>
        void Run();

        IEnumerable<IExecuteTask> Dependencies { get; }
    }
}