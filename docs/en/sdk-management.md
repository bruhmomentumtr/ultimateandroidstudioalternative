# SDK Management - Alternative Build Tool

This guide explains SDK/NDK/Flutter management with Alternative Build Tool in detail.

---

## 📦 SDK Types

### 1. Android SDK (Command-line Tools)
Essential tools package for building.

### 2. Android NDK
Required for compiling C/C++ native code.

### 3. Flutter SDK
Required for building Flutter applications.

---

## 🔍 Interactive Browse

Interactive version selection for all SDKs:

```bash
# Android SDK
alternative -sdk browse
# ★ 1. SDK 11076708 - Latest Stable (2024)
#   2. SDK 10406996 - 11.0 (2023)
#   3. SDK 9477386 - 9.0 (2022)
# ...
# Select version (1-10, or 0 to cancel): 1

# NDK
alternative -ndk browse

# Flutter SDK
alternative --flutter-sdk browse
```

---

## 📥 Manual Installation

### Android SDK
```bash
# Specific version
alternative -sdk install 11076708

# List
alternative -sdk list
```

### NDK
```bash
# Latest version
alternative -ndk install 27.0.12077973

# List
alternative -ndk list
```

### Flutter SDK
```bash
# Specific version
alternative --flutter-sdk install 3.27.1

# List
alternative --flutter-sdk list
```

---

## 🔧 ANDROID_HOME Configuration

### Automatic (Recommended)
Automatically set after SDK installation:
```
%USERPROFILE%\.alternative-sdk\android\cmdline-tools-XXXXX
```

### Manual
```bash
# Windows
setx ANDROID_HOME "C:\Android\Sdk"

# Linux/macOS
export ANDROID_HOME=$HOME/Android/Sdk
echo 'export ANDROID_HOME=$HOME/Android/Sdk' >> ~/.bashrc
```

---

## 📍 Default Locations

### Windows
```
C:\Users\USERNAME\.alternative-sdk\
├── android\
│   ├── cmdline-tools-11076708\
│   ├── ndk-27.0.12077973\
│   └── ...
└── flutter\
    └── 3.27.1\
```

### Linux/macOS
```
~/.alternative-sdk/
├── android/
│   ├── cmdline-tools-11076708/
│   ├── ndk-27.0.12077973/
│   └── ...
└── flutter/
    └── 3.27.1/
```

---

## 🔄 Multiple SDK Versions

Different SDKs for different projects:

```bash
# Project 1 - Old version
cd project1
setx ANDROID_HOME "C:\Android\Sdk\old"
alternative -kotlin build

# Project 2 - New version
cd project2
setx ANDROID_HOME "C:\Android\Sdk\new"
alternative -kotlin build
```

---

## ⚙️ Advanced Settings

### Custom SDK Location
```bash
alternative -config sdk-path "D:\CustomSDK"
```

### Proxy Settings (for sdkmanager)
```bash
# Windows: gradle.properties
systemProp.http.proxyHost=proxy.example.com
systemProp.http.proxyPort=8080
```

---

## 🐛 Troubleshooting

### SDK Not Found
```bash
# Check
echo %ANDROID_HOME%

# Reset
alternative -config sdk-path "C:\Android\Sdk"
```

### Download Error
```bash
# Check internet connection
ping dl.google.com

# Retry
alternative -sdk install 11076708
```

### Disk Space
- Android SDK: ~2-5 GB
- NDK: ~1-2 GB  
- Flutter SDK: ~2-3 GB

---

## 📚 Related Guides

- [AVD & Emulator Setup](avd-setup.md)
- [Build Configuration](build-config.md)
- [Termux Usage](termux-usage.md)
