# 🔨 Alternative Build Tool

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blue)](https://github.com)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)

**Languages:** [Türkçe](README.md) | [English](README_EN.md)

> **Android Studio olmadan** Kotlin, Java ve Flutter projelerini build edin!  
> Tek dosya (~70-95 MB), sıfır kurulum, tam bağımsız.

---

## 🎯 Ne İşe Yarar?

Alternative Build Tool, **Android Studio gerektirmeden** Android uygulamaları geliştirmenizi sağlar:

- ✅ **Kotlin/Java** projeleri build et (Gradle)
- ✅ **Flutter** projeleri build et
- ✅ **SDK/NDK** otomatik indir ve yönet
- ✅ **Android Emulator** tek komutla kur ve çalıştır
- ✅ **APK imzala** ve doğrula
- ✅ **ADB** entegrasyonu - cihaz yönetimi

**Hedef Kitle:**
- CI/CD pipeline'ları
- Hafif geliştirme ortamları
- Termux/Android tabanlı geliştirme
- Android Studio'suz build ihtiyacı

---

## 🚀 Hızlı Başlangıç

### 1. İndir
```bash
# Windows
alternative-windows-x64.exe

# Linux
chmod +x alternative-linux-x64
./alternative-linux-x64

# macOS
chmod +x alternative-macos-arm64
./alternative-macos-arm64
```

### 2. İlk Build
```bash
# Proje klasörüne git
cd MyAndroidProject

# Build (SDK otomatik indirilir!)
alternative -kotlin build
```

### 3. Emulator Kur (İsteğe Bağlı)
```bash
# Tek komut - her şeyi kurar!
alternative -avd setup --api 34 --name Pixel_5
```

---

## 📋 Temel Komutlar

### Build
```bash
alternative -kotlin build                # Debug APK
alternative -kotlin build-release        # Release APK
alternative -flutter build               # Flutter APK
```

### SDK Yönetimi (İnteraktif)
```bash
alternative -sdk browse                  # SDK seç ve kur
alternative -ndk browse                  # NDK seç ve kur
alternative --flutter-sdk browse         # Flutter SDK seç ve kur
```

### Android Emulator
```bash
alternative -system-image browse         # System image seç
alternative -avd setup --api 34 --name MyDevice  # Tam otomatik
alternative -avd start MyDevice          # Başlat
```

### Geliştirici Araçları
```bash
alternative -adb devices                 # Cihazları listele
alternative -adb install app.apk         # APK yükle
alternative -apksigner sign app.apk      # APK imzala
```

---

## 📚 Detaylı Dokümantasyon

Her bir özellik için ayrıntılı kılavuzlar:

### 🎯 Kullanım Kılavuzları
- 📖 **[🚀 AVD Hızlı Başlangıç](docs/tr/avd-quickstart.md)** - Sıfır kurulumdan emulator (3 adım!)
- 📖 **[AVD & Emulator Kurulumu](docs/tr/avd-setup.md)** - Detaylı AVD kurulum kılavuzu
- 📖 **[SDK Yönetimi](docs/tr/sdk-management.md)** - SDK/NDK/Flutter SDK kurulum ve yönetim
- 📖 **[Build Konfigürasyonu](docs/tr/build-config.md)** - Build ayarları, imzalama, optimizasyon
- 📖 **[Gömülü Araçlar](docs/tr/embedding-tools.md)** - ADB ve build tools'u executable'a gömmek
- 📖 **[Cross-Platform Kullanım](docs/tr/cross-platform.md)** - Linux, macOS, Docker, CI/CD
- 📖 **[Termux Kullanımı](docs/tr/termux-usage.md)** - Android telefondan geliştirme

### 🌍 Dil Seçenekleri
- 🇹🇷 **[Türkçe README](README.md)** (Bu dosya)
- 🇬🇧 **[English README](README_EN.md)**
- 📂 **[Türkçe Kılavuzlar](docs/tr/)** | **[English Guides](docs/en/)**

---

## 💡 Kullanım Senaryoları

### 1. CI/CD Pipeline
```yaml
# GitHub Actions örneği
- run: alternative -kotlin build-release
```

### 2. Termux (Android)
```bash
# Telefonda geliştir!
pkg install proot-distro
alternative -kotlin build
```

### 3. Hafif VM/Container
```bash
# Sadece 70 MB executable
docker run -v $(pwd):/workspace alternative
```

---

## 🌐 Platform Desteği

| Platform | Mimari | Boyut | Durum |
|----------|--------|-------|-------|
| Windows | x64 | ~70-95 MB | ✅ |
| Linux | x64 | ~65 MB | ✅ |
| Linux | ARM64 (Termux) | ~65 MB | ✅ |
| macOS | Intel | ~65 MB | ✅ |
| macOS | Apple Silicon | ~65 MB | ✅ |

---

## 🔧 Gelişmiş Özellikler

### Mevcut SDK Kullan
```bash
setx ANDROID_HOME "C:\Android\Sdk"
alternative -kotlin build  # Mevcut SDK kullanılır
```

### Özel Keystore
```bash
alternative -kotlin build-signed -keystore my.keystore -alias key0
```

### Konfigürasyon
```bash
alternative -config sdk-path "C:\custom\sdk"
alternative -config list
```

---

## 🛠️ Build

### Tek Platform
```bash
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

### Tüm Platformlar
```bash
# Windows
.\build-all-platforms.bat

# Linux/macOS  
./build-all-platforms.sh
```

Çıktı: `build-output/` klasörü

---

## 📦 İçindekiler

- **ADB** - Android Debug Bridge (embedded)
- **Build Tools** - aapt2, zipalign, apksigner (embedded)
- **SDK Manager** - Android SDK otomatik indirme
- **AVD Manager** - Emulator yönetimi
- **Gradle Wrapper** - Build sistemi

---

## 🤝 Katkıda Bulunma

```bash
git clone https://github.com/yourusername/androidstudioalternative
cd androidstudioalternative
dotnet build
```

---

## 📄 Lisans

MIT License - Özgürce kullanın, değiştirin, dağıtın!

---

## 🙏 Teşekkürler

- Android Open Source Project
- Flutter Team
- .NET Community

---

**Made with ❤️ for developers who love the command line**
