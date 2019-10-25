$CakeFiles = @("build.cake", "Settings.json", "CakeSettingsHelper.dll", "CakeSettingsHelper.deps.json")
$CakeDir = "cake"

$DirExists = Test-Path -Path $CakeDir

if($DirExists -eq "True")
{
    Set-Location $CakeDir

    foreach ($Cakefile in $CakeFiles) 
    {
        $FileExists = Test-Path -Path $CakeFile
        if($FileExists -ne "True")
        {
            trow "${CakeFile} не найден!!!" 
            exit 1
        }
    }

    Invoke-WebRequest https://cakebuild.net/download/bootstrapper/windows -OutFile build.ps1    
}
else 
{
    throw "${CakeDir} не обнаружена!!!" 
    exit 1
}
