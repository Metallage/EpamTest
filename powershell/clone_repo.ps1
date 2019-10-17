$ProjectDir = "pacman"
$ProjectGit = "https://github.com/adrianeyre/pacman.git"

$ForClean = Test-Path -Path $ProjectDir
if($ForClean -eq "True")
{   
    Write-Output "Cleaning local repo..."
    Remove-Item -Path $ProjectDir -Recurse -Force
    Write-Output "Clean complete"
}

git clone $ProjectGit