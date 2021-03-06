# About the Script
# Written by: Yufei Liu
# Email: feilfly@gmail.com
# Date: 21 Oct 2015
#-----------------------------------------------------------------------------------
param([string]$projectPath="c:\temp\", [string]$targetDLLFullPath="c:\temp\", [string]$buildConfig="Debug")
try
{
    $date = Get-Date

    [Console]::Writeline("** Executing UpdateVersionInfo.ps1 **")
    [Console]::Writeline("Parameters:")
    [Console]::Writeline([string]::Format("* projectPath: {0}", $projectPath))
    [Console]::Writeline([string]::Format("* targetDLL: {0}", $targetDLLFullPath))
    [Console]::Writeline([string]::Format("* buildConfig: {0}", $buildConfig))
    [Console]::Writeline()

    #$BuildLogFileLocation = [System.IO.Path]::Combine($projectPath,"BuildLog.txt")
    $AssemblyInfoFileLocation = $projectPath+"Properties\AssemblyInfo.cs"
	$attributeRead = "readonly"

    $enc = [Console]::OutputEncoding

    #$dll = (get-item $targetDLLFullPath)
    #$dllVersion = $dll.VersionInfo.FileVersion
    #$assemblyVersion = [Reflection.AssemblyName]::GetAssemblyName($dll.FullName).Version

    #[int]$nxtBuildNumber = $dllVersion.Build+1
    #$nextdllVersion = new-Object System.Version $dllVersion.Major, $dllVersion.Minor, $nxtBuildNumber, $dllVersion.Rivison

    #[int]$nxtBuildNumber = $assemblyVersion.Build+1
    #$nextAssemblyVersion = new-Object System.Version $assemblyVersion.Major, $assemblyVersion.Minor, $nxtBuildNumber, $assemblyVersion.Rivison

    #[Console]::Writeline([string]::Format("Dll File: {0}", $targetDLLFullPath))
    #[Console]::Writeline([string]::Format("Current Dll File Version: {0}", $dllVersion))
    #[Console]::Writeline([string]::Format("Current Dll Assembly Version: {0}", $assemblyVersion))
    #[Console]::Writeline([string]::Format("Write file version {0} in VersionInfo file.", $nextdllVersion))
    #[Console]::Writeline([string]::Format("Write assembly version {0} in VersionInfo file.", $nextAssemblyVersion))

    $versionInfoUPdated = 0
    (get-Content $AssemblyInfoFileLocation -Encoding utf8) | ForEach-Object {
        if($_ -match '\[assembly: AssemblyFileVersion.*\("(?<content>.*)"\)\]')
        {
            $VerObject = new-Object System.Version $matches['content']
            $nxtBuildNumber = get-date -Format yy
			$nxtRevisionNumber = get-date -Format MMdd
            $nextdllVersion = new-Object System.Version $VerObject.Major, $VerObject.Minor, $nxtBuildNumber, $nxtRevisionNumber

            $_ = '[assembly: AssemblyFileVersion("'+$nextdllVersion+'")]'
            [Console]::Writeline([string]::Format("Written version {0} in VersionInfo file", $nextdllVersion))
        }
        if($_ -match '\[assembly: AssemblyVersion.*\("(?<content>.*)"\)\]')
        {
            $VerObject = new-Object System.Version $matches['content']
            [int]$nxtBuildNumber = $VerObject.Build+1
            $nextAssemblyVersion = new-Object System.Version $VerObject.Major, $VerObject.Minor, $nxtBuildNumber, $VerObject.Rivison

            $_ = '[assembly: AssemblyVersion("'+$nextAssemblyVersion+'")]'
            [Console]::Writeline([string]::Format("Written version {0} in VersionInfo file", $nextAssemblyVersion))
        }
        $_
    } | set-Content -Encoding utf8 $AssemblyInfoFileLocation
    
    (get-Content $AssemblyInfoFileLocation -Encoding utf8) | ForEach-Object {
        if($_ -match '\[assembly: AssemblyFileVersion.*\("(?<content>.*)"\)\]')
        {
            $updatedVersion = new-Object System.Version $matches['content']
            if($updatedVersion -match $nextdllVersion)
            {
                [Console]::Writeline([string]::Format("Confirmed version {0} is written in VersionInfo file!", $nextdllVersion))
                $versionInfoUPdated = 1
            }
        }
        $_
    }| set-Content -Encoding utf8 tmp.txt
    
    if($versionInfoUPdated = 0)
    {
        [Console]::Writeline([string]::Format("Assembly file version failed to update to proper next version"))
        exit 1
    }
 
    [Console]::OutputEncoding = $enc

    exit 0
}
catch
{
    [Console]::Writeline($_)
    exit 1
}
