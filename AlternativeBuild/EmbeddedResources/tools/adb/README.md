# ADB Tools (Embedded)

Bu klasör ADB (Android Debug Bridge) araçlarını içerir.

## 📦 İçerik

- `adb.exe` - Android Debug Bridge binary
- `AdbWinApi.dll` - Windows API wrapper
- `AdbWinUsbApi.dll` - USB API wrapper

## 🔧 Kullanım

Bu dosyalar executable içine gömülüdür (embedded resource) ve ilk çalıştırmada otomatik olarak:
```
%TEMP%\alternative-build-tools\adb\
```
konumuna çıkartılır.

## 📥 Kurulum

Eğer dosyalar eksikse, `copy-tools-from-sdk.bat` scriptini çalıştırın:

```bash
cd ..\..\..
.\copy-tools-from-sdk.bat
```

Bu script Android SDK'dan gerekli dosyaları otomatik kopyalar.

## 📌 Kaynak

Android Platform Tools:
https://developer.android.com/studio/releases/platform-tools

## ⚖️ Lisans

Apache License 2.0 (Android Open Source Project)
