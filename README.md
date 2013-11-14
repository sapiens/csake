##C# Make

Use C# in your build scripts.

##License

Apache license 2.0. 

CSake uses [CS Script engine](http://www.csscript.net/) to execute scripts


#Getting Started

Get it from [Nuget](https://www.nuget.org/packages/CSake/).
Bundled with it (in packages/csake1.0.0/tools) you can find 2 quick start files that you can copy (remove the '-sample' suffix from extension) and use.

Here how's a build script looks (this is the CSake build script).

```csharp

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


````

#Rules

* No namespaces!
* Any public static function with no arguments is considered to be a task.
* Task dependencies are given prefixing the task with [Depends("task1","task2")].
* There can be only one Default task!
* You either have to specify a task or have a default task.
* To reference other assemblies use the "#r" directive before the 'using' statements, one directive for each assembly.
 ```csharp

	#r "myassembly.dll"

 ````
* By default the following assemblies are referenced
 * System.dll
 * System.Xml.dll
 * NuGet.Core.dll
 * System.ComponentModel.DataAnnotations.dll (required by Nuget)
 * [CavemanTools.dll](https://bitbucket.org/sapiensworks/caveman-tools/wiki/CTools)
* Loading other scripts is not supported at the moment

Check all included **[Helpers](https://github.com/sapiens/csake/wiki/Helpers)**


