# Tamamen Bağımsız Kurulum - Embedded Tools Kılavuzu

## 🎯 Amaç

Alternative Build Tool'u **hiçbir dış bağımlılık olmadan** çalıştırabilmek için gerekli tüm araçları executable içine gömeceğiz.

---

## 📦 Gömülecek Araçlar

### 1. ADB (Android Debug Bridge) - **ZORUNLU**
- **Boyut:** ~5 MB
- **Gerekli dosyalar:**
  - `adb.exe`
  - `AdbWinApi.dll`
  - `AdbWinUsbApi.dll`

### 2. Build Tools - **ZORUNLU**
- **Boyut:** ~20 MB
- **Gerekli dosyalar:**
  - `aapt2.exe` (~10 MB)
  - `zipalign.exe` (~500 KB)
  - `apksigner.jar` (~1 MB)
  - `lib/apksigner.jar` (alternatif konum)

### 3. Java Runtime (JRE) - **İSTEĞE BAĞLI**
- **Boyut:** ~40-50 MB (minimal JRE)
- **Neden gerekli:** `apksigner.jar` çalıştırmak için
- **Not:** Sistem Java'sı varsa gerekli değil

---

## 🔧 Adım Adım Kurulum

### ADIM 1: ADB Dosyalarını İndir

**Kaynak:** [Android Platform Tools](https://developer.android.com/studio/releases/platform-tools)

**İndirme:**
```bash
# Windows için
https://dl.google.com/android/repository/platform-tools-latest-windows.zip

# Veya mevcut Android Studio'dan kopyala
D:\androidstudioappdata\sdk\platform-tools\
```

**Gerekli Dosyalar:**
1. `adb.exe`
2. `AdbWinApi.dll`
3. `AdbWinUsbApi.dll`

**Hedef Konum:**
```
AlternativeBuild\EmbeddedResources\tools\adb\
├── adb.exe
├── AdbWinApi.dll
└── AdbWinUsbApi.dll
```

### ADIM 2: Build Tools Dosyalarını İndir

**Kaynak:** Android SDK Build Tools

**Mevcut SDK'dan Kopyalama:**
```bash
# Eğer Android Studio yüklüyse
D:\androidstudioappdata\sdk\build-tools\34.0.0\

# Veya en son sürümü indir
https://developer.android.com/studio#command-line-tools-only
```

**Gerekli Dosyalar:**
1. `aapt2.exe`
2. `zipalign.exe`
3. `apksigner.jar` (lib klasöründe olabilir: `lib\apksigner.jar`)

**Hedef Konum:**
```
AlternativeBuild\EmbeddedResources\tools\build-tools\
├── aapt2.exe
├── zipalign.exe
└── apksigner.jar
```

### ADIM 3: .csproj Dosyasını Güncelle

**Dosya:** `AlternativeBuild\AlternativeBuild.csproj`

Şu satırları ekle:
```xml
<ItemGroup>
  <!-- ADB Files -->
  <EmbeddedResource Include="EmbeddedResources\tools\adb\adb.exe" />
  <EmbeddedResource Include="EmbeddedResources\tools\adb\AdbWinApi.dll" />
  <EmbeddedResource Include="EmbeddedResources\tools\adb\AdbWinUsbApi.dll" />
  
  <!-- Build Tools -->
  <EmbeddedResource Include="EmbeddedResources\tools\build-tools\aapt2.exe" />
  <EmbeddedResource Include="EmbeddedResources\tools\build-tools\zipalign.exe" />
  <EmbeddedResource Include="EmbeddedResources\tools\build-tools\apksigner.jar" />
</ItemGroup>
```

### ADIM 4: Rebuild

```bash
# Clean ve rebuild
dotnet clean -c Release
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

---

## 📊 Boyut Karşılaştırması

| Konfigürasyon | Exe Boyutu | Açıklama |
|---------------|------------|----------|
| **Minimal** (Embedded yok) | ~67 MB | ANDROID_HOME gerekli |
| **ADB Only** | ~72 MB | ADB bağımsız |
| **ADB + Build Tools** | ~92 MB | Tam bağımsız |
| **Full (Java + All)** | ~140 MB | Her şey dahil |

**Önerilen:** ADB + Build Tools (~92 MB)

---

## ✅ Doğrulama

### Test 1: Temiz Windows VM
```bash
# ANDROID_HOME yokken
alternative.exe -adb devices
# Çalışmalı!

alternative.exe -apksigner verify app.apk
# Çalışmalı!
```

### Test 2: Tool Extraction Kontrolü
```bash
# İlk çalıştırmada
%TEMP%\alternative-build-tools\
├── adb\
│   ├── adb.exe
│   ├── AdbWinApi.dll
│   └── AdbWinUsbApi.dll
└── build-tools\
    ├── aapt2.exe
    ├── zipalign.exe
    └── apksigner.jar
```

---

## 🔍 Komut Dosyası (Otomatik Kopyalama)

Eğer Android Studio yüklüyse, bu script dosyaları otomatik kopyalar:

```batch
@echo off
echo Copying tools from Android Studio SDK...

set SDK_PATH=D:\androidstudioappdata\sdk
set TARGET=AlternativeBuild\EmbeddedResources\tools

REM ADB
mkdir %TARGET%\adb 2>nul
copy "%SDK_PATH%\platform-tools\adb.exe" "%TARGET%\adb\" /Y
copy "%SDK_PATH%\platform-tools\AdbWinApi.dll" "%TARGET%\adb\" /Y
copy "%SDK_PATH%\platform-tools\AdbWinUsbApi.dll" "%TARGET%\adb\" /Y

REM Build Tools (latest version)
for /f "delims=" %%i in ('dir /b /ad /o-n "%SDK_PATH%\build-tools" 2^>nul') do (
    set BUILD_TOOLS=%%i
    goto :found
)
:found

mkdir %TARGET%\build-tools 2>nul
copy "%SDK_PATH%\build-tools\%BUILD_TOOLS%\aapt2.exe" "%TARGET%\build-tools\" /Y
copy "%SDK_PATH%\build-tools\%BUILD_TOOLS%\zipalign.exe" "%TARGET%\build-tools\" /Y
copy "%SDK_PATH%\build-tools\%BUILD_TOOLS%\lib\apksigner.jar" "%TARGET%\build-tools\" /Y

echo Done! Files copied to %TARGET%
echo.
echo Now rebuild the project:
echo   dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

**Kaydet:** `copy-tools-from-sdk.bat`

---

## ⚠️ Önemli Notlar

### Java Gereksinimi
- `apksigner.jar` çalıştırmak için **Java gerekli**
- Kullanıcı bilgisayarında Java yoksa:
  - Hata mesajı: "Java not found"
  - Çözüm: Kullanıcıya Java kurmasını söyle
  - **Veya:** Portable JRE embed et (boyut +40 MB)

### Lisans Uyumluluğu
- Apache 2.0 License (Android tools)
- Embed edilebilir
- Attribution gerekli (LICENSE dosyasında belirt)

### Güncelleme
- Araçlar güncellendikçe yeni versiyonları embed et
- Build tools her yıl güncellenir
- ADB daha seyrek güncellenir

---

## 🎯 Sonuç

Bu adımları tamamladıktan sonra:
- ✅ Alternative.exe tamamen bağımsız
- ✅ Hiçbir ANDROID_HOME gerekliliği yok
- ✅ Temiz Windows'ta çalışır
- ✅ Portable USB kullanımı
- ✅ CI/CD ortamlarında sorunsuz

**Final boyut:** ~92 MB (ADB + Build Tools)
