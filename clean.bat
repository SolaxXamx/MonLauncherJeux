@echo off
setlocal enabledelayedexpansion
chcp 65001 >nul 2>nul

REM ========================================
REM Script de nettoyage du projet
REM Optimisé pour .NET 8.0 SDK x86
REM ========================================

cd /d "%~dp0"

cls
echo.
echo ╔════════════════════════════════════════════════════════════════════════════════╗
echo ║          🧹 Nettoyage du projet                             ║
echo ╚════════════════════════════════════════════════════════════════════════════════╝
echo.

echo ⏳ Suppression des fichiers temporaires...
echo.

REM Supprime les répertoires de build
if exist "bin" (
    echo 🗑️  Suppression du dossier bin...
    rmdir /s /q "bin" >nul 2>nul
)

if exist "obj" (
    echo 🗑️  Suppression du dossier obj...
    rmdir /s /q "obj" >nul 2>nul
)

if exist "publish" (
    echo 🗑️  Suppression du dossier publish...
    rmdir /s /q "publish" >nul 2>nul
)

REM Nettoie les fichiers de cache
if exist ".vs" (
    echo 🗑️  Suppression du dossier .vs (cache Visual Studio)...
    rmdir /s /q ".vs" >nul 2>nul
)

echo.
echo ✅ Nettoyage terminé!
echo.
echo 💡 Ensuite, relance: run-launcher.bat
echo.
pause
exit /b 0
