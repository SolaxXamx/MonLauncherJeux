@echo off
setlocal enabledelayedexpansion
chcp 65001 >nul 2>nul

REM ========================================
REM Script de lancement du launcher
REM Optimisé pour .NET 8.0 SDK x86
REM ========================================

set "LAUNCHER_NAME=Mon Launcher Premium"
set "PROJECT_FILE=MonLauncherJeux.csproj"

REM Détecte le répertoire du script
cd /d "%~dp0"

cls
echo.
echo ╔════════════════════════════════════════════════════════════════════════════════╗
echo ║          🎮 %LAUNCHER_NAME%                  ║
echo ╚════════════════════════════════════════════════════════════════════════════════╝
echo.

REM Cherche dotnet dans les emplacements standards
set "DOTNET_PATH="

REM Essai 1: En variable d'environnement
where dotnet >nul 2>nul
if not errorlevel 1 (
    for /f "tokens=*" %%A in ('where dotnet') do set "DOTNET_PATH=%%A"
    goto :found_dotnet
)

REM Essai 2: Installation x86 par défaut
if exist "C:\Program Files (x86)\dotnet\dotnet.exe" (
    set "DOTNET_PATH=C:\Program Files (x86)\dotnet\dotnet.exe"
    goto :found_dotnet
)

REM Essai 3: Installation x64 par défaut
if exist "C:\Program Files\dotnet\dotnet.exe" (
    set "DOTNET_PATH=C:\Program Files\dotnet\dotnet.exe"
    goto :found_dotnet
)

REM Si pas trouvé
echo ❌ ERREUR: .NET 8.0 SDK n'a pas pu être détecté
echo.
echo 📋 Essaye ceci:
echo.
echo 1. Installe .NET 8.0 SDK x86 depuis:
echo    https://dotnet.microsoft.com/download/dotnet/8.0
echo.
echo    Choisis bien "Windows x86" car tu utilises le SDK 32-bit
echo.
echo 2. Après installation, relance ce script
echo.
echo 3. Ou vérifie manuellement:
echo    Appuie sur Win + R et tape: C:\Program Files (x86)\dotnet\dotnet.exe --version
echo.
pause
exit /b 1

:found_dotnet
echo ✓ .NET SDK détecté: !DOTNET_PATH!
echo.

REM Affiche la version
for /f "tokens=*" %%A in ('"!DOTNET_PATH!" --version 2^>nul') do set "DOTNET_VERSION=%%A"
echo ✓ Version: %DOTNET_VERSION%
echo.

REM Vérifie le projet
if not exist "%PROJECT_FILE%" (
    echo ❌ ERREUR: '%PROJECT_FILE%' non trouvé
    echo.
    echo Assure-toi d'être dans le bon répertoire
    echo.
    pause
    exit /b 1
)

echo ⏳ Restauration des packages...
echo.
"!DOTNET_PATH!" restore "%PROJECT_FILE%" --verbosity quiet

if errorlevel 1 (
    echo.
    echo ❌ ERREUR lors de la restauration
    echo.
    pause
    exit /b 1
)

echo.
echo ⏳ Compilation et lancement...
echo.

"!DOTNET_PATH!" run --project "%PROJECT_FILE%" --configuration Release --no-restore

if errorlevel 1 (
    echo.
    echo ❌ ERREUR lors du lancement
    echo.
    echo 💡 Essaye ceci:
    echo "!DOTNET_PATH!" clean
    echo "!DOTNET_PATH!" build -c Release
    echo.
    pause
    exit /b 1
)

exit /b 0
