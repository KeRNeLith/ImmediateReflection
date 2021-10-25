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

<#
.Synopsis
    Update the BuildSetup.props to make the build a deploy build.
#>
function UpdateDeployBuild()
{
    # Update the package generation props to enable package generation of the right package
    $genPackagesFilePath = "./build/BuildSetup.props";
    $genPackagesContent = Get-Content $genPackagesFilePath;
    $newGenPackagesContent = $genPackagesContent -replace "false","true";
    $newGenPackagesContent | Set-Content $genPackagesFilePath;
}


# Update .props based on git tag status & setup build version
if ($env:APPVEYOR_REPO_TAG -eq "true")
{
    UpdateDeployBuild;
    $env:Build_Version = "$($env:APPVEYOR_REPO_TAG_NAME.Replace('v', ''))";
    $env:Build_Assembly_Version = $env:Build_Version;
    $env:IsFullIntegrationBuild = $false;   # Run only tests on deploy builds (not coverage, etc.)
}
else
{
    $env:Build_Version = "$($env:APPVEYOR_BUILD_VERSION)";
    $env:Build_Assembly_Version = "$env:Build_Version" -replace "\-.*","";
    $env:IsFullIntegrationBuild = "$env:APPVEYOR_PULL_REQUEST_NUMBER" -eq "" -And $env:Configuration -eq "Release";
}

UpdateAllPackagesGeneration;
$env:Release_Name = $env:Build_Version;

"Building version: $env:Build_Version";
"Building assembly version: $env:Build_Assembly_Version";

if ($env:IsFullIntegrationBuild -eq $true)
{
    "With full integration";

    $env:PATH="C:\Program Files\Java\jdk15\bin;$($env:PATH)"
    $env:JAVA_HOME_11_X64='C:\Program Files\Java\jdk15'
    $env:JAVA_HOME='C:\Program Files\Java\jdk15'
}
else
{
    "Without full integration";
}