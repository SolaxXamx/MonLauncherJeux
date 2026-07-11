@echo off
setlocal enabledelayedexpansion

REM Script de generation d'un EXE portable
REM Optimise pour .NET 8.0 SDK x86

set "PROJECT_FILE=MonLauncherJeux.csproj"
set "PUBLISH_DIR=publish"
set "RUNTIME=win-x86"

cd /d "%~dp0"

cls
echo.
echo Generation de l'EXE Windows x86
echo ==================================
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

echo ERREUR: .NET 8.0 SDK n'a pas ete detecte
echo.
echo Installe .NET 8.0 SDK x86 depuis:
echo https://dotnet.microsoft.com/download/dotnet/8.0
echo.
pause
exit /b 1

:found_dotnet
echo [OK] .NET SDK detecte: !DOTNET_PATH!
echo.

echo [...] Suppression de l'ancien dossier publish...
if exist "%PUBLISH_DIR%" (
    rmdir /s /q "%PUBLISH_DIR%" >nul 2>nul
)

echo [...] Compilation et publication en mode Release...
echo     Cela peut prendre 1-2 minutes la premiere fois...
echo.

"!DOTNET_PATH!" publish "%PROJECT_FILE%" -c Release -r %RUNTIME% --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false

if errorlevel 1 (
    echo.
    echo ERREUR lors de la publication
    echo.
    pause
    exit /b 1
)

echo.
echo [OK] Publication reussie!
echo.
echo EXE genere: %PUBLISH_DIR%\MonLauncherJeux.exe
echo Taille approximative: 50-100 MB
echo.
echo Tu peux maintenant:
echo 1. Double-cliquer sur l'EXE pour lancer le launcher
echo 2. Copier le dossier 'publish' sur un PC sans .NET
echo.
pause
exit /b 0
