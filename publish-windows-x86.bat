@echo off
setlocal enabledelayedexpansion
chcp 65001 >nul 2>nul

REM ========================================
REM Script de génération d'un EXE portable
REM ========================================

set "PROJECT_FILE=MonLauncherJeux.csproj"
set "PUBLISH_DIR=publish"
set "RUNTIME=win-x86"

cd /d "%~dp0"

cls
echo.
echo ╔════════════════════════════════════════════════════════════╗
echo ║     📦 Génération de l'EXE Windows x86 ^(32-bit^)         ║
echo ╚════════════════════════════════════════════════════════════╝
echo.

where dotnet >nul 2>nul
if errorlevel 1 (
    echo ❌ ERREUR: .NET SDK n'est pas installé
    echo Installe-le depuis: https://dotnet.microsoft.com/download/dotnet/8.0
    echo.
    pause
    exit /b 1
)

echo ⏳ Préparation de la publication...
echo.

REM Supprime l'ancien dossier publish
if exist "%PUBLISH_DIR%" (
    echo 🗑️  Suppression de l'ancien dossier publish...
    rmdir /s /q "%PUBLISH_DIR%" >nul 2>nul
)

echo.
echo 🔨 Compilation en mode Release...
dotnet publish "%PROJECT_FILE%" -c Release -r %RUNTIME% --self-contained true -p:PublishSingleFile=true -p:SelfContained=true -p:PublishTrimmed=false

if errorlevel 1 (
    echo.
    echo ❌ ERREUR lors de la publication
    echo.
    pause
    exit /b 1
)

echo.
echo ✅ Publication réussie!
echo.
echo 📂 EXE généré: %PUBLISH_DIR%\MonLauncherJeux.exe
echo 💾 Taille approximative: ~100-150 MB ^(incluant le runtime .NET^)
echo.
echo 🚀 Tu peux maintenant:
echo    1. Double-cliquer sur l'EXE pour lancer le launcher
echo    2. Copier le dossier 'publish' sur un PC 32 bits sans .NET
echo.
echo 📝 Note: Cet EXE est autonome et ne nécessite pas .NET d'être installé
echo.
pause
exit /b 0
