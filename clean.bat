@echo off
setlocal enabledelayedexpansion

REM Script de nettoyage du projet

cd /d "%~dp0"

cls
echo.
echo Nettoyage du projet
echo ====================
echo.

echo [...] Suppression des fichiers temporaires...
echo.

if exist "bin" (
    echo Suppression de bin...
    rmdir /s /q "bin" >nul 2>nul
)

if exist "obj" (
    echo Suppression de obj...
    rmdir /s /q "obj" >nul 2>nul
)

if exist "publish" (
    echo Suppression de publish...
    rmdir /s /q "publish" >nul 2>nul
)

if exist ".vs" (
    echo Suppression de .vs (cache Visual Studio)...
    rmdir /s /q ".vs" >nul 2>nul
)

echo.
echo [OK] Nettoyage termine!
echo.
echo Ensuite, relance: run-launcher.bat
echo.
pause
exit /b 0
