using System.Linq;
using System.Reflection;
using CSake.Internals;
using CSScriptLibrary;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace Tests
{
    public class ScriptWrapperTests
    {
        private Stopwatch _t = new Stopwatch();
        private ScriptWrapper _sut;

        public ScriptWrapperTests()
        {
            _sut = new ScriptWrapper();
        }

        [Fact]
        public void wrap_a_bunch_of_methods()
        {
            var code = @"
using System;
using Xunit;

    //something
    [Default]
    public static void Clean()
    {
    }
";
            var result = _sut.Wrap(new CodeBlockReader(code));
            result.Should().Be(@"
using System;
using Xunit;

public class CSakeWrapper{
[Default]
public static void Clean()
{
}

}");
        }

        [Fact]
        public void wrapped_code_is_loaded_successfuly()
        {
            var code = @"
using System;
using Xunit;
using CSake;

const int Data=23;
    //something
    [Default]
    public static void Clean()
    {
        var j=Data+34;
    }

    class MyClass {
    public string Name {get;set;}

    }
";
            var wrapped = _sut.Wrap(new CodeBlockReader(code));
            Assembly asm = null;
            Assert.DoesNotThrow(() =>
            {
                asm = CSScript.LoadCode(wrapped);
            });
            asm.GetTypes().Count().Should().Be(2);
        }

        [Fact]
        public void identify_referenced_assemblies()
        {
            var code = @"
#r ""my.dll"";
using System;
using Xunit;

    //something
    [Default]
    public static void Clean()
    {
    }
";
            var result = _sut.Wrap(new CodeBlockReader(code));
            result.Should().Be(@"
using System;
using Xunit;

public class CSakeWrapper{
[Default]
public static void Clean()
{
}

}");
            _sut.ReferencedAssemblies.Should().Contain("my.dll");
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}