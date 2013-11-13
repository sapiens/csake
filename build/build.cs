using System;
using System.IO;
using CSake;
using NuGet;

const string SlnFile=@"../src/csake.sln";

const string ReleaseDir=@"../src/csake/bin/Release";

const string CSakeDir=@"../src/csake";

const string PackageDir=@"temp/package";

const string NugetBuildDir=@"temp/nuget";

const string TempDir=@"temp";

static string CurrentDir=Path.GetFullPath("./");

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
    NugetBuildDir.MkDir();
      
    var nuspecFile=Path.Combine(CurrentDir,"csake.nuspec");
    UpdateVersion(nuspecFile);
    Path.Combine(NugetBuildDir,"csake.nuspec").CreateNuget(CSakeDir,PackageDir);    
}

static void UpdateVersion(string nuspecFile)
{
    var nuspec=nuspecFile.AsNuspec();
   
    nuspec.Metadata.Version=GetVersion("csake.exe");
    nuspec.Save(NugetBuildDir);    
}

static string GetVersion(string asmName)
{
   return Path.Combine(ReleaseDir,asmName).GetAssemblyVersion().ToSemanticVersion().ToString();
}

