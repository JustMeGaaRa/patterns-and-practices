$nuget = ".\nuget.exe"
$nugetUrl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"

if(!(Test-Path $nuget)) {
    Invoke-WebRequest -Uri $nugetUrl -OutFile $nuget
}

$rootPath = (Resolve-Path ".\").Path
$nupkgsPath = Join-Path $rootPath "artifacts\packages"
$localPath = Join-Path $rootPath "libs\packages"

# Publish all *.nupkg files to nuget gallery
Write-Host "Publish all *.nupkg" -Foreground Green
& $nuget init $nupkgsPath $localPath
