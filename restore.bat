@echo off
setlocal enabledelayedexpansion
chcp 65001 >nul 2>nul

REM ========================================
REM Script de restauration des dépendances
REM ========================================

set "PROJECT_FILE=MonLauncherJeux.csproj"

cd /d "%~dp0"

cls
echo.
echo ╔════════════════════════════════════════════════════════════╗
echo ║   📥 Restauration des dépendances NuGet                    ║
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

echo ⏳ Restauration des packages NuGet...
echo.

dotnet restore "%PROJECT_FILE%"

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
pause
exit /b 0
