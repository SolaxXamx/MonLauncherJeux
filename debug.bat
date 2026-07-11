@echo off
setlocal enabledelayedexpansion

REM Script de diagnostic
REM Affiche les informations systeme

cls
echo.
echo Diagnostic du systeme
echo =====================
echo.

echo Verification des emplacements .NET:
echo.

REM Verifie x86
if exist "C:\Program Files (x86)\dotnet\dotnet.exe" (
    echo [OK] .NET x86 trouve: C:\Program Files (x86)\dotnet\dotnet.exe
    echo.
    for /f "tokens=*" %%A in ('"C:\Program Files (x86)\dotnet\dotnet.exe" --version') do echo        Version: %%A
    echo.
) else (
    echo [NON] .NET x86 NON trouve: C:\Program Files (x86)\dotnet
    echo.
)

REM Verifie x64
if exist "C:\Program Files\dotnet\dotnet.exe" (
    echo [OK] .NET x64 trouve: C:\Program Files\dotnet\dotnet.exe
    echo.
    for /f "tokens=*" %%A in ('"C:\Program Files\dotnet\dotnet.exe" --version') do echo        Version: %%A
    echo.
) else (
    echo [NON] .NET x64 NON trouve: C:\Program Files\dotnet
    echo.
)

REM Verifie via PATH
echo Verification via PATH:
where dotnet >nul 2>nul
if not errorlevel 1 (
    for /f "tokens=*" %%A in ('where dotnet') do (
        echo [OK] Trouve via PATH: %%A
        "%%A" --version
    )
    echo.
) else (
    echo [NON] dotnet non trouve dans PATH
    echo.
)

echo Fichiers du projet:
if exist "MonLauncherJeux.csproj" (
    echo [OK] MonLauncherJeux.csproj trouve
) else (
    echo [NON] MonLauncherJeux.csproj NON trouve
)

echo.
echo Si tu vois des [NON], il faut installer .NET 8.0 SDK:
echo    https://dotnet.microsoft.com/download/dotnet/8.0
echo.
pause
