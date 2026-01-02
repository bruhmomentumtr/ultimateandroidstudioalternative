# Build Configuration - Alternative Build Tool

Bu kılavuz, Alternative Build Tool ile build konfigürasyonu ve APK imzalama süreçlerini açıklar.

---

## 🔨 Build Türleri

### Debug Build
```bash
alternative -kotlin build
# Çıktı: app/build/outputs/apk/debug/app-debug.apk
```

### Release Build
```bash
alternative -kotlin build-release
# Çıktı: app/build/outputs/apk/release/app-release.apk
```

---

## ✍️ APK İmzalama

### Keystore Oluşturma
```bash
keytool -genkey -v -keystore my-release-key.keystore \
  -alias my-key-alias \
  -keyalg RSA \
  -keysize 2048 \
  -validity 10000
```

### İmzalı Build
```bash
alternative -kotlin build-signed \
  -keystore my-release-key.keystore \
  -alias my-key-alias \
  -storepass mypassword \
  -keypass mykeypassword
```

### Manuel İmzalama
```bash
# Unsigned APK oluştur
alternative -kotlin build-release

# İmzala
alternative -apksigner sign \
  --ks my-release-key.keystore \
  --ks-key-alias my-key-alias \
  --out app-signed.apk \
  app-release.apk
```

---

## 🔍 APK Doğrulama

```bash
# İmza doğrulama
alternative -apksigner verify app-signed.apk

# Detaylı bilgi
alternative -apksigner verify --verbose app-signed.apk
```

---

## ⚙️ Gradle Konfigürasyonu

### build.gradle (app)
```gradle
android {
    compileSdk 34
    
    defaultConfig {
        applicationId "com.example.myapp"
        minSdk 21
        targetSdk 34
        versionCode 1
        versionName "1.0"
    }
    
    buildTypes {
        release {
            minifyEnabled true
            proguardFiles getDefaultProguardFile('proguard-android-optimize.txt'), 'proguard-rules.pro'
        }
    }
    
    signingConfigs {
        release {
            storeFile file("my-release-key.keystore")
            storePassword "mypassword"
            keyAlias "my-key-alias"
            keyPassword "mykeypassword"
        }
    }
}
```

---

## 📦 Build Varyantları

### Flavor Kullanımı
```gradle
android {
    flavorDimensions "version"
    productFlavors {
        free {
            dimension "version"
            applicationIdSuffix ".free"
        }
        paid {
            dimension "version"
            applicationIdSuffix ".paid"
        }
    }
}
```

```bash
# Free debug
alternative -kotlin build -variant freeDebug

# Paid release
alternative -kotlin build-release -variant paidRelease
```

---

## 🚀 Optimizasyon

### ProGuard/R8
```gradle
android {
    buildTypes {
        release {
            minifyEnabled true
            shrinkResources true
        }
    }
}
```

### APK Boyutu Küçültme
```gradle
android {
    splits {
        abi {
            enable true
            reset()
            include "armeabi-v7a", "arm64-v8a", "x86", "x86_64"
            universalApk false
        }
    }
}
```

---

## 🔧 Build Cache

### Clean Build
```bash
alternative -kotlin clean
alternative -kotlin build
```

### Gradle Cache Temizleme
```bash
# Windows
rmdir /s /q %USERPROFILE%\.gradle\caches

# Linux/macOS
rm -rf ~/.gradle/caches
```

---

## 📊 Build Performansı

### Gradle Daemon
```bash
# gradle.properties
org.gradle.daemon=true
org.gradle.parallel=true
org.gradle.caching=true
```

### Build Süresi Analizi
```bash
alternative -kotlin build --scan
```

---

## 🐛 Sorun Giderme

### Build Hatası: SDK Bulunamadı
```bash
# ANDROID_HOME kontrol
echo %ANDROID_HOME%

# SDK kur
alternative -sdk browse
```

### Build Hatası: NDK Bulunamadı
```bash
# NDK kur
alternative -ndk browse
```

### İmzalama Hatası
```bash
# Keystore doğrula
keytool -list -v -keystore my-release-key.keystore
```

### Out of Memory
```bash
# gradle.properties
org.gradle.jvmargs=-Xmx4096m -XX:MaxMetaspaceSize=512m
```

---

## 📚 İlgili Kılavuzlar

- [SDK Management](sdk-management.md)
- [AVD & Emulator](avd-setup.md)
- [Termux Usage](termux-usage.md)
