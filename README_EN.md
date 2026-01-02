# Alternative Build Tool - Android Studio Alternative

> **Ultimate command-line Android build tool**  
> Build Kotlin, Java, and Flutter projects without Android Studio!

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![Platform](https://img.shields.io/badge/platform-Windows-blue)](https://www.microsoft.com/windows)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)

**Languages:** [Türkçe](README.md) | [English](README_EN.md)

---

## 📑 Table of Contents

1. [Features](#-features)
2. [Quick Start](#-quick-start)
3. [Usage Guide](#-usage-guide)
4. [Using Existing Android Studio SDK](#-using-existing-android-studio-sdk)
5. [SDK Management](#-sdk-management)
6. [ADB and Build Tools](#-adb-and-build-tools)
7. [Usage Scenarios](#-usage-scenarios)
8. [Embedding Tools](#-embedding-tools)
9. [Project Structure](#-project-structure)
10. [Developer Information](#-developer-information)
11. [Troubleshooting](#-troubleshooting)

---

## 🎯 Features

| Feature | Description |
|---------|-------------|
| 🔨 **Kotlin/Java Build** | Debug, release, and signed APK with Gradle integration |
| 🦋 **Flutter Build** | Build Flutter projects |
| 🤖 **Auto SDK Detection** | Automatically detect and download required SDK/NDK from build.gradle |
| 📦 **SDK Management** | Download and manage Android SDK, NDK, and Flutter SDK |
| 📲 **ADB Integration** | Device management and debugging |
| 📺 **AVD (Emulator) Support** | Android Virtual Device management and emulator |
| ✍️ **APK Signing** | APK signing with apksigner |
| ⚙️ **Configurable** | Customize SDK location and settings |
| 💾 **Single Executable** | ~70-95 MB self-contained exe |
| 🌐 **Android Studio Independent** | Works with zero installation |
| 🔄 **Cross-Platform** | Windows, Linux, macOS support |

---

## 🚀 Quick Start

### Installation

```bash
git clone https://github.com/yourusername/androidstudioalternative.git
cd androidstudioalternative
.\build-release.bat
```

**Output:** `AlternativeBuild\bin\Release\net10.0\win-x64\publish\alternative.exe`

### Add to PATH (Optional)

```powershell
# Copy exe
copy AlternativeBuild\bin\Release\net10.0\win-x64\publish\alternative.exe C:\Tools\

# Add to PATH (PowerShell Admin)
$env:Path += ";C:\Tools"
```

### First Build

```bash
cd MyAndroidProject
alternative.exe -kotlin build  # SDKs will be downloaded automatically!
```

---

## 📖 Usage Guide

### Auto SDK/NDK Detection

**Required SDK and NDK versions are automatically detected and downloaded when building!**

```bash
cd MyAndroidProject
alternative.exe -kotlin build  # Automatically reads compileSdk and ndkVersion from build.gradle
```

The program automatically reads from `build.gradle` or `build.gradle.kts`:
- `compileSdk` / `compileSdkVersion` - Android SDK
- `ndkVersion` - NDK

And installs them if missing.

### Build Commands

#### Kotlin/Java Project
```bash
alternative.exe -kotlin build                    # Debug APK
alternative.exe -kotlin build-release            # Release APK
alternative.exe -kotlin build-signed -keystore my.jks -alias mykey
alternative.exe -kotlin clean                    # Clean project
```

#### Flutter Project
```bash
alternative.exe -flutter build                   # Debug APK
alternative.exe -flutter build-release           # Release APK
alternative.exe -flutter clean                   # Clean project
```

---

## 🔧 Using Existing Android Studio SDK

### ANDROID_HOME Environment Variable (Recommended)

To use your existing Android Studio SDK:

```powershell
# Windows - System-wide (PowerShell Admin)
setx ANDROID_HOME "C:\Users\YourName\AppData\Local\Android\Sdk"

# Or your custom location
setx ANDROID_HOME "D:\androidstudioappdata\sdk"

# Current session only
$env:ANDROID_HOME = "D:\androidstudioappdata\sdk"
```

### Using Config Command

```bash
# Specify your Android Studio SDK
alternative.exe -config sdk-path "D:\androidstudioappdata\sdk"

# Check
alternative.exe -config get sdk-path
alternative.exe -config list
```

### SDK Priority Order

The program searches for SDKs in this order:

1. ✅ **ANDROID_HOME** environment variable
2. ✅ **Config sdk-path** (`alternative.exe -config sdk-path`)
3. ✅ **Auto download** (`%USERPROFILE%\.alternative-sdk\`)

**Result:** Existing SDK is used, no unnecessary downloads!

---

## 📦 SDK Management

### SDK Download

```bash
# Android SDK
alternative.exe -sdk install 11076708           # SDK version
alternative.exe -sdk list                        # List installed SDKs

# NDK
alternative.exe -ndk install 26.1.10909125      # NDK version
alternative.exe -ndk list                        # List installed NDKs

# Flutter SDK
alternative.exe --flutter-sdk install 3.19.0    # Flutter version
alternative.exe --flutter-sdk list               # List installed Flutter SDKs
```

### Default SDK Locations

- **Android SDK:** `%USERPROFILE%\.alternative-sdk\android\`
- **NDK:** `%USERPROFILE%\.alternative-sdk\ndk\`
- **Flutter SDK:** `%USERPROFILE%\.alternative-sdk\flutter\`
- **Config:** `%USERPROFILE%\.alternative-build\config.json`

---

## 📲 ADB and Build Tools

### ADB Commands

```bash
alternative.exe -adb devices                     # List devices
alternative.exe -adb install app.apk             # Install APK
alternative.exe -adb uninstall com.example.app   # Uninstall app
alternative.exe -adb logcat                      # View logs
alternative.exe -adb shell pm list packages      # List packages
alternative.exe -adb push local.txt /sdcard/     # Push file
alternative.exe -adb pull /sdcard/file.txt .     # Pull file
```

### APK Signing

```bash
# Sign APK
alternative.exe -apksigner sign ^
  --ks my.jks ^
  --ks-key-alias mykey ^
  --out signed.apk ^
  app.apk

# Verify APK
alternative.exe -apksigner verify signed.apk
alternative.exe -apksigner verify --verbose signed.apk
```

### AVD (Android Virtual Device / Emulator)

**List AVDs and System Images:**
```bash
# List installed AVDs and available system images
alternative.exe -avd list

# Example output:
# Installed AVDs:
#   Pixel_5 (API 34)
#   Nexus_5X (API 33)
#
# Available System Images (for creating new AVDs):
#   system-images;android-34;google_apis;x86_64
#   system-images;android-33;google_apis;x86_64
```

**Creating AVDs:**
```bash
# Create new AVD (name, api-level)
alternative.exe -avd create Pixel_5 34        # API 34 (Android 14)
alternative.exe -avd create Nexus_5X 33       # API 33 (Android 13)
alternative.exe -avd create Tablet_10 34      # For tablet

# Note: System image must be in ANDROID_HOME/system-images/
# If not, download with SDK Manager:
# alternative.exe -sdk install <version>
```

**Starting and Stopping AVD:**
```bash
# Start AVD
alternative.exe -avd start Pixel_5

# Stop emulator (all running emulators)
alternative.exe -avd stop
```

**Deleting AVD:**
```bash
# Completely delete AVD
alternative.exe -avd delete Pixel_5
```

**Testing with Emulator:**
```bash
# 1. Start AVD
alternative.exe -avd start Pixel_5

# 2. Check devices
alternative.exe -adb devices

# 3. Install APK
alternative.exe -adb install app-debug.apk

# 4. View logs
alternative.exe -adb logcat
```

**Note:** ANDROID_HOME must be set and Android SDK must be installed for AVD management.

---

## 🌐 Cross-Platform Usage

### Windows
```powershell
alternative.exe -kotlin build
alternative.exe -avd list
```

### Linux
```bash
chmod +x alternative
./alternative -kotlin build
./alternative -avd list
```

### macOS
```bash
chmod +x alternative
./alternative -kotlin build
./alternative -avd list
```

### Multi-Platform Build
```bash
# Build for all platforms (requires .NET 10 SDK)
./build-all-platforms.sh         # Linux/macOS
build-all-platforms.bat          # Windows (all platforms)
.\build-release.bat              # Windows only
```

**Outputs:**
- Windows: `alternative.exe` (~70 MB)
- Linux: `alternative` (~65 MB)
- macOS: `alternative` (~70 MB)
- **Android/Termux: `alternative` (~65 MB) - Linux ARM64**

### 📱 Termux/Android Support

**Alternative Build Tool works on Android devices via Termux!**

```bash
# Build for Android ARM64 on PC
dotnet publish -c Release -r linux-arm64 --self-contained -p:PublishSingleFile=true

# Transfer to Android (ADB)
adb push alternative /sdcard/

# In Termux
mv /sdcard/alternative ~/
chmod +x ~/alternative
~/alternative -help
```

**Usage Scenarios:**
- ✅ SDK management
- ✅ APK signing
- ✅ Light operations
- ⚠️ Gradle build limited (performance)

**Details:** [TERMUX_USAGE.md](TERMUX_USAGE.md)

---

## 🎬 Usage Scenarios

### Scenario 1: Zero Installation (Clean Computer)

**Requirements:**
- ✅ Only `alternative.exe`
- ✅ Internet connection

**How it works:**
```bash
alternative.exe -kotlin build
# Program automatically downloads SDK and builds
```

**Advantage:** No need to install Android Studio!

### Scenario 2: Existing Android Studio

**Configuration:**
```bash
setx ANDROID_HOME "C:\Users\YourName\AppData\Local\Android\Sdk"
alternative.exe -kotlin build  # Uses existing SDK
```

**Result:** No re-download, uses existing SDK.

### Scenario 3: CI/CD Pipeline

```yaml
# GitHub Actions / Azure DevOps
- name: Build APK
  run: |
    alternative.exe -sdk install 11076708
    alternative.exe -kotlin build-release
```

SDKs can be cached:
```yaml
- uses: actions/cache@v3
  with:
    path: ~\.alternative-sdk
    key: android-sdk-${{ hashFiles('**/build.gradle') }}
```

### Scenario 4: Portable Usage (USB)

```
USB:\
├── alternative.exe
├── .alternative-sdk\
└── MyProjects\
```

```bash
alternative.exe -config sdk-path "E:\.alternative-sdk"
cd E:\MyProjects\MyApp
alternative.exe -kotlin build
```

**Result:** Entire setup on USB, works on any computer!

### Scenario 5: Multiple Projects (Different SDKs)

```bash
cd ProjectA
alternative.exe -kotlin build  # SDK 33 automatic

cd ProjectB  
alternative.exe -kotlin build  # SDK 34 automatic
```

**Advantage:** No SDK conflicts, each project independent!

---

## 🛠️ Embedding Tools

### Adding ADB

1. Download [Android Platform Tools](https://developer.android.com/studio/releases/platform-tools)
2. Extract `adb.exe`, `AdbWinApi.dll`, `AdbWinUsbApi.dll`
3. Copy to `AlternativeBuild\EmbeddedResources\tools\adb\`
4. Rebuild project

### Adding Build Tools

1. Download Android SDK Build Tools (34.0.0+)
2. Extract `aapt2.exe`, `zipalign.exe`, `apksigner.jar`
3. Copy to `AlternativeBuild\EmbeddedResources\tools\build-tools\`
4. Set build action to "Embedded Resource" in `.csproj`
5. Rebuild project

**Note:** These tools are embedded in exe and auto-extracted on first run.

---

## 🏗️ Project Structure

```
AlternativeBuild/
├── Commands/          # CLI commands
│   ├── FlutterBuildCommand.cs
│   ├── KotlinBuildCommand.cs
│   ├── SdkCommand.cs
│   ├── NdkCommand.cs
│   ├── FlutterSdkCommand.cs
│   ├── ConfigCommand.cs
│   ├── AdbCommand.cs
│   ├── AvdCommand.cs
│   └── ApkSignerCommand.cs
├── Build/             # Build pipelines
│   ├── GradleBuilder.cs
│   └── FlutterBuilder.cs
├── SDK/               # SDK management
│   ├── SdkManager.cs
│   ├── SdkDownloader.cs
│   └── ProjectInspector.cs
├── Tools/             # Embedded tools
│   ├── ToolsManager.cs
│   └── ResourceExtractor.cs
└── Utils/             # Utility classes
    ├── ConsoleLogger.cs
    ├── ProcessRunner.cs
    ├── ConfigManager.cs
    └── PlatformHelper.cs
```

---

## 🔧 Developer Information

### Requirements
- .NET 10 SDK
- Windows 10/11, Linux (Ubuntu 20.04+), macOS (10.15+)

### Build
```bash
dotnet build
dotnet run -- -help
```

### Multi-Platform Build

**All Platforms (Linux/macOS):**
```bash
chmod +x build-all-platforms.sh
./build-all-platforms.sh
```

**Windows Only:**
```batch
build-release.bat
```

**Manual Build:**
```bash
# Windows x64
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux x64
dotnet publish -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# Linux ARM64 (Android/Termux)
dotnet publish -c Release -r linux-arm64 --self-contained -p:PublishSingleFile=true

# macOS Intel
dotnet publish -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true

# macOS Apple Silicon (M1/M2)
dotnet publish -c Release -r osx-arm64 --self-contained -p:PublishSingleFile=true
```

### Platform Detection

Tool automatically detects platform:

```csharp
// Using PlatformHelper.cs
if (PlatformHelper.IsWindows)
{
    // Windows-specific code
}
else if (PlatformHelper.IsLinux)
{
    // Linux-specific code
}
else if (PlatformHelper.IsMacOS)
{
    // macOS-specific code
}
```

**Platform-Specific Paths:**
- **Home:** Windows: `%USERPROFILE%`, Linux/macOS: `$HOME`
- **SDK:** `~/.alternative-sdk` (all platforms)
- **Config:** `~/.alternative-build` (all platforms)
- **Temp:** Windows: `%TEMP%`, Linux/macOS: `/tmp`

**Executable Extensions:**
- Windows: `.exe`, `.bat`
- Linux/macOS: (none), `.sh`

### Testing
```bash
# Debug mode
dotnet run -- -kotlin build

# Release build
.\build-release.bat
```

---

## ❓ Troubleshooting

### "SDK not found" Error

**Solution 1:** Check ANDROID_HOME
```powershell
echo $env:ANDROID_HOME
# If empty: setx ANDROID_HOME "YOUR_SDK_PATH"
```

**Solution 2:** Check config
```bash
alternative.exe -config get sdk-path
```

**Solution 3:** Install SDK manually
```bash
alternative.exe -sdk install 11076708
```

### "Java not found" Error

```bash
# Set JAVA_HOME
setx JAVA_HOME "C:\Program Files\Java\jdk-17"
```

### Build Error

```bash
# Clean project
alternative.exe -kotlin clean
cd MyProject
gradlew clean
```

### ANDROID_HOME Set But Not Used

```powershell
# Restart PowerShell
# Or restart computer

# Test
echo $env:ANDROID_HOME
```

---

## 📚 Help

```bash
alternative.exe -help                    # General help
alternative.exe -kotlin -help            # Kotlin/Java help
alternative.exe -flutter -help           # Flutter help
alternative.exe -adb -help               # ADB help
alternative.exe -apksigner -help         # APK signing help
alternative.exe -sdk -help               # SDK management help
alternative.exe -avd -help               # AVD management help
```

---

## 🤝 Contributing

Contributions are welcome! Before submitting a pull request:
1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push and open a PR

---

## 📄 License

MIT License - See [LICENSE](LICENSE) file for details

---

## 🌟 Acknowledgments

- Android Open Source Project
- Flutter Team
- .NET Community

---

**Made with ❤️ for Android developers who prefer the command line**

---

## ✅ Summary

**With Alternative Build Tool:**
- ✅ Build Android applications without Android Studio
- ✅ SDKs are automatically detected and downloaded
- ✅ Can use your existing Android Studio SDK
- ✅ Portable, single executable
- ✅ Kotlin, Java, and Flutter support
- ✅ ADB and APK signing included
- ✅ AVD/Emulator management
- ✅ Cross-platform (Windows, Linux, macOS, Android/Termux)

**Get started now:**
```bash
alternative.exe -kotlin build  # It's that simple!
```

🎉 **Now completely independent from Android Studio!**
