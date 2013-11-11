using System.Collections.Generic;

namespace CSake.Internals
{
    public interface IExecuteTask
    {
        /// <summary>
        /// Runs dependencies then task
        /// </summary>
        void Run();
        string Name { get; }
        IEnumerable<IExecuteTask> Dependencies { get; }
    }
}