# Cross-Platform Usage - Alternative Build Tool

Alternative Build Tool, Windows, Linux, macOS ve Android/Termux platformlarında çalışır.

---

## 🪟 Windows

### Kurulum
```bash
# İndir
alternative-windows-x64.exe

# PATH'e ekle (opsiyonel)
setx PATH "%PATH%;C:\path\to\alternative"

# Kullan
alternative -help
```

### PowerShell vs CMD
Her ikisinde de çalışır:
```powershell
# PowerShell
.\alternative.exe -kotlin build

# CMD
alternative.exe -kotlin build
```

---

## 🐧 Linux

### Kurulum
```bash
# İndir
wget https://github.com/.../alternative-linux-x64

# Executable yap
chmod +x alternative-linux-x64

# PATH'e ekle (opsiyonel)
sudo mv alternative-linux-x64 /usr/local/bin/alternative

# Kullan
alternative -help
```

### Gereksinimler
- glibc 2.31+ (Ubuntu 20.04+, Debian 11+)
- libstdc++ (genelde yüklü)

### Doğrulama
```bash
# glibc versiyonu
ldd --version

# Kütüphane kontrol
ldd alternative-linux-x64
```

---

## 🍎 macOS

### Intel (x86_64)
```bash
# İndir
curl -O https://github.com/.../alternative-macos-x64

# Executable yap
chmod +x alternative-macos-x64

# Gatekeeper bypass (ilk çalıştırma)
xattr -d com.apple.quarantine alternative-macos-x64

# PATH'e ekle
sudo mv alternative-macos-x64 /usr/local/bin/alternative
```

### Apple Silicon (ARM64)
```bash
# ARM64 versiyonu indir
curl -O https://github.com/.../alternative-macos-arm64

chmod +x alternative-macos-arm64
xattr -d com.apple.quarantine alternative-macos-arm64
sudo mv alternative-macos-arm64 /usr/local/bin/alternative
```

### Gatekeeper Uyarısı
İlk çalıştırmada:
```
System Preferences → Security & Privacy → Allow
```

---

## 📱 Android/Termux

### Kurulum
```bash
# Termux'ta
pkg update
pkg install proot-distro wget

# Alternative indir
wget https://github.com/.../alternative-android-arm64
chmod +x alternative-android-arm64

# Alias oluştur
echo 'alias alternative="~/alternative-android-arm64"' >> ~/.bashrc
source ~/.bashrc
```

### Sınırlamalar
- ✅ Build işlemleri çalışır
- ✅ SDK yönetimi çalışır
- ❌ Emulator çalışmaz (ARM64)
- ✅ ADB fiziksel cihazlarla çalışır

Detay: [Termux Usage Guide](termux-usage.md)

---

## 🐳 Docker

### Dockerfile
```dockerfile
FROM mcr.microsoft.com/dotnet/runtime:10.0

# Alternative kopyala
COPY alternative-linux-x64 /usr/local/bin/alternative
RUN chmod +x /usr/local/bin/alternative

# Java (Gradle için)
RUN apt-get update && apt-get install -y openjdk-17-jdk

# Workspace
WORKDIR /workspace
VOLUME /workspace

ENTRYPOINT ["alternative"]
```

### Kullanım
```bash
docker build -t alternative-build .
docker run -v $(pwd):/workspace alternative-build -kotlin build
```

---

## ☁️ CI/CD

### GitHub Actions
```yaml
name: Android Build

on: [push]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Download Alternative
        run: |
          wget https://github.com/.../alternative-linux-x64
          chmod +x alternative-linux-x64
      
      - name: Build APK
        run: ./alternative-linux-x64 -kotlin build-release
      
      - name: Upload APK
        uses: actions/upload-artifact@v3
        with:
          name: app-release
          path: app/build/outputs/apk/release/*.apk
```

### GitLab CI
```yaml
build-android:
  image: ubuntu:22.04
  script:
    - apt-get update && apt-get install -y wget
    - wget https://github.com/.../alternative-linux-x64
    - chmod +x alternative-linux-x64
    - ./alternative-linux-x64 -kotlin build-release
  artifacts:
    paths:
      - app/build/outputs/apk/release/*.apk
```

---

## 🔧 Platform Farkları

### Path Separator
```bash
# Windows
alternative -config sdk-path "C:\Android\Sdk"

# Linux/macOS
alternative -config sdk-path "/home/user/Android/Sdk"
```

### Environment Variables
```bash
# Windows
setx ANDROID_HOME "C:\Android\Sdk"

# Linux/macOS
export ANDROID_HOME="$HOME/Android/Sdk"
```

### Script Extensions
- Windows: `.bat`, `.cmd`, `.ps1`
- Linux/macOS: `.sh` (executable)

---

## 🐛 Platform-Specific Issues

### Linux: Permission Denied
```bash
chmod +x alternative-linux-x64
```

### macOS: Quarantine
```bash
xattr -d com.apple.quarantine alternative-macos-arm64
```

### Windows: Antivirus
Bazı antivirüs programları exe'yi engelleyebilir - istisna ekleyin.

---

## 📊 Performans

| Platform | Build Hızı | Executable Boyut |
|----------|------------|------------------|
| Windows x64 | 1.0x (baseline) | ~72 MB |
| Linux x64 | 1.1x | ~65 MB |
| macOS ARM64 | 0.9x (faster!) | ~65 MB |
| Android ARM64 | 0.7x (slower) | ~65 MB |

---

## 📚 İlgili Kılavuzlar

- [Termux Detailed Usage](termux-usage.md)
- [Build Configuration](build-config.md)
- [SDK Management](sdk-management.md)
