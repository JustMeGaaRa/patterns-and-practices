# Check if nuget.exe is present
#if (!(Test-Path .\nuget.exe)) {
#    wget "https://dist.nuget.org/win-x86-commandline/v4.1.0/nuget.exe" -outfile .\nuget.exe
#}

$configuration = "Release"
$rootPath = (Resolve-Path "..\").Path
$srcPath = Join-Path $rootPath "src"
$nupkgsPath = Join-Path $rootPath "artifacts\nupkgs"

# Getting all directories with library projects excluding tests and samples
$projs = dir -Directory $srcPath | Select -ExpandProperty FullName

New-Item -ItemType Directory -Force -Path $nupkgsPath

dotnet restore $srcPath

# Packing the project based on *.csproj for solution level
foreach ($proj in $projs) {
	Write-Host "Packing $proj" -Foreground Green
	dotnet pack $proj --output $nupkgsPath --configuration $configuration
}

