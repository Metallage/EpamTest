$CakeDir = "cake"
$CakeFile = $CakeDir + "\build.cake"
$IsExists = Test-Path -Path $CakeFile

if($IsExists -ne "True")
{
   # Write-Output "${CakeFile} не найден!!!"
   # ThrowError -ExceptionName "FileNotFound" -ExceptionMessage "${CakeFile} не найден!!!" -errorId 
    Write-Error "${CakeFile} не найден!!!"
}
else 
{
    Set-Location $CakeDir
    Invoke-WebRequest https://cakebuild.net/download/bootstrapper/windows -OutFile build.ps1    
}