using System;
using System.IO;
using CSake;
using NuGet;

const string SlnFile = @"../src/csake.sln";

const string ReleaseDir = @"../src/csake/bin/Release";

const string CSakeDir = @"../src/csake";

const string PackageDir = @"temp/package";

const string TempDir = @"temp";

static string CurrentDir = Path.GetFullPath("./");

public static void CleanUp()
{
    TempDir.CleanupDir();
    SlnFile.MsBuildClean();
}

[Depends("CleanUp")]
public static void Build()
{
    SlnFile.MsBuildRelease();
}

[Default]
[Depends("Build")]
public static void Nuget()
{
    PackageDir.MkDir();
    var nuspecFile=Path.Combine(CurrentDir,"csake.nuspec");
    UpdateVersion(nuspecFile,"csake.exe");
    BuildNuget("csake",CSakeDir);
}

//basePath= relative path for package files source. Usually is the project dir
static void BuildNuget(string nuspecFile,string basePath)
{
    if (!nuspecFile.EndsWith(".nuspec"))
    {
        nuspecFile+=".nuspec";
    }
    Path.Combine(TempDir,nuspecFile).CreateNuget(basePath,PackageDir);    
}

static void UpdateVersion(string nuspecFile,string assemblyName)
{
    var nuspec=nuspecFile.AsNuspec();
   
    nuspec.Metadata.Version=GetVersion(assemblyName);
    nuspec.Save(TempDir);    
}

static string GetVersion(string asmName)
{
   return Path.Combine(ReleaseDir,asmName).GetAssemblyVersion().ToSemanticVersion().ToString();
}

