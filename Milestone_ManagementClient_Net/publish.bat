@echo off
setlocal
echo ========================================================
echo Publishing Milestone XProtect Management Client Release
echo ========================================================

set CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe
set BIN_DIR=%~dp0bin
set PUB_DIR=%~dp0publish

if not exist "%BIN_DIR%" mkdir "%BIN_DIR%"
if not exist "%PUB_DIR%" mkdir "%PUB_DIR%"

echo [1/3] Compiling optimized x64 production binary...
"%CSC%" /t:winexe /optimize+ /platform:x64 "/win32icon:%~dp0Lychee.ico" "/out:%PUB_DIR%\MilestoneXProtect-ManagementClient.exe" /r:System.dll /r:System.Core.dll /r:System.Data.dll /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Xml.dll "/r:%~dp0Milestone_Core_Binaries\VideoOS.Management.Common.dll" "/r:%~dp0Milestone_Core_Binaries\VideoOS.Management.VmoClient.dll" "/r:%~dp0Milestone_Core_Binaries\VideoOS.Platform.dll" "/r:%~dp0Milestone_Core_Binaries\VideoOS.Platform.SDK.dll" "/r:%~dp0Milestone_Core_Binaries\VideoOS.Common.Controls.dll" "%~dp0Core\*.cs" "%~dp0Core\AccessControl\*.cs" "%~dp0Core\MediaProcessing\*.cs" "%~dp0Core\IncidentManager\*.cs" "%~dp0Core\SmartMap\*.cs" "%~dp0Core\Security\*.cs" "%~dp0Core\Failover\*.cs" "%~dp0Core\SCH\*.cs" "%~dp0Models\*.cs" "%~dp0Services\*.cs" "%~dp0UI\Forms\*.cs" "%~dp0UI\Dialogs\*.cs" "%~dp0UI\Views\*.cs" "%~dp0Program.cs"

if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Compilation failed!
    exit /b 1
)

echo [2/3] Publishing native C/C++ Milestone acceleration DLLs and runtime dependencies...
copy /Y "%~dp0Milestone_Core_Binaries\*.dll" "%PUB_DIR%\" >nul
copy /Y "%~dp0cctv_frame.png" "%PUB_DIR%\" >nul
copy /Y "%~dp0Lychee.ico" "%PUB_DIR%\" >nul
copy /Y "%~dp0Lychee.ico" "%PUB_DIR%\app.ico" >nul

echo [3/3] Synchronizing bin directory...
copy /Y "%PUB_DIR%\MilestoneXProtect-ManagementClient.exe" "%BIN_DIR%\MilestoneXProtect-ManagementClient.exe" >nul
copy /Y "%PUB_DIR%\MilestoneXProtect-ManagementClient.exe" "%BIN_DIR%\MilestoneXProtect-App.exe" >nul
copy /Y "%~dp0Milestone_Core_Binaries\*.dll" "%BIN_DIR%\" >nul
copy /Y "%~dp0cctv_frame.png" "%BIN_DIR%\" >nul
copy /Y "%~dp0Lychee.ico" "%BIN_DIR%\" >nul
copy /Y "%~dp0Lychee.ico" "%BIN_DIR%\app.ico" >nul

echo ========================================================
echo [SUCCESS] Standalone Executable Successfully Published to BIN!
echo Output Directory: %BIN_DIR%
echo Binary 1: %BIN_DIR%\MilestoneXProtect-ManagementClient.exe
echo Binary 2: %BIN_DIR%\MilestoneXProtect-App.exe
echo ========================================================
exit /b 0
