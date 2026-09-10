@echo off
setlocal
cd /d "%~dp0"

echo ========================================================
echo Starting Milestone XProtect Management Client 2025 R3
echo ========================================================

set EXE1="%~dp0publish\MilestoneXProtect-ManagementClient.exe"
set EXE2="%~dp0bin\MilestoneXProtect-ManagementClient.exe"
set EXE3="%~dp0bin\MilestoneXProtect-App.exe"

if exist %EXE1% (
    start "" %EXE1%
    exit /b 0
)

if exist %EXE2% (
    start "" %EXE2%
    exit /b 0
)

if exist %EXE3% (
    start "" %EXE3%
    exit /b 0
)

echo Executable not found, running build...
call "%~dp0build.bat"

if exist %EXE1% (
    start "" %EXE1%
    exit /b 0
)

if exist %EXE2% (
    start "" %EXE2%
    exit /b 0
)

echo [ERROR] Could not start application.
pause
exit /b 1
