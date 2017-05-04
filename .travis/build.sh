#! /bin/sh

project="hololens-mapping-movement-poc"

echo "Attempting to build $project for Windows"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
    -projectPath $(pwd)"/HoloLens Mapping and Movement" \
    -logFile $(pwd)/unity.log \
    -buildOutput $(pwd)"/HoloLens Mapping and Movement/App" \
    -duskBuildTarget WSAPlayer \
    -wsaSDK UWP \
    -wsaUWPBuildType D3D \
    -executeMethod HoloToolkit.Unity.BuildSLNUtilities.PerformBuild_CommandLine \
    -batchmode \
    -quit

# echo "Attempting to build $project for Windows"
# /Applications/Unity/Unity.app/Contents/MacOS/Unity \
#   -batchmode \
#   -nographics \
#   -silent-crashes \
#   -logFile $(pwd)/unity.log \
#   -projectPath $(pwd) \
#   -buildWindowsPlayer "$(pwd)/Build/windows/$project.exe" \
#   -quit

# echo "Attempting to build $project for OS X"
# /Applications/Unity/Unity.app/Contents/MacOS/Unity \
#   -batchmode \
#   -nographics \
#   -silent-crashes \
#   -logFile $(pwd)/unity.log \
#   -projectPath $(pwd) \
#   -buildOSXUniversalPlayer "$(pwd)/Build/osx/$project.app" \
#   -quit

# echo "Attempting to build $project for Linux"
# /Applications/Unity/Unity.app/Contents/MacOS/Unity \
#   -batchmode \
#   -nographics \
#   -silent-crashes \
#   -logFile $(pwd)/unity.log \
#   -projectPath $(pwd) \
#   -buildLinuxUniversalPlayer "$(pwd)/Build/linux/$project.exe" \
#   -quit

echo 'Logs from build'
cat $(pwd)/unity.log


echo 'Attempting to zip builds'
zip -r $(pwd)/Build/linux.zip $(pwd)/Build/linux/
zip -r $(pwd)/Build/mac.zip $(pwd)/Build/osx/
zip -r $(pwd)/Build/windows.zip $(pwd)/Build/windows/