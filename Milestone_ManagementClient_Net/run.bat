@echo off
set EXE="%~dp0bin\MilestoneXProtect-ManagementClient.exe"
if not exist %EXE% (
    call "%~dp0build.bat"
)
start "" %EXE%
