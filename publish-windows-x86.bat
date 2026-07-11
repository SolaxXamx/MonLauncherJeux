@echo off
setlocal enabledelayedexpansion
chcp 65001 >nul 2>nul

REM ========================================
REM Script de génération d'un EXE portable
REM Optimisé pour .NET 8.0 SDK x86
REM ========================================

set "PROJECT_FILE=MonLauncherJeux.csproj"
set "PUBLISH_DIR=publish"
set "RUNTIME=win-x86"

cd /d "%~dp0"

cls
echo.
echo ╔════════════════════════════════════════════════════════════════════════════════╗
echo ║     📦 Génération de l'EXE Windows x86 ^(32-bit^)         ║
echo ╚════════════════════════════════════════════════════════════════════════════════╝
echo.

REM Cherche dotnet
set "DOTNET_PATH="

where dotnet >nul 2>nul
if not errorlevel 1 (
    for /f "tokens=*" %%A in ('where dotnet') do set "DOTNET_PATH=%%A"
    goto :found_dotnet
)

if exist "C:\Program Files (x86)\dotnet\dotnet.exe" (
    set "DOTNET_PATH=C:\Program Files (x86)\dotnet\dotnet.exe"
    goto :found_dotnet
)

if exist "C:\Program Files\dotnet\dotnet.exe" (
    set "DOTNET_PATH=C:\Program Files\dotnet\dotnet.exe"
    goto :found_dotnet
)

echo ❌ ERREUR: .NET 8.0 SDK n'a pas pu être détecté
echo.
echo Installe .NET 8.0 SDK x86 depuis:
echo https://dotnet.microsoft.com/download/dotnet/8.0
echo.
pause
exit /b 1

:found_dotnet
echo ✓ .NET SDK détecté: !DOTNET_PATH!
echo.

echo ⏳ Préparation de la publication...
echo.

REM Supprime l'ancien dossier publish
if exist "%PUBLISH_DIR%" (
    echo 🗑️  Suppression de l'ancien dossier publish...
    rmdir /s /q "%PUBLISH_DIR%" >nul 2>nul
)

echo.
echo 🔨 Compilation et publication en mode Release...
echo    Cela peut prendre 1-2 minutes la première fois...
echo.

"!DOTNET_PATH!" publish "%PROJECT_FILE%" -c Release -r %RUNTIME% --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false

if errorlevel 1 (
    echo.
    echo ❌ ERREUR lors de la publication
    echo.
    echo 💡 Essaye:
    echo "!DOTNET_PATH!" clean
    echo "!DOTNET_PATH!" restore
    echo.
    pause
    exit /b 1
)

echo.
echo ✅ Publication réussie!
echo.
echo 📂 EXE généré: %PUBLISH_DIR%\MonLauncherJeux.exe
echo 💾 Taille approximative: ~50-100 MB
echo.
echo 🚀 Tu peux maintenant:
echo    1. Double-cliquer sur l'EXE pour lancer le launcher
echo    2. Copier le dossier 'publish' sur un PC sans .NET
echo.
echo 📝 Note: Cet EXE est autonome et ne nécessite pas .NET d'être installé
echo.
pause
exit /b 0
