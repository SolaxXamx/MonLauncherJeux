@echo off
setlocal enabledelayedexpansion
chcp 65001 >nul 2>nul

REM ========================================
REM Script de lancement du launcher moderne
REM ========================================

set "LAUNCHER_NAME=Mon Launcher Premium"
set "PROJECT_FILE=MonLauncherJeux.csproj"
set "PUBLISH_DIR=publish"

REM Détecte le répertoire du script
cd /d "%~dp0"

cls
echo.
echo ╔════════════════════════════════════════════════════════════╗
echo ║          🎮 %LAUNCHER_NAME%                  ║
echo ╚════════════════════════════════════════════════════════════╝
echo.

REM Vérifie si on lance depuis l'exe publié
if exist "MonLauncherJeux.exe" (
    echo ✓ Mode: Exécutable autonome
    echo ✓ Lancement de l'application...
    echo.
    start "" "MonLauncherJeux.exe"
    exit /b 0
)

REM Vérifie si dotnet est installé
where dotnet >nul 2>nul
if errorlevel 1 (
    echo ❌ ERREUR: .NET n'est pas installé ou introuvable
    echo.
    echo 📋 Options:
    echo.
    echo 1. Installe le .NET 8 SDK depuis:
    echo    https://dotnet.microsoft.com/download/dotnet/8.0
    echo.
    echo 2. Sélectionne l'installation pour ton système:
    echo    • Windows x86 ^(32-bit^) pour PC 32 bits
    echo    • Windows x64 ^(64-bit^) pour PC 64 bits
    echo.
    echo 3. Relance ce script après l'installation.
    echo.
    echo 🔧 OU: Génère un .exe portable avec publish-windows-x86.bat
    echo.
    pause
    exit /b 1
)

REM Affiche la version de dotnet
for /f "tokens=*" %%A in ('dotnet --version 2^>nul') do set "DOTNET_VERSION=%%A"
echo ✓ Mode: Développement
echo ✓ .NET SDK détecté: %DOTNET_VERSION%
echo.

REM Vérifie si le projet existe
if not exist "%PROJECT_FILE%" (
    echo ❌ ERREUR: Fichier projet '%PROJECT_FILE%' introuvable
    echo.
    echo Assure-toi que tu es dans le bon répertoire.
    echo.
    pause
    exit /b 1
)

echo ⏳ Compilation et lancement du launcher...
echo.

REM Lance l'application
dotnet run --project "%PROJECT_FILE%" --configuration Release

if errorlevel 1 (
    echo.
    echo ❌ ERREUR lors du lancement
    echo.
    echo 💡 Essaie ceci:
    echo dotnet restore
    echo dotnet build
    echo.
    pause
    exit /b 1
)

exit /b 0
