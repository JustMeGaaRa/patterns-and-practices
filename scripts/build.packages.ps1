$configuration = "Release"
$rootPath = (Resolve-Path ".\").Path
$sourcePath = Join-Path $rootPath "src"
$nupkgsPath = Join-Path $rootPath "artifacts\packages"

# Getting all directories with library projects excluding tests and samples
$projs = dir -Directory $sourcePath | Select -ExpandProperty FullName

New-Item -ItemType Directory -Force -Path $nupkgsPath

dotnet restore $srcPath

# Packing the project based on *.csproj for solution level
foreach ($proj in $projs) {
	Write-Host "Packing $proj" -Foreground Green
	dotnet pack $proj --output $nupkgsPath --configuration $configuration
}
