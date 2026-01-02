# Alternative Build Tool - Termux/Android Kullanımı

## 📱 Android/Termux Desteği

Alternative Build Tool, Termux üzerinde Android cihazlarda çalışabilir!

### Gereksinimler

1. **Termux** uygulaması (F-Droid veya Google Play)
2. **.NET Runtime** (Termux'ta kurulacak)
3. **ARM64 cihaz** (çoğu modern Android cihaz)

---

## 🚀 Kurulum

### 1. Termux'u Kur

```bash
# F-Droid'den Termux indir: https://f-droid.org/en/packages/com.termux/
# Veya Google Play'den
```

### 2. Termux'ta Gerekli Paketleri Kur

```bash
# Termux'u güncelle
pkg update && pkg upgrade

# Gerekli paketler
pkg install wget tar

# (İsteğe bağlı) Git
pkg install git

# (İsteğe bağlı) Build için .NET SDK
# Not: Termux'ta .NET SDK resmi olarak desteklenmez
# Ancak runtime çalışabilir
```

### 3. Alternative Binary İndir

```bash
# Linux ARM64 versiyonunu indir (bilgisayardan build et)
# Termux'a transfer et:

# Seçenek 1: USB kablo ile
# PC'de build et:
dotnet publish -c Release -r linux-arm64 --self-contained -p:PublishSingleFile=true

# Çıktıyı Android'e kopyala
# Windows'ta: adb push alternative /sdcard/
# Termux'ta: cp /sdcard/alternative ~/
chmod +x ~/alternative

# Seçenek 2: SSH/SCP ile
# Termux'ta SSH server kur:
pkg install openssh
sshd
# PC'den SCP ile gönder
```

---

## 📦 Kullanım (Termux'ta)

### Alternative'i Çalıştır

```bash
# Termux ana dizininde
~/alternative -help

# PATH'e ekle
export PATH=$PATH:~
alternative -help
```

### Android Projesi Build Et

```bash
# Termux'ta proje klasörüne git
cd ~/myproject

# Build yap
alternative -kotlin build
```

### SDK Yönetimi

```bash
# SDK yükle
alternative -sdk install 11076708

# SDK konumu (Termux'ta)
export ANDROID_HOME=~/.alternative-sdk/android
```

---

## ⚠️ Sınırlamalar

### 1. Gradlew Çalışmıyor
- Gradle wrapper Android'de sorun yaşayabilir
- **Çözüm:** PC'de build edin, Termux'ta sadece SDK yönetimi yapın

### 2. SDK İndirme Yavaş Olabilir
- Mobil bağlantı yavaş olabilir
- **Çözüm:** WiFi kullanın

### 3. Emulator Çalışmaz
- Android cihazda emulator çalıştıramazsınız
- **Çözüm:** Fiziksel cihazı kullanın (ADB self-connect)

### 4. Performans
- ARM64 performansı x64'ten düşük olabilir
- **Önerilen:** Hafif projeler için kullanın

---

## 💡 Kullanım Senaryoları

### Senaryo 1: Hızlı SDK Yönetimi

```bash
# Termux'ta SDK indir
alternative -sdk install 11076708
alternative -ndk install 26.1.10909125

# Android Studio'da kullan
# Settings -> SDK Location -> ~/.alternative-sdk/android
```

### Senaryo 2: APK İmzalama

```bash
# APK'yı Android'e kopyala
# Termux'ta imzala
alternative -apksigner sign --ks my.jks --out signed.apk app.apk
```

### Senaryo 3: ADB Self-Connect

```bash
# Termux'ta ADB kullan (root gerekli)
alternative -adb devices
alternative -adb logcat
```

---

## 🔧 Alternatif: Proot-Distro ile

Daha iyi .NET desteği için Proot-Distro kullanın:

```bash
# Termux'ta Ubuntu kur
pkg install proot-distro
proot-distro install ubuntu

# Ubuntu'ya gir
proot-distro login ubuntu

# .NET 10 SDK kur
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 10.0

# Alternative'i build et veya çalıştır
dotnet ~/alternative.dll
```

---

## ✅ Termux Build Komutu

PC'de Termux için build:

```bash
# Linux ARM64 (Android ARM64)
dotnet publish -c Release -r linux-arm64 --self-contained -p:PublishSingleFile=true

# Çıktı: AlternativeBuild/bin/Release/net10.0/linux-arm64/publish/alternative
```

Android'e transfer:

```bash
# ADB ile
adb push alternative /sdcard/
# Termux'ta: mv /sdcard/alternative ~/

# Veya cloud (Termux'ta wget)
wget https://your-server.com/alternative
chmod +x alternative
```

---

## 🎯 Özet

**Termux Desteği:**
- ✅ Teorik olarak çalışır (Linux ARM64)
- ✅ SDK yönetimi yapabilir
- ✅ APK signing yapabilir
- ⚠️ Gradle build sınırlı
- ⚠️ Performans düşük olabilir

**Önerilen Kullanım:**
- SDK yönetimi
- APK signing
- Hafif işlemler

**Tam build için:**
- PC kullanın (Windows/Linux/macOS)
- Termux'u yardımcı araç olarak kullanın

---

## 📚 Kaynaklar

- [Termux Wiki](https://wiki.termux.com/)
- [.NET on Linux ARM64](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Proot-Distro](https://github.com/termux/proot-distro)
