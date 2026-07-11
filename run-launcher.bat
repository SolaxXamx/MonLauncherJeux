@echo off
setlocal enabledelayedexpansion

REM Script de lancement du launcher
REM Optimise pour .NET 8.0 SDK x86
REM Sans caracteres speciaux (UTF-8)

set "PROJECT_FILE=MonLauncherJeux.csproj"
cd /d "%~dp0"

cls
echo.
echo Mon Launcher Premium - Lancement
echo ===================================
echo.

REM Cherche dotnet
set "DOTNET_PATH="

where dotnet >nul 2>nul
if not errorlevel 1 (
    for /f "tokens=*" %%A in ('where dotnet') do set "DOTNET_PATH=%%A"
    goto :found_dotnet
)

REM Essai x86
if exist "C:\Program Files (x86)\dotnet\dotnet.exe" (
    set "DOTNET_PATH=C:\Program Files (x86)\dotnet\dotnet.exe"
    goto :found_dotnet
)

REM Essai x64
if exist "C:\Program Files\dotnet\dotnet.exe" (
    set "DOTNET_PATH=C:\Program Files\dotnet\dotnet.exe"
    goto :found_dotnet
)

echo ERREUR: .NET 8.0 SDK n'a pas ete detecte
echo.
echo Solutions:
echo 1. Installe .NET 8.0 SDK x86 depuis:
echo    https://dotnet.microsoft.com/download/dotnet/8.0
echo.
echo 2. Relance ce script apres l'installation
echo.
echo 3. Ou lance directement:
echo    "C:\Program Files (x86)\dotnet\dotnet.exe" run --project MonLauncherJeux.csproj
echo.
pause
exit /b 1

:found_dotnet
echo [OK] .NET SDK detecte: !DOTNET_PATH!
echo.

REM Affiche version
for /f "tokens=*" %%A in ('"!DOTNET_PATH!" --version 2^>nul') do set "DOTNET_VERSION=%%A"
echo [OK] Version: %DOTNET_VERSION%
echo.

if not exist "%PROJECT_FILE%" (
    echo ERREUR: '%PROJECT_FILE%' non trouve
    pause
    exit /b 1
)

echo [...] Restauration des packages...
"!DOTNET_PATH!" restore "%PROJECT_FILE%" --verbosity quiet

if errorlevel 1 (
    echo ERREUR lors de la restauration
    pause
    exit /b 1
)

echo [...] Compilation et lancement...
echo.

"!DOTNET_PATH!" run --project "%PROJECT_FILE%" --configuration Release --no-restore

if errorlevel 1 (
    echo.
    echo ERREUR lors du lancement
    echo.
    echo Essaye:
    echo "!DOTNET_PATH!" clean
    echo "!DOTNET_PATH!" build -c Release
    echo.
    pause
    exit /b 1
)

exit /b 0
