#!/bin/bash

echo "============================================"
echo "  Building Alternative Build Tool (Multi-Platform)"
echo "============================================"
echo ""

cd AlternativeBuild

echo "Cleaning previous builds..."
dotnet clean -c Release

echo ""
echo "Building for Windows x64..."
dotnet publish -c Release -r win-x64 \
  --self-contained true \
  -p:PublishSingleFile=true \
  -p:IncludeNativeLibrariesForSelfExtract=true \
  -p:EnableCompressionInSingleFile=true \
  -p:DebugType=none \
  -p:DebugSymbols=false

echo ""
echo "Building for Linux x64..."
dotnet publish -c Release -r linux-x64 \
  --self-contained true \
  -p:PublishSingleFile=true \
  -p:IncludeNativeLibrariesForSelfExtract=true \
  -p:EnableCompressionInSingleFile=true \
  -p:DebugType=none \
  -p:DebugSymbols=false

echo ""
echo "Building for macOS x64..."
dotnet publish -c Release -r osx-x64 \
  --self-contained true \
  -p:PublishSingleFile=true \
  -p:IncludeNativeLibrariesForSelfExtract=true \
  -p:EnableCompressionInSingleFile=true \
  -p:DebugType=none \
  -p:DebugSymbols=false

echo ""
echo "Building for macOS ARM64..."
dotnet publish -c Release -r osx-arm64 \
  --self-contained true \
  -p:PublishSingleFile=true \
  -p:IncludeNativeLibrariesForSelfExtract=true \
  -p:EnableCompressionInSingleFile=true \
  -p:DebugType=none \
  -p:DebugSymbols=false

echo ""
echo "Building for Android/Termux (Linux ARM64)..."
dotnet publish -c Release -r linux-arm64 \
  --self-contained true \
  -p:PublishSingleFile=true \
  -p:IncludeNativeLibrariesForSelfExtract=true \
  -p:EnableCompressionInSingleFile=true \
  -p:DebugType=none \
  -p:DebugSymbols=false

echo ""
echo "============================================"
echo "  BUILD SUCCESSFUL!"
echo "============================================"
echo ""

echo "Collecting outputs to build-output directory..."
mkdir -p build-output

cp AlternativeBuild/bin/Release/net10.0/win-x64/publish/alternative.exe build-output/alternative-windows-x64.exe 2>/dev/null || true
cp AlternativeBuild/bin/Release/net10.0/linux-x64/publish/alternative build-output/alternative-linux-x64 2>/dev/null || true
cp AlternativeBuild/bin/Release/net10.0/osx-x64/publish/alternative build-output/alternative-macos-x64 2>/dev/null || true
cp AlternativeBuild/bin/Release/net10.0/osx-arm64/publish/alternative build-output/alternative-macos-arm64 2>/dev/null || true
cp AlternativeBuild/bin/Release/net10.0/linux-arm64/publish/alternative build-output/alternative-android-arm64 2>/dev/null || true

# Make non-Windows executables executable
chmod +x build-output/alternative-linux-x64 2>/dev/null || true
chmod +x build-output/alternative-linux-arm64 2>/dev/null || true
chmod +x build-output/alternative-macos-x64 2>/dev/null || true
chmod +x build-output/alternative-macos-arm64 2>/dev/null || true

echo ""
echo "============================================"
echo "  ALL BUILDS COLLECTED!"
echo "============================================"
echo ""
echo "Build outputs:"
ls -lh build-output/alternative-* 2>/dev/null || true
echo ""
echo "Location: build-output/"
echo ""
echo "Files:"
echo "  alternative-windows-x64.exe    - Windows x64"
echo "  alternative-linux-x64          - Linux x64"
echo "  alternative-linux-arm64        - Android/Termux ARM64"
echo "  alternative-macos-x64          - macOS Intel"
echo "  alternative-macos-arm64        - macOS Apple Silicon"
echo ""
