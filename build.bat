rem @echo off
echo %~dp0

echo Start SLN build
"C:\program files\Unity\Editor\Unity.exe" -projectPath "%~dp0\HoloLens Mapping and Movement" -logFile "%~dp0\log.txt" -buildOutput "%~dp0\HoloLens Mapping and Movement\App" -duskBuildTarget WSAPlayer -wsaUWPBuildType D3D -executeMethod HoloToolkit.Unity.BuildSLNUtilities.PerformBuild_CommandLine -batchmode -quit
echo completing SLN build

echo Start APPX build
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" /nologo /verbosity:d "/target:Build" /p:Configuration=Debug /p:Platform="x86" /p:OutDir=bin /p:GenerateProjectSpecificOutputFolder=true /p:StyleCopTreatErrorsAsWarnings=false /maxcpucount "%~dp0\HoloLens Mapping and Movement\App\HoloLens Mapping and Movement.sln"
echo completing APPX build