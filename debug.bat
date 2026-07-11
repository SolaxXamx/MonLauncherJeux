@echo off
setlocal enabledelayedexpansion
chcp 65001 >nul 2>nul

REM ========================================
REM Script de débogage
REM Affiche les informations système
REM ========================================

cls
echo.
echo ╔════════════════════════════════════════════════════════════════════════════════╗
echo ║          🔍 Diagnostic du système                           ║
echo ╚════════════════════════════════════════════════════════════════════════════════╝
echo.

echo 📋 Vérification des emplacements .NET:
echo.

REM Vérifie x86
if exist "C:\Program Files (x86)\dotnet\dotnet.exe" (
    echo ✓ .NET x86 trouvé: C:\Program Files ^(x86^)\dotnet\dotnet.exe
    echo.
    for /f "tokens=*" %%A in ('"C:\Program Files (x86)\dotnet\dotnet.exe" --version') do echo   Version: %%A
    echo.
) else (
    echo ✗ .NET x86 NON trouvé: C:\Program Files ^(x86^)\dotnet
    echo.
)

REM Vérifie x64
if exist "C:\Program Files\dotnet\dotnet.exe" (
    echo ✓ .NET x64 trouvé: C:\Program Files\dotnet\dotnet.exe
    echo.
    for /f "tokens=*" %%A in ('"C:\Program Files\dotnet\dotnet.exe" --version') do echo   Version: %%A
    echo.
) else (
    echo ✗ .NET x64 NON trouvé: C:\Program Files\dotnet
    echo.
)

REM Vérifie via PATH
echo 📍 Vérification via PATH:
where dotnet >nul 2>nul
if not errorlevel 1 (
    for /f "tokens=*" %%A in ('where dotnet') do (
        echo ✓ Trouvé via PATH: %%A
        "%%A" --version
    )
    echo.
) else (
    echo ✗ dotnet non trouvé dans PATH
    echo.
)

echo 📂 Fichiers du projet:
if exist "MonLauncherJeux.csproj" (
    echo ✓ MonLauncherJeux.csproj trouvé
) else (
    echo ✗ MonLauncherJeux.csproj NON trouvé
)

echo.
echo 🔧 Si tu vois des ❌, il faut installer .NET 8.0 SDK:
echo    https://dotnet.microsoft.com/download/dotnet/8.0
echo.
pause
