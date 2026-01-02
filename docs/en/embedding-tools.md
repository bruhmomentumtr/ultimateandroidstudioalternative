# Embedding Tools Guide - Alternative Build Tool

This guide explains how to embed all necessary tools into the executable for complete independence.

---

## 🎯 Purpose

Embed ADB, build-tools, and optionally Java runtime into the executable to make it work without any external dependencies.

---

## 📦 Tools to Embed

### 1. ADB (Android Debug Bridge) - **REQUIRED**
- **Size:** ~5 MB
- **Required files:**
  - `adb.exe`
  - `AdbWinApi.dll`
  - `AdbWinUsbApi.dll`

### 2. Build Tools - **REQUIRED**
- **Size:** ~20 MB
- **Required files:**
  - `aapt2.exe` (~10 MB)
  - `zipalign.exe` (~500 KB)
  - `apksigner.jar` (~1 MB)

### 3. Java Runtime (JRE) - **OPTIONAL**
- **Size:** ~40-50 MB (minimal JRE)
- **Why needed:** To run `apksigner.jar`
- **Note:** Not needed if system Java exists

---

## 🔧 Step-by-Step Setup

### STEP 1: Download ADB Files

**Source:** [Android Platform Tools](https://developer.android.com/studio/releases/platform-tools)

**Download:**
```bash
# For Windows
https://dl.google.com/android/repository/platform-tools-latest-windows.zip

# Or copy from existing Android Studio
D:\androidstudioappdata\sdk\platform-tools\
```

**Required Files:**
1. `adb.exe`
2. `AdbWinApi.dll`
3. `AdbWinUsbApi.dll`

**Target Location:**
```
AlternativeBuild\EmbeddedResources\tools\adb\
├── adb.exe
├── AdbWinApi.dll
└── AdbWinUsbApi.dll
```

### STEP 2: Download Build Tools

**Source:** Android SDK Build Tools

**Copy from existing SDK:**
```bash
# If Android Studio is installed
D:\androidstudioappdata\sdk\build-tools\34.0.0\

# Or download latest
https://developer.android.com/studio#command-line-tools-only
```

**Required Files:**
1. `aapt2.exe`
2. `zipalign.exe`
3. `apksigner.jar` (may be in `lib\` folder: `lib\apksigner.jar`)

**Target Location:**
```
AlternativeBuild\EmbeddedResources\tools\build-tools\
├── aapt2.exe
├── zipalign.exe
└── apksigner.jar
```

### STEP 3: Update .csproj

**File:** `AlternativeBuild\AlternativeBuild.csproj`

Add these lines:
```xml
<ItemGroup>
  <!-- Already included - all files under EmbeddedResources -->
  <EmbeddedResource Include="EmbeddedResources\**\*" />
</ItemGroup>
```

### STEP 4: Rebuild

```bash
# Clean and rebuild
dotnet clean -c Release
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

---

## 📊 Size Comparison

| Configuration | Exe Size | Description |
|---------------|----------|-------------|
| **Minimal** (No embedded) | ~67 MB | ANDROID_HOME required |
| **ADB Only** | ~72 MB | ADB independent |
| **ADB + Build Tools** | ~92 MB | Fully independent |
| **Full (Java + All)** | ~140 MB | Everything included |

**Recommended:** ADB + Build Tools (~92 MB)

---

## ✅ Verification

### Test 1: Clean Windows VM
```bash
# Without ANDROID_HOME
alternative.exe -adb devices
# Should work!

alternative.exe -apksigner verify app.apk
# Should work!
```

### Test 2: Tool Extraction Check
```bash
# On first run
%TEMP%\alternative-build-tools\
├── adb\
│   ├── adb.exe
│   ├── AdbWinApi.dll
│   └── AdbWinUsbApi.dll
└── build-tools\
    ├── aapt2.exe
    ├── zipalign.exe
    └── apksigner.jar
```

---

## 🔍 Automated Copy Script

If Android Studio is installed, this script automatically copies files:

**File:** `copy-tools-from-sdk.bat` (already exists in project root)

**Usage:**
```bash
.\copy-tools-from-sdk.bat
```

---

## ⚠️ Important Notes

### Java Requirement
- `apksigner.jar` requires **Java**
- If user doesn't have Java:
  - Error: "Java not found"
  - Solution: Ask user to install Java
  - **Or:** Embed portable JRE (size +40 MB)

### License Compatibility
- Apache 2.0 License (Android tools)
- Can be embedded
- Attribution required (mention in LICENSE file)

### Updates
- Update embedded tools when they get updated
- Build tools update yearly
- ADB updates less frequently

---

## 🎯 Result

After completing these steps:
- ✅ Alternative.exe fully independent
- ✅ No ANDROID_HOME requirement
- ✅ Works on clean Windows
- ✅ Portable USB usage
- ✅ Works in CI/CD environments

**Final size:** ~92 MB (ADB + Build Tools)

---

## 📚 Related Guides

- [AVD Setup](avd-setup.md)
- [SDK Management](sdk-management.md)
- [Build Configuration](build-config.md)
