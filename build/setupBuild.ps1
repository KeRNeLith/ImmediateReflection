<#
.Synopsis
    Update the PackagesGeneration.props to generate all packages.
#>
function UpdateAllPackagesGeneration()
{
    # Update the package generation props to enable package generation of the right package
    $genPackagesFilePath = "./src/PackagesGeneration.props";
    $genPackagesContent = Get-Content $genPackagesFilePath;
    $newGenPackagesContent = $genPackagesContent -replace "false","true";
    $newGenPackagesContent | Set-Content $genPackagesFilePath;
}

# Update .props & setup build version
UpdateAllPackagesGeneration;
$env:Build_Version = "$($env:APPVEYOR_BUILD_VERSION)";
$env:Release_Name = $env:Build_Version;
$env:IsFullIntegrationBuild = "$env:APPVEYOR_PULL_REQUEST_NUMBER" -eq "" -And $env:Configuration -eq "Release";
$env:Build_Assembly_Version = "$env:Build_Version" -replace "\-.*","";

"Building version: $env:Build_Version";
"Building assembly version: $env:Build_Assembly_Version";

if ($env:IsFullIntegrationBuild -eq $true)
{
    "With full integration";
}
else
{
    "Without full integration";
}