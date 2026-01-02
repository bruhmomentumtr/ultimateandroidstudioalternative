@echo off
echo ============================================
echo   Collecting Build Outputs
echo ============================================
echo.

if not exist build-output mkdir build-output

echo Copying files...
copy AlternativeBuild\bin\Release\net10.0\win-x64\publish\alternative.exe build-output\alternative-windows-x64.exe >nul 2>&1
copy AlternativeBuild\bin\Release\net10.0\linux-x64\publish\alternative build-output\alternative-linux-x64 >nul 2>&1
copy AlternativeBuild\bin\Release\net10.0\osx-x64\publish\alternative build-output\alternative-macos-x64 >nul 2>&1
copy AlternativeBuild\bin\Release\net10.0\osx-arm64\publish\alternative build-output\alternative-macos-arm64 >nul 2>&1
copy AlternativeBuild\bin\Release\net10.0\linux-arm64\publish\alternative build-output\alternative-android-arm64 >nul 2>&1

echo.
echo ============================================
echo   FILES COLLECTED!
echo ============================================
echo.
echo Build outputs:
dir /b build-output\alternative-* 2>nul
echo.
echo Full details:
dir build-output\alternative-*
echo.
echo Location: build-output\
echo.
