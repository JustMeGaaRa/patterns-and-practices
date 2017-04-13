# Check if nuget.exe is present
if (!(Test-Path .\nuget.exe)) {
    wget "https://dist.nuget.org/win-x86-commandline/v4.1.0/nuget.exe" -outfile .\nuget.exe
}

$configuration = "Release"
$rootPath = (Resolve-Path .).Path
$artifactsPath = Join-Path $rootPath "artifacts"
$nupkgsPath = Join-Path $artifactsPath "nupkgs"

# Getting all directories with library projects excluding tests and samples
$projs = dir -Directory $rootPath -Include "*Silent.Practices*" -Exclude "*Test*" | Select -ExpandProperty FullName

New-Item -ItemType Directory -Force -Path $artifactsPath

# Packing the project based on *.csproj for solution level
foreach ($proj in $projs) {
	Write-Host "Packing $proj" -Foreground Green
	dotnet pack $proj --output $nupkgsPath --configuration $configuration
}

# Publish all *.nupkg files to nuget gallery
$nupkgs = dir -File $nupkgsPath | Select -ExpandProperty FullName

foreach ($nupkg in $nupkgs) {
	Write-Host "Publish $nupkg" -Foreground Green
	dotnet nuget push $nupkg --source https://www.nuget.org/api/v2/package
}
