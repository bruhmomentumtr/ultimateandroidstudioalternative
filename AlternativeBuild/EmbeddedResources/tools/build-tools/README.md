# Build Tools (Embedded)

Bu klasör Android build araçlarını içerir.

## 📦 İçerik

- `aapt2.exe` - Android Asset Packaging Tool 2
- `zipalign.exe` - APK alignment tool
- `apksigner.jar` - APK signing tool

## 🔧 Kullanım

Bu dosyalar executable içine gömülüdür (embedded resource) ve ilk çalıştırmada otomatik olarak:
```
%TEMP%\alternative-build-tools\build-tools\
```
konumuna çıkartılır.

## 📥 Kurulum

Eğer dosyalar eksikse, `copy-tools-from-sdk.bat` scriptini çalıştırın:

```bash
cd ..\..\..
.\copy-tools-from-sdk.bat
```

Bu script Android SDK build-tools klasöründen gerekli dosyaları otomatik kopyalar.

## 📌 Kaynak

Android SDK Build Tools (latest version):
https://developer.android.com/studio#command-line-tools-only

## ⚠️ Notlar

- `apksigner.jar` çalıştırmak için **Java** gereklidir
- Dosyalar SDK'nın `build-tools/` klasöründen alınır
- En son build-tools versiyonu kullanılması önerilir

## ⚖️ Lisans

Apache License 2.0 (Android Open Source Project)
