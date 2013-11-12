using System;
using CSake;

const string SlnFile=@"..\csake.sln";

public static void CleanUp()
{
    SlnFile.MsBuildClean();     
}

[Default]
[Depends("CleanUp")]
public static void Build()
{
        
}


