$version = $args[0]
$isRelease = $args[1]

$versionFile = ".\ProTONE Suite\src\Framework\OPMediaBase\OPMedia.Core\Version.cs"

$content = (Get-Content $versionFile).replace('1.0.0.0', $version).Replace('((false))', "$isRelease")
$content | Set-Content $versionFile

$buildInfoFile = ".\ProTONE Suite $version.buildinfo.txt"
$content = (Get-Date -Format 'yyyy-MM-dd') + ", $isRelease"
$content | Set-Content $buildInfoFile
