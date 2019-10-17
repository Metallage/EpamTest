

#tool nuget:?package=NUnit.ConsoleRunner&version=3.6.0
#r CakeSettingsHelper.dll
using CakeSettingsHelper


var settingsFile = "set.json";
var configuration = Argument("configuration", "Debug");


var projectOutput="";

//var buildDir = Directory(projectOutput) + Directory("/Build/");
//var outDir = Directory(projectOutput) + Directory("/Publish/");

Task("LoadConfig")
.Does(()=>{
   SettingsHelper mysettings = new SettingsHelper("set.json");
   projectOutput = mysettings.GetSettingValue("OutputDir");
    if(projectOutput == null)
    {
        throw new Exception("Output directory error");
    }
    else
    {
        Information("Output directory is: {0}", projectOutput);
    }

});

Task("Clean")
.IsDependentOn("LoadConfig")
.Does(()=>{
    EnsureDirectoryExists(projectOutput);
    CleanDirectory(projectOutput);
});

Task("RestoreNugets")
.IsDependentOn("Clean")
.Does(() =>{
    NuGetRestore("Pacman.sln");
});

Task("Build")
.IsDependentOn("RestoreNugets")
.Does(() =>{
    MSBuild("Pacman.sln", x => x
    .SetConfiguration(configuration)
    .SetVerbosity(Verbosity.Minimal)
    .WithTarget("build")
    .WithProperty("TreatWarningsAsErrors", "false"));

});

Task("RunTests")
.IsDependentOn("Build")
.Does(()=>{
 NUnit3(@"UnitTests\bin\" + configuration + @"\UnitTests.dll");
});

Task("Output")
.IsDependentOn("RunTests")
.Does(()=>{
    CopyFiles(@"Pacman\bin\" + configuration + @"\*.dll", projectOutput);
    CopyFiles(@"Pacman\bin\" + configuration + @"\*.pdb", projectOutput);
    CopyFiles(@"Pacman\bin\" + configuration + @"\*.exe", projectOutput);
    CopyFiles(@"Pacman\bin\" + configuration + @"\*.config", projectOutput);
});
RunTarget("Output");