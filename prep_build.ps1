$version = $args[0]
$isRelease = $args[1]

$templateFile = ".\ProTONE Suite\src\Framework\OPMediaBase\OPMedia.LiteCore\VersionTemplate.cs"
$versionFile = ".\ProTONE Suite\src\Framework\OPMediaBase\OPMedia.LiteCore\Version.cs"

$content = (Get-Content $templateFile).replace('VERSION', $version).Replace('RELEASE', "$isRelease")
$content | Set-Content $versionFile

$buildInfoFile = ".\ProTONE Suite $version.buildinfo.txt"
$content = (Get-Date -Format 'yyyy-MM-dd') + ", $isRelease"
$content | Set-Content $buildInfoFile
