using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text;
using System.Xml;
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

            Manifest om;
            using (var ms = new MemoryStream())
            {
                var buffer = UnicodeEncoding.Default.GetBytes(data);
                ms.Write(buffer,0,buffer.Length);
                ms.Seek(0, SeekOrigin.Begin);
                om = Manifest.ReadFrom(ms,false);
            }
            

            
            var manifest = new Manifest();
            manifest.Metadata.Id = "haa";
            manifest.Metadata.Version = "1.0.0";
            manifest.Metadata.Authors = "bla";
            manifest.Metadata.Description = "desc";
            manifest.Files=new List<ManifestFile>();
            manifest.Files.Add(new ManifestFile()
            {
                Source = "bla",Target="lib"
            });

            string result = "";
            using (var m = new MemoryStream())
                {
                    manifest.Save(m);
                    m.Seek(0, SeekOrigin.Begin);
                    result=m.ReadAsString();
                }
                
            Write(result);
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}