$rootPath = (Resolve-Path ".\").Path
$nupkgsPath = Join-Path $rootPath "artifacts\packages"
$remotePath = "https://www.nuget.org/api/v2/package"

# Publish all *.nupkg files to nuget gallery
$nupkgs = dir -File $nupkgsPath | Select -ExpandProperty FullName

foreach ($nupkg in $nupkgs) {
	Write-Host "Publish $nupkg" -Foreground Green
	dotnet nuget push $nupkg --source $remotePath
}
