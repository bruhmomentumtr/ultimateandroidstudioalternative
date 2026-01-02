# SDK Management - Alternative Build Tool

Bu kılavuz, Alternative Build Tool ile SDK/NDK/Flutter yönetimini detaylı olarak açıklar.

---

## 📦 SDK Türleri

### 1. Android SDK (Command-line Tools)
Build için gerekli temel araçlar paketi.

### 2. Android NDK
C/C++ native kod derlemek için gerekli.

### 3. Flutter SDK
Flutter uygulamaları build etmek için.

---

## 🔍 İnteraktif Browse

Tüm SDK'lar için interaktif sürüm seçimi:

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

## 📥 Manuel Kurulum

### Android SDK
```bash
# Belirli sürüm
alternative -sdk install 11076708

# Liste
alternative -sdk list
```

### NDK
```bash
# Son sürüm
alternative -ndk install 27.0.12077973

# Liste
alternative -ndk list
```

### Flutter SDK
```bash
# Belirli sürüm
alternative --flutter-sdk install 3.27.1

# Liste
alternative --flutter-sdk list
```

---

## 🔧 ANDROID_HOME Konfigürasyonu

### Otomatik (Önerilen)
SDK kurulumu sonrası otomatik ayarlanır:
```
%USERPROFILE%\.alternative-sdk\android\cmdline-tools-XXXXX
```

### Manuel
```bash
# Windows
setx ANDROID_HOME "C:\Android\Sdk"

# Linux/macOS
export ANDROID_HOME=$HOME/Android/Sdk
echo 'export ANDROID_HOME=$HOME/Android/Sdk' >> ~/.bashrc
```

---

## 📍 Varsayılan Konumlar

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

## 🔄 Çoklu SDK Versiyonları

Farklı projeler için farklı SDK'lar:

```bash
# Proje 1 - Eski versiyon
cd project1
setx ANDROID_HOME "C:\Android\Sdk\old"
alternative -kotlin build

# Proje 2 - Yeni versiyon
cd project2
setx ANDROID_HOME "C:\Android\Sdk\new"
alternative -kotlin build
```

---

## ⚙️ Gelişmiş Ayarlar

### Özel SDK Konumu
```bash
alternative -config sdk-path "D:\CustomSDK"
```

### Proxy Ayarları (sdkmanager için)
```bash
# Windows: gradle.properties
systemProp.http.proxyHost=proxy.example.com
systemProp.http.proxyPort=8080
```

---

## 🐛 Sorun Giderme

### SDK Bulunamadı
```bash
# Kontrol et
echo %ANDROID_HOME%

# Yeniden ayarla
alternative -config sdk-path "C:\Android\Sdk"
```

### İndirme Hatası
```bash
# İnternet bağlantısı kontrol
ping dl.google.com

# Yeniden dene
alternative -sdk install 11076708
```

### Disk Alanı
- Android SDK: ~2-5 GB
- NDK: ~1-2 GB  
- Flutter SDK: ~2-3 GB

---

## 📚 İlgili Kılavuzlar

- [AVD & Emulator Kurulumu](avd-setup.md)
- [Build Configuration](build-config.md)
- [Termux Usage](termux-usage.md)
