@echo off
setlocal
cd /d "%~dp0"

set EXE="%~dp0..\publish\Milestone-LicenseGenerator.exe"
if not exist %EXE% (
    call "%~dp0build_licgen.bat"
)

if exist %EXE% (
    start "" %EXE%
    exit /b 0
)

start "" "%~dp0..\bin\Milestone-LicenseGenerator.exe"
