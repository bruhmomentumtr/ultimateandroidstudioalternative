# AVD & Emulator Setup - Without Android Studio

This guide explains how to set up Android Emulator without Android Studio using Alternative Build Tool.

---

## 🎯 Goal

Setup complete AVD/Emulator from scratch - no Android Studio needed!

---

## 📋 Required Components

### 1. Android SDK Command-Line Tools ✅ (Available)
```bash
alternative -sdk install 11076708
```

### 2. Platform Tools (ADB) ✅ (Embedded)
- Already included in executable

### 3. Build Tools ✅ (Embedded)
- Already included in executable

### 4. Emulator Binary ✅ (Auto-install)
```bash
alternative -emulator install
```

### 5. System Image ✅ (Auto-install)
```bash
alternative -system-image install android-34
```

---

## 🚀 Quick Setup (Automatic)

### Single Command - Everything Automatic!
```bash
alternative -avd setup --api 34 --name Pixel_5

# This command:
# 1. Checks system image, downloads if missing (~1.5 GB)
# 2. Checks emulator, downloads if missing (~150 MB)
# 3. Creates AVD
# 4. Asks if you want to start it
```

**Done!** Emulator ready in one command!

---

## 📖 Manual Setup (Step by Step)

### Step 1: Install SDK
```bash
alternative -sdk browse
# Select: 1 (Latest)
```

### Step 2: Set ANDROID_HOME
```bash
# Windows
setx ANDROID_HOME "%USERPROFILE%\.alternative-sdk\android\cmdline-tools-11076708"

# Linux/macOS
export ANDROID_HOME=~/.alternative-sdk/android/cmdline-tools-11076708
```

### Step 3: Install System Image
```bash
# Browse and select
alternative -system-image browse

# Or direct install
alternative -system-image install 34
```

### Step 4: Install Emulator
```bash
alternative -emulator install
```

### Step 5: Create AVD
```bash
alternative -avd create Pixel_5 34
```

### Step 6: Start Emulator
```bash
alternative -avd start Pixel_5
```

---

## 🔧 AVD Management

### List AVDs
```bash
alternative -avd list
```

### Delete AVD
```bash
alternative -avd delete Pixel_5
```

### Stop Emulator
```bash
alternative -avd stop
```

---

## 📦 Size Impact

| Component | Size | Required For |
|-----------|------|--------------|
| Emulator Binary | ~150 MB | Running AVD |
| System Image (x86_64) | ~1-1.5 GB | Running AVD |
| Platform SDK | ~50 MB | Build (optional for AVD) |
| **Total** | **~1.2-1.7 GB** | First AVD setup |

**Note:** Files are not embedded in exe, downloaded to `%USERPROFILE%\.alternative-sdk\`

---

## 🌐 Available System Images

```bash
alternative -system-image browse

# List:
# ★ 1. Android 14 (API 34) - Recommended
#   2. Android 13 (API 33)
#   3. Android 12L (API 32)
#   4. Android 12 (API 31)
#   5. Android 11 (API 30)
# ...
```

---

## ✅ Summary

**Current Status:**
- ✅ SDK can be installed with Alternative
- ✅ AVD management available
- ✅ Single-command auto setup
- ✅ AVD can be started

**Result:** 100% functional emulator without Android Studio!

---

## 📚 Related Guides

- [SDK Management](sdk-management.md)
- [Build Configuration](build-config.md)
- [Termux Usage](termux-usage.md)
