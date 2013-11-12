using System.Linq;
using System.Reflection;
using CSake.Internals;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace Tests
{
    public class TasksTests
    {
        private Stopwatch _t = new Stopwatch();
        private TasksManager _sut;

        public TasksTests()
        {
            _sut = new TasksManager(BuildAssembly());
        }

        [Fact]
        public void identify_tasks()
        {
            var names = _sut.GetTasksNames();
            names.Should().Contain("CleanUp");
            names.Should().Contain("Build");
            names.Should().NotContain("MyUtility");
        }

        [Fact]
        public void task_name_are_case_insensitive()
        {
            var task=_sut.GetTask("build");
            task.Should().NotBeNull();
            task.Run();
        }

        [Fact]
        public void build_has_cleanup_dependency()
        {
            var task = _sut.GetTask("build");
            task.Dependencies.Any(t => t.Name == "CleanUp").Should().BeTrue();
        }

        [Fact]
        public void default_task_is_build()
        {
            var task = _sut.GetDefaultTask();
            task.Name.Should().Be("Build");
        }

        Assembly BuildAssembly()
        {
            var code = @"
using System;
using CSake;

class Hello{}

const int Count=23;

public static void CleanUp()
{
  ""Cleanup {0}"".ToConsole(Count);
}

public static void MyUtility(int f)
{}

[Default]
[Depends(""CleanUp"")]
public static void Build()
{
   ""Build"".ToConsole();
}
";
            var wrapper = new ScriptWrapper();
            return CSScriptLibrary.CSScript.LoadCode(wrapper.Wrap(new CodeBlockReader(code)));
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}