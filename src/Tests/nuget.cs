using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text;
using System.Xml;
using CSake;
using NuGet;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace Tests
{
    public class nuget
    {
        private Stopwatch _t = new Stopwatch();

        public nuget()
        {

        }
        const string NuspecFile=@"../../test.nuspec";
        [Fact]
        public void test()
        {
            var data = @"<?xml version=""1.0""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"">
  <metadata>
    <id>CSake</id>
    <version>1.0.0</version>
    <title>CSake</title>
    <authors>Mihai Mogosanu</authors>
    <owners>Mihai Mogosanu</owners>
    <projectUrl>https://github.com/sapiens/CSake</projectUrl>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>CSake - C# Make - is a straightforward build automation tool for .Net . Tasks and dependencies are defined using C# static methods</description>
    <language>en-US</language>
    <tags>make build</tags>
    <dependencies>
   <dependency id=""CS-Script"" version=""3.6.7"" />
  </dependencies>

	</metadata>
<files>
        <file src=""..\..\src\csake\bin\Release\**\csake*"" target=""lib""></file>
        
    </files>
</package>";

            var nuspec = new NuSpecFile(NuspecFile);
            nuspec.AddDependency("bla","123");
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}