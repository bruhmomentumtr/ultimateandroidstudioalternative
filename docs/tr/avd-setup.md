# Android Studio Olmadan Tam AVD/Emulator Kurulumu

## 🎯 Hedef

Alternative Build Tool ile **sıfırdan** emulator kurulumu - hiçbir Android Studio olmadan!

---

## 📋 Gerekli Bileşenler

### 1. Android SDK Command-Line Tools ✅ (Mevcut)
```bash
alternative -sdk install 11076708
```

### 2. Platform Tools (ADB) ✅ (Embedded)
- Zaten exe içinde

### 3. Build Tools ✅ (Embedded)
- Zaten exe içinde

### 4. Emulator Binary ❌ (Eklenecek)
```bash
# Gelecek özellik
alternative -emulator install
```

### 5. System Image ❌ (Eklenecek)
```bash
# Gelecek özellik  
alternative -system-image install android-34 google_apis x86_64
```

### 6. Platform SDK ❌ (Kısmen var)
```bash
# Gelecek özellik - platform SDK indirme
alternative -platform install android-34
```

---

## 🚀 Şu Anki Durum (v1.0.0)

### ✅ Çalışan:
```bash
# 1. SDK kur
alternative -sdk install 11076708

# 2. ANDROID_HOME ayarla
setx ANDROID_HOME "%USERPROFILE%\.alternative-sdk\android\cmdline-tools-11076708"

# 3. Manuel olarak system image indir
# (sdkmanager kullanarak - komut satırından)

# 4. AVD oluştur
alternative -avd create Pixel_5 34

# 5. AVD başlat
alternative -avd start Pixel_5
```

### ❌ Eksik (Manuel Gerekli):
- Emulator binary indirme
- System image indirme
- Platform SDK indirme

---

## 🔧 Manuel Çözüm (Şu An İçin)

### ADIM 1: SDK Kur
```bash
alternative -sdk install 11076708
```

### ADIM 2: ANDROID_HOME Ayarla
```bash
setx ANDROID_HOME "%USERPROFILE%\.alternative-sdk\android\cmdline-tools-11076708"
```

### ADIM 3: sdkmanager ile Eksikleri İndir

**Windows:**
```bash
cd %USERPROFILE%\.alternative-sdk\android\cmdline-tools-11076708\bin

# Emulator kur
sdkmanager.bat "emulator"

# System image kur
sdkmanager.bat "system-images;android-34;google_apis;x86_64"

# Platform kur
sdkmanager.bat "platforms;android-34"

# Build tools (zaten embedded ama isterseniz)
sdkmanager.bat "build-tools;34.0.0"
```

**Linux/macOS:**
```bash
cd ~/.alternative-sdk/android/cmdline-tools-11076708/bin

./sdkmanager "emulator"
./sdkmanager "system-images;android-34;google_apis;x86_64"
./sdkmanager "platforms;android-34"
```

### ADIM 4: AVD Oluştur
```bash
alternative -avd create Pixel_5 34
```

### ADIM 5: AVD Başlat
```bash
alternative -avd start Pixel_5
```

---

## 🎯 Gelecek Özellikler (v1.1.0)

### Tam Otomatik Emulator Kurulumu

Planlanan komutlar:

```bash
# Tek komut - her şeyi kur
alternative -emulator setup --api 34

# Veya adım adım:
alternative -emulator install                              # Emulator binary
alternative -system-image install android-34 google_apis   # System image
alternative -platform install android-34                   # Platform SDK
alternative -avd create Pixel_5 34                         # AVD oluştur
alternative -avd start Pixel_5                             # Başlat
```

### Implementasyon Planı

**1. EmulatorCommand.cs** (Yeni)
```csharp
public class EmulatorCommand : ICommand
{
    public async Task<int> InstallAsync()
    {
        // sdkmanager kullanarak emulator indir
        // %ANDROID_HOME%/emulator/ altına
    }
}
```

**2. SystemImageCommand.cs** (Yeni)
```csharp
public class SystemImageCommand : ICommand
{
    public async Task<int> InstallAsync(string api, string variant, string arch)
    {
        // sdkmanager kullanarak system image indir
        // %ANDROID_HOME%/system-images/android-{api}/{variant}/{arch}/
    }
    
    public async Task<int> ListAsync()
    {
        // Yüklü system image'ları listele
    }
}
```

**3. PlatformCommand.cs** (Yeni)
```csharp
public class PlatformCommand : ICommand
{
    public async Task<int> InstallAsync(string api)
    {
        // Platform SDK indir
        // %ANDROID_HOME%/platforms/android-{api}/
    }
}
```

---

## 📦 Boyut Etkileri

| Bileşen | Boyut | Gereklilik |
|---------|-------|------------|
| Emulator Binary | ~150 MB | AVD çalıştırmak için |
| System Image (x86_64) | ~1-1.5 GB | AVD çalıştırmak için |
| Platform SDK | ~50 MB | Build için (AVD için opsiyonel) |
| **Toplam** | **~1.2-1.7 GB** | İlk AVD kurulumu için |

**Not:** Bu dosyalar exe'ye gömülmez, indirilerek `%USERPROFILE%\.alternative-sdk\` altına kaydedilir.

---

## ✅ Sonuç

**Şu anki durum:**
- ✅ Alternative ile SDK kurulabilir
- ✅ AVD yönetimi yapılabilir
- ⚠️ Emulator/system image manuel indirilmeli (sdkmanager ile)
- ✅ AVD başlatılabilir

**v1.1.0'da:**
- ✅ Tam otomatik emulator kurulumu
- ✅ Tek komutla sıfırdan AVD hazırlama
- ✅ System image yönetimi

**Şu an için workaround:**
Manuel olarak sdkmanager kullanarak emulator ve system image indirin (yukarıdaki ADIM 3).
