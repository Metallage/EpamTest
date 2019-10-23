

#tool nuget:?package=NUnit.ConsoleRunner&version=3.6.0
#r CakeSettingsHelper.dll
using CakeSettingsHelper;


var settingsFile = @"Settings.json"; //Здесь храним настройки
var configuration = Argument("configuration", "Debug");


var projectOutput=""; //Сюда складываем артефакты сборки

//Загружаем настройки
Task("LoadConfig")
.Does(()=>{
   SettingsHelper mysettings = new SettingsHelper(settingsFile);
   projectOutput = mysettings.GetSettingValue("OutputDir"); //Парсим файл настроек для получения директории для артефактов
    if(projectOutput == null)
    {
        throw new Exception("Output directory error"); //если не парсится то что-то не так
    }
    else
    {
        Information("Output directory is: {0}", projectOutput);
    }

});

//Отчищаем директорию артефактов от прежних сборок
Task("Clean")
.IsDependentOn("LoadConfig")
.Does(()=>{
    EnsureDirectoryExists(projectOutput);
    CleanDirectory(projectOutput);
});

//Востанавливаем нугеты
Task("RestoreNugets")
.IsDependentOn("Clean")
.Does(() =>{
    NuGetRestore(@"..\project\Pacman.sln");
});

//Собираем проект
Task("Build")
.IsDependentOn("RestoreNugets")
.Does(() =>{
    MSBuild(@"..\project\Pacman.sln", x => x
    .SetConfiguration(configuration)
    .SetVerbosity(Verbosity.Minimal)
    .WithTarget("build")
    .WithProperty("TreatWarningsAsErrors", "false"));

});

//Проганяем на тесты
Task("RunTests")
.IsDependentOn("Build")
.Does(()=>{
 NUnit3(@"..\project\UnitTests\bin\" + configuration + @"\UnitTests.dll");
});

//Копируем артефакты сборки в выходную директорию
Task("Output")
.IsDependentOn("RunTests")
.Does(()=>{
    CopyFiles(@"..\project\Pacman\bin\" + configuration + @"\*.dll", projectOutput);
    CopyFiles(@"..\project\Pacman\bin\" + configuration + @"\*.pdb", projectOutput);
    CopyFiles(@"..\project\Pacman\bin\" + configuration + @"\*.exe", projectOutput);
    CopyFiles(@"..\project\Pacman\bin\" + configuration + @"\*.config", projectOutput);
});
RunTarget("Output");