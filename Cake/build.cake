

#tool nuget:?package=NUnit.ConsoleRunner&version=3.6.0
#r CakeSettingsHelper.dll
using CakeSettingsHelper


var settingsFile = @"Settings.json";
var configuration = Argument("configuration", "Debug");


var projectOutput="";

//var buildDir = Directory(projectOutput) + Directory("/Build/");
//var outDir = Directory(projectOutput) + Directory("/Publish/");

Task("LoadConfig")
.Does(()=>{
   SettingsHelper mysettings = new SettingsHelper(settingsFile);
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
    NuGetRestore(@"..\project\Pacman.sln");
});

Task("Build")
.IsDependentOn("RestoreNugets")
.Does(() =>{
    MSBuild(@"..\project\Pacman.sln", x => x
    .SetConfiguration(configuration)
    .SetVerbosity(Verbosity.Minimal)
    .WithTarget("build")
    .WithProperty("TreatWarningsAsErrors", "false"));

});

Task("RunTests")
.IsDependentOn("Build")
.Does(()=>{
 NUnit3(@"EpamTest\project\UnitTests\bin\" + configuration + @"\UnitTests.dll");
});

Task("Output")
.IsDependentOn("RunTests")
.Does(()=>{
    CopyFiles(@"..\project\Pacman\bin\" + configuration + @"\*.dll", projectOutput);
    CopyFiles(@"..\project\Pacman\bin\" + configuration + @"\*.pdb", projectOutput);
    CopyFiles(@"..\project\Pacman\bin\" + configuration + @"\*.exe", projectOutput);
    CopyFiles(@"..\project\Pacman\bin\" + configuration + @"\*.config", projectOutput);
});
RunTarget("Output");