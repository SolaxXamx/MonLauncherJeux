@echo off
setlocal enabledelayedexpansion
chcp 65001 >nul 2>nul

REM ========================================
REM Script de restauration des dépendances
REM Optimisé pour .NET 8.0 SDK x86
REM ========================================

set "PROJECT_FILE=MonLauncherJeux.csproj"

cd /d "%~dp0"

cls
echo.
echo ╔════════════════════════════════════════════════════════════════════════════════╗
echo ║   📥 Restauration des dépendances NuGet                    ║
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

echo ⏳ Restauration des packages NuGet...
echo.

"!DOTNET_PATH!" restore "%PROJECT_FILE%"

if errorlevel 1 (
    echo.
    echo ❌ ERREUR lors de la restauration
    echo.
    pause
    exit /b 1
)

echo.
echo ✅ Restauration réussie!
echo.
echo 💡 Maintenant tu peux lancer: run-launcher.bat
echo.
pause
exit /b 0
