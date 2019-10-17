
$CakeFile ="build.cake"
$IsExists = Test-Path -Path $CakeFile

if($IsExists -ne "True")
{
   # Write-Output "${CakeFile} не найден!!!"
    trow "${CakeFile} не найден!!!" 
    exit 1
}
else 
{
    Set-Location $CakeDir
    Invoke-WebRequest https://cakebuild.net/download/bootstrapper/windows -OutFile build.ps1    
}