#Список файлов необходимых ддя работы cake-скрипта
$CakeFiles = @("build.cake", "Settings.json", "CakeSettingsHelper.dll")
#дирректория с cake-скриптом
$CakeDir = "cake"

#Проверка на существование дирректории с cake
$DirExists = Test-Path -Path $CakeDir

if($DirExists -eq "True")
{
    Set-Location $CakeDir

    #проверка на наличие необходимых файлов
    foreach ($Cakefile in $CakeFiles) 
    {
        $FileExists = Test-Path -Path $CakeFile
        if($FileExists -ne "True")
        {
            throw $CakeFile + " не найден!!!" 
            exit 1
        }
    }

    #Скачиваем build.ps1 для cake-скрипта
    Invoke-WebRequest https://cakebuild.net/download/bootstrapper/windows -OutFile build.ps1    
}
else 
{
    throw $CakeDir + " не обнаружена!!!" 
    exit 1
}
