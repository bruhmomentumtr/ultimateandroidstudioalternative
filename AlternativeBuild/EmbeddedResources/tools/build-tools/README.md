# Placeholder for Android Build Tools

## How to Add Build Tools

1. Download Android SDK Build Tools from Android SDK Manager or:
   https://developer.android.com/studio/releases/build-tools

2. Extract the following files and place them here:
   - aapt2.exe (from build-tools folder)
   - zipalign.exe (from build-tools folder)
   - apksigner.jar (from build-tools/lib folder)
   - d8.jar (optional, from build-tools/lib folder)

3. Set build action to "Embedded Resource" in .csproj

4. Rebuild the project

## Recommended Version

Use the latest stable build-tools version (e.g., 34.0.0 or newer)

## Note

These tools are part of the Android SDK and should be downloaded
from the official Android developer website.
