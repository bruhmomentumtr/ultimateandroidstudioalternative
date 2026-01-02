# 🚀 AVD Hızlı Başlangıç - Sıfır Kurulum

Android Studio olmayan temiz bir bilgisayarda, sıfırdan Android Emulator kurulumu.

---

## ✅ İhtiyaçlar

- ✅ `alternative.exe` (~70-95 MB)
- ✅ İnternet bağlantısı
- ✅ ~2 GB boş disk alanı

**Hepsi bu kadar!** Android Studio gerekmez.

---

## 📝 Adım Adım Kurulum

### Adım 1: alternative.exe İndir

```bash
# İndirilen klasöre git
cd C:\Downloads

# veya PATH'e ekle (opsiyonel)
move alternative.exe C:\Tools\
setx PATH "%PATH%;C:\Tools"
```

### Adım 2: SDK Kur

```bash
# İnteraktif SDK listesi
alternative -sdk browse

# Ekranda:
# ★ 1. SDK 11076708 - Latest Stable (2024)
#   2. SDK 10406996 - 11.0 (2023)
#   ...
# Select version (1-10, or 0 to cancel): 

# 1 yazıp Enter
1

# SDK otomatik indirilir ve kurulur (~500 MB)
```

### Adım 3: AVD Otomatik Kur (TEK KOMUT!)

```bash
# Tek komut - her şeyi yapar!
alternative -avd setup --api 34 --name Pixel_5

# Program otomatik:
# 1. System image kontrol eder, yoksa indirir (~1.5 GB)
#    "Downloading system image for API 34..."
#    "This may take 10-20 minutes..."
#
# 2. Emulator kontrol eder, yoksa indirir (~150 MB)
#    "Installing emulator binary..."
#
# 3. AVD oluşturur
#    "Creating AVD: Pixel_5"
#
# 4. Başlatmak ister misiniz? sorar
#    "Start emulator now? (Y/n):"
```

**Y yazıp Enter → Emulator başlar!** 🎉

---

## ⚡ Hızlı Versiyon (Hepsi Otomatik)

```bash
# 1. SDK kur
alternative -sdk browse
# 1 seç

# 2. RESTART POWERSHELL (ANDROID_HOME için)

# 3. AVD kur ve başlat
alternative -avd setup --api 34 --name Pixel_5
# Y yaz (başlatmak için)
```

**Süre:** ~20-30 dakika (internetinize bağlı)

---

## 🔍 Adım Detayları

### System Image Seçimi

API seviyeleri:
- **API 34** - Android 14 (Önerilen)
- **API 33** - Android 13
- **API 32** - Android 12L
- **API 31** - Android 12

Farklı API için:
```bash
alternative -avd setup --api 33 --name Android13
```

### AVD İsimlendirme

İstediğiniz ismi verin:
```bash
alternative -avd setup --api 34 --name MyEmulator
alternative -avd setup --api 34 --name Pixel_5
alternative -avd setup --api 34 --name Tablet10
```

---

## 🎮 Emulator Kullanımı

### Başlatma
```bash
alternative -avd start Pixel_5
```

### Durdurma
```bash
alternative -avd stop
```

### APK Yükleme
```bash
# Emulator çalışırken
alternative -adb install app.apk
```

### AVD Silme
```bash
alternative -avd delete Pixel_5
```

---

## 🐛 Sorun Giderme

### "ANDROID_HOME not set" Hatası

**Çözüm:** PowerShell'i yeniden başlat

```bash
# PowerShell'i kapat ve tekrar aç
# Sonra tekrar dene
alternative -avd setup --api 34 --name Pixel_5
```

### "System image not found" Hatası

**Manuel system image kurulumu:**
```bash
alternative -system-image browse
# API seviyesi seç (örn: 1 = API 34)
1
```

### "Emulator not found" Hatası

**Manuel emulator kurulumu:**
```bash
alternative -emulator install
```

### Yavaş İndirme

Normal! Dosyalar büyük:
- System image: ~1.5 GB
- Emulator: ~150 MB

10-20 dakika sürebilir.

---

## ✅ Başarı Kontrolü

```bash
# AVD listele
alternative -avd list

# Çıktı:
# Installed AVDs:
#   Pixel_5 (API 34)
#
# Available System Images:
#   system-images;android-34;google_apis;x86_64
```

Gördüyseniz → Başarılı! ✨

---

## 🎯 Özet

**3 Adımda Emulator:**

1. `alternative -sdk browse` → 1 seç
2. PowerShell yeniden başlat
3. `alternative -avd setup --api 34 --name Pixel_5` → Y

**Hepsi bu! Android Studio gerektirmez.** 🚀

---

## 📚 Detaylı Bilgi

- [AVD Setup Guide](avd-setup.md) - Detaylı AVD kılavuzu
- [SDK Management](sdk-management.md) - SDK yönetimi
- [README (Ana Sayfa)](../../README.md)
