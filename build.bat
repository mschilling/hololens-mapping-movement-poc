@echo off
echo Project path = %~dp0

echo.
echo Start SLN build
"C:\program files\Unity\Editor\Unity.exe" -projectPath "%~dp0\HoloLens Mapping and Movement" -logFile "%~dp0\log.txt" -buildOutput "%~dp0\HoloLens Mapping and Movement\App" -duskBuildTarget WSAPlayer -wsaSDK UWP -wsaUWPBuildType D3D -executeMethod HoloToolkit.Unity.BuildSLNUtilities.PerformBuild_CommandLine -batchmode -quit

echo.
echo Restoring NuGet packages
nuget restore "%~dp0\HoloLens Mapping and Movement\UWP\project.json"

echo.
echo Start APPX build
call "%vs140comnTools%\VsDevCmd.bat"
MSBuild.exe /nologo /verbosity:d "/target:Build" /p:Configuration=Debug /p:Platform="x86" /p:OutDir=bin /p:GenerateProjectSpecificOutputFolder=true /p:StyleCopTreatErrorsAsWarnings=false /maxcpucount "%~dp0\HoloLens Mapping and Movement\App\HoloLens Mapping and Movement.sln"

echo.
echo Done