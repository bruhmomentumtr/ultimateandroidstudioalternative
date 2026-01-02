@echo off
echo ============================================
echo   Copying Tools from Android Studio SDK
echo   Enhanced Version - All Required Files
echo ============================================
echo.

set SDK_PATH=D:\androidstudioappdata\sdk
set TARGET=AlternativeBuild\EmbeddedResources\tools

if not exist "%SDK_PATH%" (
    echo ERROR: Android SDK not found at %SDK_PATH%
    echo.
    echo Please update SDK_PATH in this script to your Android SDK location.
    echo Common locations:
    echo   - %LOCALAPPDATA%\Android\Sdk
    echo   - C:\Android\Sdk
    echo   - D:\androidstudioappdata\sdk
    pause
    exit /b 1
)

echo SDK Path: %SDK_PATH%
echo Target: %TARGET%
echo.

REM Create directories
echo Creating directories...
mkdir "%TARGET%\adb" 2>nul
mkdir "%TARGET%\build-tools" 2>nul
mkdir "%TARGET%\platform-tools" 2>nul

REM ===========================
REM PART 1: ADB (Platform Tools)
REM ===========================
echo.
echo [1/3] Copying Platform Tools (ADB)...
echo.

if not exist "%SDK_PATH%\platform-tools\adb.exe" (
    echo WARNING: Platform tools not found
    echo Install with: sdkmanager "platform-tools"
) else (
    copy "%SDK_PATH%\platform-tools\adb.exe" "%TARGET%\adb\" /Y
    copy "%SDK_PATH%\platform-tools\AdbWinApi.dll" "%TARGET%\adb\" /Y
    copy "%SDK_PATH%\platform-tools\AdbWinUsbApi.dll" "%TARGET%\adb\" /Y
    
    REM Optional: Fastboot
    if exist "%SDK_PATH%\platform-tools\fastboot.exe" (
        echo   + Including fastboot...
        copy "%SDK_PATH%\platform-tools\fastboot.exe" "%TARGET%\platform-tools\" /Y
    )
    
    echo   ✓ ADB copied successfully
)

REM ===========================
REM PART 2: Build Tools
REM ===========================
echo.
echo [2/3] Copying Build Tools...
echo.

REM Find latest build-tools version
for /f "delims=" %%i in ('dir /b /ad /o-n "%SDK_PATH%\build-tools" 2^>nul') do (
    set BUILD_TOOLS=%%i
    goto :found_build_tools
)

echo ERROR: No build-tools found
echo Install with: sdkmanager "build-tools;34.0.0"
goto :skip_build_tools

:found_build_tools
echo Using build-tools version: %BUILD_TOOLS%
echo.

REM Core build tools
copy "%SDK_PATH%\build-tools\%BUILD_TOOLS%\aapt2.exe" "%TARGET%\build-tools\" /Y 2>nul
if errorlevel 1 (
    echo WARNING: aapt2.exe not found
) else (
    echo   ✓ aapt2.exe
)

copy "%SDK_PATH%\build-tools\%BUILD_TOOLS%\zipalign.exe" "%TARGET%\build-tools\" /Y 2>nul
if errorlevel 1 (
    echo WARNING: zipalign.exe not found
) else (
    echo   ✓ zipalign.exe
)

REM apksigner (check both locations)
if exist "%SDK_PATH%\build-tools\%BUILD_TOOLS%\lib\apksigner.jar" (
    copy "%SDK_PATH%\build-tools\%BUILD_TOOLS%\lib\apksigner.jar" "%TARGET%\build-tools\" /Y
    echo   ✓ apksigner.jar (from lib/)
) else if exist "%SDK_PATH%\build-tools\%BUILD_TOOLS%\apksigner.jar" (
    copy "%SDK_PATH%\build-tools\%BUILD_TOOLS%\apksigner.jar" "%TARGET%\build-tools\" /Y
    echo   ✓ apksigner.jar
) else (
    echo WARNING: apksigner.jar not found
)

REM Additional useful tools
if exist "%SDK_PATH%\build-tools\%BUILD_TOOLS%\aidl.exe" (
    copy "%SDK_PATH%\build-tools\%BUILD_TOOLS%\aidl.exe" "%TARGET%\build-tools\" /Y 2>nul
    echo   + aidl.exe
)

if exist "%SDK_PATH%\build-tools\%BUILD_TOOLS%\d8.bat" (
    copy "%SDK_PATH%\build-tools\%BUILD_TOOLS%\d8.bat" "%TARGET%\build-tools\" /Y 2>nul
    copy "%SDK_PATH%\build-tools\%BUILD_TOOLS%\lib\d8.jar" "%TARGET%\build-tools\" /Y 2>nul
    echo   + d8 (dex compiler)
)

:skip_build_tools

REM ===========================
REM PART 3: Summary
REM ===========================
echo.
echo [3/3] Verifying copied files...
echo.

echo ============================================
echo   SUMMARY
echo ============================================
echo.

echo Platform Tools (ADB):
dir /b "%TARGET%\adb\" 2>nul | findstr /v "README" || echo   (empty)
echo.

echo Build Tools:
dir /b "%TARGET%\build-tools\" 2>nul | findstr /v "README" || echo   (empty)
echo.

echo ============================================
echo   NEXT STEPS
echo ============================================
echo.
echo 1. Files have been copied to:
echo    %TARGET%
echo.
echo 2. These files are embedded as resources in .csproj:
echo    ^<EmbeddedResource Include="EmbeddedResources\**\*" /^>
echo.
echo 3. Rebuild the project:
echo    dotnet clean
echo    dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
echo.
echo 4. The executable will be ~25MB larger with embedded tools
echo.
echo ============================================

pause
