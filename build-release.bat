@echo off
echo ============================================
echo   Building Alternative Build Tool (Release)
echo ============================================
echo.

cd /d AlternativeBuild

echo Cleaning previous build...
dotnet clean -c Release
if errorlevel 1 goto error

echo.
echo Building single-file executable...
dotnet publish -c Release -r win-x64 ^
  --self-contained true ^
  -p:PublishSingleFile=true ^
  -p:IncludeNativeLibrariesForSelfExtract=true ^
  -p:EnableCompressionInSingleFile=true ^
  -p:DebugType=none ^
  -p:DebugSymbols=false

if errorlevel 1 goto error

echo.
echo ============================================
echo   BUILD SUCCESSFUL!
echo ============================================
echo.
echo Output: AlternativeBuild\bin\Release\net10.0\win-x64\publish\alternative.exe
echo.

dir /s AlternativeBuild\bin\Release\net10.0\win-x64\publish\alternative.exe

goto end

:error
echo.
echo ============================================
echo   BUILD FAILED!
echo ============================================
exit /b 1

:end
