# Placeholder for ADB binaries

## How to Add ADB

1. Download Android Platform Tools from:
   https://developer.android.com/studio/releases/platform-tools

2. Extract the following files from the `platform-tools` folder:
   - adb.exe
   - AdbWinApi.dll
   - AdbWinUsbApi.dll

3. Place them in this directory: `EmbeddedResources/tools/adb/`

4. Rebuild the project to embed them in the executable

## Note

ADB binaries cannot be included in the repository due to licensing.
Users must download them from the official Android website.

Alternatively, you can set the build action of these files to "Embedded Resource"
in the .csproj file after adding them.
