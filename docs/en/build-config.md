# Build Configuration - Alternative Build Tool

This guide explains build configuration and APK signing process with Alternative Build Tool.

---

## 🔨 Build Types

### Debug Build
```bash
alternative -kotlin build
# Output: app/build/outputs/apk/debug/app-debug.apk
```

### Release Build
```bash
alternative -kotlin build-release
# Output: app/build/outputs/apk/release/app-release.apk
```

---

## ✍️ APK Signing

### Creating Keystore
```bash
keytool -genkey -v -keystore my-release-key.keystore \
  -alias my-key-alias \
  -keyalg RSA \
  -keysize 2048 \
  -validity 10000
```

### Signed Build
```bash
alternative -kotlin build-signed \
  -keystore my-release-key.keystore \
  -alias my-key-alias \
  -storepass mypassword \
  -keypass mykeypassword
```

### Manual Signing
```bash
# Create unsigned APK
alternative -kotlin build-release

# Sign
alternative -apksigner sign \
  --ks my-release-key.keystore \
  --ks-key-alias my-key-alias \
  --out app-signed.apk \
  app-release.apk
```

---

## 🔍 APK Verification

```bash
# Verify signature
alternative -apksigner verify app-signed.apk

# Detailed info
alternative -apksigner verify --verbose app-signed.apk
```

---

## ⚙️ Gradle Configuration

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

## 📦 Build Variants

### Using Flavors
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

## 🚀 Optimization

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

### APK Size Reduction
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

### Clear Gradle Cache
```bash
# Windows
rmdir /s /q %USERPROFILE%\.gradle\caches

# Linux/macOS
rm -rf ~/.gradle/caches
```

---

## 📊 Build Performance

### Gradle Daemon
```bash
# gradle.properties
org.gradle.daemon=true
org.gradle.parallel=true
org.gradle.caching=true
```

### Build Time Analysis
```bash
alternative -kotlin build --scan
```

---

## 🐛 Troubleshooting

### Build Error: SDK Not Found
```bash
# Check ANDROID_HOME
echo %ANDROID_HOME%

# Install SDK
alternative -sdk browse
```

### Build Error: NDK Not Found
```bash
# Install NDK
alternative -ndk browse
```

### Signing Error
```bash
# Verify keystore
keytool -list -v -keystore my-release-key.keystore
```

### Out of Memory
```bash
# gradle.properties
org.gradle.jvmargs=-Xmx4096m -XX:MaxMetaspaceSize=512m
```

---

## 📚 Related Guides

- [SDK Management](sdk-management.md)
- [AVD & Emulator](avd-setup.md)
- [Termux Usage](termux-usage.md)
