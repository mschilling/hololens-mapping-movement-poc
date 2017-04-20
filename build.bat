echo Start SLN build
"C:\program files\Unity\Editor\Unity.exe" -projectPath "%~dp0\HoloLens Mapping and Movement" -logFile "%~dp0\log.txt" -buildOutput "%~dp0\HoloLens Mapping and Movement\App" -duskBuildTarget WSAPlayer -wsaUWPBuildType D3D -executeMethod HoloToolkit.Unity.BuildSLNUtilities.PerformBuild_CommandLine -batchmode -quit
echo SLN is build