# Alternative Build Tool - Android Studio Alternative

> **Ultimate command-line Android build tool**  
> Kotlin, Java, ve Flutter projelerini Android Studio olmadan build edin!

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![Platform](https://img.shields.io/badge/platform-Windows-blue)](https://www.microsoft.com/windows)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)

**Languages:** [Türkçe](README.md) | [English](README_EN.md)

---

## 📑 İçindekiler

1. [Özellikler](#-özellikler)
2. [Hızlı Başlangıç](#-hızlı-başlangıç)
3. [Kullanım Kılavuzu](#-kullanım-kılavuzu)
4. [Mevcut Android Studio SDK Kullanımı](#-mevcut-android-studio-sdk-kullanımı)
5. [SDK Yönetimi](#-sdk-yönetimi)
6. [ADB ve Build Tools](#-adb-ve-build-tools)
7. [Kullanım Senaryoları](#-kullanım-senaryoları)
8. [Embedded Tools](#-embedded-tools-ekleme)
9. [Proje Yapısı](#-proje-yapısı)
10. [Geliştirici Bilgileri](#-geliştirici-bilgileri)
11. [Sorun Giderme](#-sorun-giderme)

---

## 🎯 Özellikler

| Özellik | Açıklama |
|---------|----------|
| 🔨 **Kotlin/Java Build** | Gradle entegrasyonu ile debug, release, ve signed APK |
| 🦋 **Flutter Build** | Flutter projelerini build edin |
| 🤖 **Auto SDK Detection** | Build.gradle'dan otomatik SDK/NDK tespit ve indirme |
| 📦 **SDK Yönetimi** | Android SDK, NDK, ve Flutter SDK indirin |
| 📲 **ADB Entegrasyonu** | Cihaz yönetimi ve debugging |
| 📺 **AVD (Emulator) Desteği** | Android Virtual Device yönetimi ve emulator |
| ✍️ **APK İmzalama** | Apksigner ile APK imzalama |
| ⚙️ **Yapılandırılabilir** | SDK konumu ve ayarları özelleştirin |
| 💾 **Tek Executable** | ~70-95 MB self-contained exe |
| 🌐 **Android Studio'dan Bağımsız** | Sıfır kurulum ile çalışır |
| 🔄 **Cross-Platform** | Windows, Linux, macOS desteği |

---

## 🚀 Hızlı Başlangıç

### Kurulum

```bash
git clone https://github.com/yourusername/androidstudioalternative.git
cd androidstudioalternative
.\build-release.bat
```

**Çıktı:** `AlternativeBuild\bin\Release\net10.0\win-x64\publish\alternative.exe`

### PATH'e Ekle (İsteğe Bağlı)

```powershell
# Exe'yi kopyala
copy AlternativeBuild\bin\Release\net10.0\win-x64\publish\alternative.exe C:\Tools\

# PATH'e ekle (PowerShell Admin)
$env:Path += ";C:\Tools"
```

### İlk Build

```bash
cd MyAndroidProject
alternative.exe -kotlin build  # SDK'lar otomatik yüklenecek!
```

---

## 📖 Kullanım Kılavuzu

### Auto SDK/NDK Detection

**Projeyi build ederken otomatik olarak gerekli SDK ve NDK sürümleri tespit edilip indirilir!**

```bash
cd MyAndroidProject
alternative.exe -kotlin build  # Otomatik olarak build.gradle'dan compileSdk ve ndkVersion okur ve gerekirse yükler
```

Program `build.gradle` veya `build.gradle.kts` dosyasından:
- `compileSdk` / `compileSdkVersion` - Android SDK
- `ndkVersion` - NDK

bilgilerini otomatik okur ve eksikse yükler.

### Build Commands

#### Kotlin/Java Projesi
```bash
alternative.exe -kotlin build                    # Debug APK
alternative.exe -kotlin build-release            # Release APK
alternative.exe -kotlin build-signed -keystore my.jks -alias mykey
alternative.exe -kotlin clean                    # Clean project
```

#### Flutter Projesi
```bash
alternative.exe -flutter build                   # Debug APK
alternative.exe -flutter build-release           # Release APK
alternative.exe -flutter clean                   # Clean project
```

---

## 🔧 Mevcut Android Studio SDK Kullanımı

### ANDROID_HOME Environment Variable (Önerilen)

Mevcut Android Studio SDK'nızı kullanmak için:

```powershell
# Windows - Sistem geneli için (PowerShell Admin)
setx ANDROID_HOME "C:\Users\YourName\AppData\Local\Android\Sdk"

# Veya özel konumunuz varsa
setx ANDROID_HOME "D:\androidstudioappdata\sdk"

# Sadece mevcut oturum için
$env:ANDROID_HOME = "D:\androidstudioappdata\sdk"
```

### Config Komutu ile

```bash
# Mevcut Android Studio SDK'nızı gösterin
alternative.exe -config sdk-path "D:\androidstudioappdata\sdk"

# Kontrol edin
alternative.exe -config get sdk-path
alternative.exe -config list
```

### SDK Öncelik Sırası

Program SDK'ları şu sırayla arar:

1. ✅ **ANDROID_HOME** environment variable
2. ✅ **Config sdk-path** (`alternative.exe -config sdk-path`)
3. ✅ **Otomatik indirme** (`%USERPROFILE%\.alternative-sdk\`)

**Sonuç:** Mevcut SDK kullanılır, gereksiz indirme olmaz!

---

## 📦 SDK Yönetimi

### SDK İndirme

```bash
# Android SDK
alternative.exe -sdk install 11076708           # SDK version
alternative.exe -sdk list                        # Yüklü SDK'ları listele

# NDK
alternative.exe -ndk install 26.1.10909125      # NDK version
alternative.exe -ndk list                        # Yüklü NDK'ları listele

# Flutter SDK
alternative.exe --flutter-sdk install 3.19.0    # Flutter version
alternative.exe --flutter-sdk list               # Yüklü Flutter SDK'ları listele
```

### Varsayılan SDK Konumu

- **Android SDK:** `%USERPROFILE%\.alternative-sdk\android\`
- **NDK:** `%USERPROFILE%\.alternative-sdk\ndk\`
- **Flutter SDK:** `%USERPROFILE%\.alternative-sdk\flutter\`
- **Config:** `%USERPROFILE%\.alternative-build\config.json`

---

## 📲 ADB ve Build Tools

### ADB Komutları

```bash
alternative.exe -adb devices                     # Cihazları listele
alternative.exe -adb install app.apk             # APK yükle
alternative.exe -adb uninstall com.example.app   # App kaldır
alternative.exe -adb logcat                      # Log görüntüle
alternative.exe -adb shell pm list packages      # Paketleri listele
alternative.exe -adb push local.txt /sdcard/     # Dosya gönder
alternative.exe -adb pull /sdcard/file.txt .     # Dosya al
```

### APK İmzalama

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

**List AVDs ve System Images:**
```bash
# Yüklü AVD'leri ve kullanılabilir system image'ları listele
alternative.exe -avd list

# Çıktı örneği:
# Installed AVDs:
#   Pixel_5 (API 34)
#   Nexus_5X (API 33)
#
# Available System Images (for creating new AVDs):
#   system-images;android-34;google_apis;x86_64
#   system-images;android-33;google_apis;x86_64
```

**AVD Oluşturma:**
```bash
# Yeni AVD oluştur (name, api-level)
alternative.exe -avd create Pixel_5 34        # API 34 (Android 14)
alternative.exe -avd create Nexus_5X 33       # API 33 (Android 13)
alternative.exe -avd create Tablet_10 34      # Tablet için

# Not: System image ANDROID_HOME/system-images/ altında olmalı
# Yoksa önce SDK Manager ile indirin:
# alternative.exe -sdk install <version>
```

**AVD Başlatma ve Durdurma:**
```bash
# AVD başlat
alternative.exe -avd start Pixel_5

# Emulator'u durdur (çalışan tüm emulator'lar)
alternative.exe -avd stop
```

**AVD Silme:**
```bash
# AVD'yi tamamen sil
alternative.exe -avd delete Pixel_5
```

**Emulator ile Test:**
```bash
# 1. AVD başlat
alternative.exe -avd start Pixel_5

# 2. Cihazları kontrol et
alternative.exe -adb devices

# 3. APK yükle
alternative.exe -adb install app-debug.apk

# 4. Logları izle
alternative.exe -adb logcat
```

**Not:** AVD yönetimi için ANDROID_HOME ayarlanmış olmalı ve Android SDK yüklü olmalıdır.

---

## 🌐 Cross-Platform Kullanım

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
# Tüm platformlar için build (requires .NET 10 SDK)
./build-all-platforms.sh         # Linux/macOS
build-all-platforms.bat          # Windows (tüm platformlar)
.\build-release.bat              # Windows only
```

**Çıktılar:**
- Windows: `alternative.exe` (~70 MB)
- Linux: `alternative` (~65 MB)
- macOS: `alternative` (~70 MB)
- **Android/Termux: `alternative` (~65 MB) - Linux ARM64**

### 📱 Termux/Android Desteği

**Alternative Build Tool, Termux üzerinde Android cihazlarda çalışabilir!**

```bash
# PC'de Android ARM64 için build
dotnet publish -c Release -r linux-arm64 --self-contained -p:PublishSingleFile=true

# Android'e transfer (ADB)
adb push alternative /sdcard/

# Termux'ta
mv /sdcard/alternative ~/
chmod +x ~/alternative
~/alternative -help
```

**Kullanım Senaryoları:**
- ✅ SDK yönetimi
- ✅ APK signing
- ✅ Hafif işlemler
- ⚠️ Gradle build sınırlı (performans)

**Detaylar:** [TERMUX_USAGE.md](TERMUX_USAGE.md)

---

## 🎬 Kullanım Senaryoları

### Senaryo 1: Sıfır Kurulum (Temiz Bilgisayar)

**Gereksinimler:**
- ✅ Sadece `alternative.exe`
- ✅ İnternet bağlantısı

**Çalışma:**
```bash
alternative.exe -kotlin build
# Program otomatik olarak SDK'yı indirir ve build yapar
```

**Avantaj:** Android Studio kurmanıza gerek yok!

### Senaryo 2: Mevcut Android Studio Var

**Yapılandırma:**
```bash
setx ANDROID_HOME "C:\Users\YourName\AppData\Local\Android\Sdk"
alternative.exe -kotlin build  # Mevcut SDK kullanılır
```

**Sonuç:** Tekrar indirme olmaz, mevcut SDK kullanılır.

### Senaryo 3: CI/CD Pipeline

```yaml
# GitHub Actions / Azure DevOps
- name: Build APK
  run: |
    alternative.exe -sdk install 11076708
    alternative.exe -kotlin build-release
```

SDK'lar cache'lenebilir:
```yaml
- uses: actions/cache@v3
  with:
    path: ~\.alternative-sdk
    key: android-sdk-${{ hashFiles('**/build.gradle') }}
```

### Senaryo 4: Portable Kullanım (USB)

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

**Sonuç:** Tüm setup USB'de, her bilgisayarda çalışır!

### Senaryo 5: Çoklu Proje (Farklı SDK)

```bash
cd ProjectA
alternative.exe -kotlin build  # SDK 33 otomatik

cd ProjectB  
alternative.exe -kotlin build  # SDK 34 otomatik
```

**Avantaj:** SDK çakışması yok, her proje bağımsız!

---

## 🛠️ Embedded Tools Ekleme

### ADB Eklemek İçin

1. [Android Platform Tools](https://developer.android.com/studio/releases/platform-tools) indir
2. `adb.exe`, `AdbWinApi.dll`, `AdbWinUsbApi.dll` dosyalarını çıkar
3. `AlternativeBuild\EmbeddedResources\tools\adb\` klasörüne kopyala
4. Projeyi rebuild et

### Build Tools Eklemek İçin

1. Android SDK Build Tools indir (34.0.0+)
2. `aapt2.exe`, `zipalign.exe`, `apksigner.jar` dosyalarını çıkar
3. `AlternativeBuild\EmbeddedResources\tools\build-tools\` klasörüne kopyala
4. `.csproj` dosyasında build action'ı "Embedded Resource" yap
5. Projeyi rebuild et

**Not:** Bu araçlar exe içine gömülür ve ilk çalıştırmada otomatik extract edilir.

---

## 🏗️ Proje Yapısı

```
AlternativeBuild/
├── Commands/          # CLI komutları
│   ├── FlutterBuildCommand.cs
│   ├── KotlinBuildCommand.cs
│   ├── SdkCommand.cs
│   ├── NdkCommand.cs
│   ├── FlutterSdkCommand.cs
│   ├── ConfigCommand.cs
│   ├── AdbCommand.cs
│   └── ApkSignerCommand.cs
├── Build/             # Build pipeline'ları
│   ├── GradleBuilder.cs
│   └── FlutterBuilder.cs
├── SDK/               # SDK yönetimi
│   ├── SdkManager.cs
│   ├── SdkDownloader.cs
│   └── ProjectInspector.cs
├── Tools/             # Embedded tools
│   ├── ToolsManager.cs
│   └── ResourceExtractor.cs
└── Utils/             # Yardımcı sınıflar
    ├── ConsoleLogger.cs
    ├── ProcessRunner.cs
    └── ConfigManager.cs
```

---

## 🔧 Geliştirici Bilgileri

### Gereksinimler
- .NET 10 SDK
- Windows 10/11, Linux (Ubuntu 20.04+), macOS (10.15+)

### Build
```bash
dotnet build
dotnet run -- -help
```

### Multi-Platform Build

**Tüm Platformlar (Linux/macOS):**
```bash
chmod +x build-all-platforms.sh
./build-all-platforms.sh
```

**Sadece Windows:**
```batch
build-release.bat
```

**Manuel Build:**
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

Tool otomatik olarak platformu tespit eder:

```csharp
// PlatformHelper.cs kullanımı
if (PlatformHelper.IsWindows)
{
    // Windows-specific kod
}
else if (PlatformHelper.IsLinux)
{
    // Linux-specific kod
}
else if (PlatformHelper.IsMacOS)
{
    // macOS-specific kod
}
```

**Platform-Specific Paths:**
- **Home:** Windows: `%USERPROFILE%`, Linux/macOS: `$HOME`
- **SDK:** `~/.alternative-sdk` (tüm platformlar)
- **Config:** `~/.alternative-build` (tüm platformlar)
- **Temp:** Windows: `%TEMP%`, Linux/macOS: `/tmp`

**Executable Extensions:**
- Windows: `.exe`, `.bat`
- Linux/macOS: (hiçbiri), `.sh`

### Test
```bash
# Debug mode
dotnet run -- -kotlin build

# Release build
.\build-release.bat
```

---

## ❓ Sorun Giderme

### "SDK not found" Hatası

**Çözüm 1:** ANDROID_HOME'u kontrol edin
```powershell
echo $env:ANDROID_HOME
# Boşsa: setx ANDROID_HOME "SDK_YOLUNUZ"
```

**Çözüm 2:** Config'i kontrol edin
```bash
alternative.exe -config get sdk-path
```

**Çözüm 3:** Manuel SDK yükleyin
```bash
alternative.exe -sdk install 11076708
```

### "Java not found" Hatası

```bash
# JAVA_HOME ayarla
setx JAVA_HOME "C:\Program Files\Java\jdk-17"
```

### Build Hatası

```bash
# Projeyi temizle
alternative.exe -kotlin clean
cd MyProject
gradlew clean
```

### ANDROID_HOME Ayarlandı Ama Kullanmıyor

```powershell
# PowerShell'i yeniden başlatın
# Veya bilgisayarı restart edin

# Test
echo $env:ANDROID_HOME
```

---

## 📚 Yardım

```bash
alternative.exe -help                    # Genel yardım
alternative.exe -kotlin -help            # Kotlin/Java yardımı
alternative.exe -flutter -help           # Flutter yardımı
alternative.exe -adb -help               # ADB yardımı
alternative.exe -apksigner -help         # APK signing yardımı
alternative.exe -sdk -help               # SDK yönetimi yardımı
```

---

## 🤝 Katkıda Bulunma

Katkılar memnuniyetle karşılanır! Lütfen pull request göndermeden önce:
1. Fork yapın
2. Feature branch oluşturun
3. Değişikliklerinizi commit edin
4. Push yapın ve PR açın

---

## 📄 Lisans

MIT License - Detaylar için [LICENSE](LICENSE) dosyasına bakın

---

## 🌟 Teşekkürler

- Android Open Source Project
- Flutter Team
- .NET Community

---

**Made with ❤️ for Android developers who prefer the command line**

---

## ✅ Özet

**Alternative Build Tool ile:**
- ✅ Android Studio olmadan Android uygulamaları build edin
- ✅ SDK'lar otomatik tespit edilir ve indirilir
- ✅ Mevcut Android Studio SDK'nızı kullanabilirsiniz
- ✅ Portable, tek executable
- ✅ Kotlin, Java, ve Flutter desteği
- ✅ ADB ve APK signing dahil

**Hemen başlayın:**
```bash
alternative.exe -kotlin build  # Bu kadar basit!
```

🎉 **Artık tamamen Android Studio'dan bağımsızsınız!**
