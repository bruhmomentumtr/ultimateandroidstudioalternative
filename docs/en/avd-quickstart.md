# 🚀 AVD Quick Start - Zero Setup

Complete Android Emulator setup on a clean computer without Android Studio.

---

## ✅ Requirements

- ✅ `alternative.exe` (~70-95 MB)
- ✅ Internet connection
- ✅ ~2 GB free disk space

**That's it!** No Android Studio needed.

---

## 📝 Step-by-Step Setup

### Step 1: Download alternative.exe

```bash
# Navigate to download folder
cd C:\Downloads

# or add to PATH (optional)
move alternative.exe C:\Tools\
setx PATH "%PATH%;C:\Tools"
```

### Step 2: Install SDK

```bash
# Interactive SDK selection
alternative -sdk browse

# On screen:
# ★ 1. SDK 11076708 - Latest Stable (2024)
#   2. SDK 10406996 - 11.0 (2023)
#   ...
# Select version (1-10, or 0 to cancel): 

# Type 1 and press Enter
1

# SDK downloads and installs automatically (~500 MB)
```

### Step 3: Auto-Setup AVD (ONE COMMAND!)

```bash
# Single command - does everything!
alternative -avd setup --api 34 --name Pixel_5

# Program automatically:
# 1. Checks system image, downloads if missing (~1.5 GB)
#    "Downloading system image for API 34..."
#    "This may take 10-20 minutes..."
#
# 2. Checks emulator, downloads if missing (~150 MB)
#    "Installing emulator binary..."
#
# 3. Creates AVD
#    "Creating AVD: Pixel_5"
#
# 4. Asks if you want to start it
#    "Start emulator now? (Y/n):"
```

**Type Y and press Enter → Emulator starts!** 🎉

---

## ⚡ Quick Version (All Automatic)

```bash
# 1. Install SDK
alternative -sdk browse
# Select 1

# 2. RESTART POWERSHELL (for ANDROID_HOME)

# 3. Setup and start AVD
alternative -avd setup --api 34 --name Pixel_5
# Type Y (to start)
```

**Time:** ~20-30 minutes (depending on internet)

---

## 🔍 Step Details

### System Image Selection

API levels:
- **API 34** - Android 14 (Recommended)
- **API 33** - Android 13
- **API 32** - Android 12L
- **API 31** - Android 12

For different API:
```bash
alternative -avd setup --api 33 --name Android13
```

### AVD Naming

Use any name you want:
```bash
alternative -avd setup --api 34 --name MyEmulator
alternative -avd setup --api 34 --name Pixel_5
alternative -avd setup --api 34 --name Tablet10
```

---

## 🎮 Emulator Usage

### Start
```bash
alternative -avd start Pixel_5
```

### Stop
```bash
alternative -avd stop
```

### Install APK
```bash
# While emulator is running
alternative -adb install app.apk
```

### Delete AVD
```bash
alternative -avd delete Pixel_5
```

---

## 🐛 Troubleshooting

### "ANDROID_HOME not set" Error

**Solution:** Restart PowerShell

```bash
# Close and reopen PowerShell
# Then try again
alternative -avd setup --api 34 --name Pixel_5
```

### "System image not found" Error

**Manual system image installation:**
```bash
alternative -system-image browse
# Select API level (e.g., 1 = API 34)
1
```

### "Emulator not found" Error

**Manual emulator installation:**
```bash
alternative -emulator install
```

### Slow Download

Normal! Files are large:
- System image: ~1.5 GB
- Emulator: ~150 MB

May take 10-20 minutes.

---

## ✅ Success Check

```bash
# List AVDs
alternative -avd list

# Output:
# Installed AVDs:
#   Pixel_5 (API 34)
#
# Available System Images:
#   system-images;android-34;google_apis;x86_64
```

If you see this → Success! ✨

---

## 🎯 Summary

**Emulator in 3 Steps:**

1. `alternative -sdk browse` → Select 1
2. Restart PowerShell
3. `alternative -avd setup --api 34 --name Pixel_5` → Type Y

**That's it! No Android Studio required.** 🚀

---

## 📚 More Information

- [AVD Setup Guide](avd-setup.md) - Detailed AVD guide
- [SDK Management](sdk-management.md) - SDK management
- [README (Main Page)](../../README_EN.md)
