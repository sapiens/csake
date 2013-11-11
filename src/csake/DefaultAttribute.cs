using System;

namespace CSake
{
    public class DefaultAttribute : Attribute { }

    public class DependsAttribute : Attribute
    {
        private readonly string[] _taskNames;

        public DependsAttribute(params string[] taskName)
        {
            _taskNames = taskName;
        }

        public string[] TaskNames
        {
            get { return _taskNames; }
        }
    }
}