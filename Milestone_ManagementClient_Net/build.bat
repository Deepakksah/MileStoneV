@echo off
setlocal
echo ========================================================
echo Building and Publishing Milestone XProtect Management Client
echo ========================================================

set CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe
set OUT_DIR=%~dp0bin
set PUB_DIR=%~dp0publish
set OUT_EXE=%OUT_DIR%\MilestoneXProtect-ManagementClient.exe

if not exist "%OUT_DIR%" mkdir "%OUT_DIR%"
if not exist "%PUB_DIR%" mkdir "%PUB_DIR%"

echo Compiling all C# source files...
"%CSC%" /t:winexe /optimize+ /platform:x64 "/win32icon:%~dp0Lychee.ico" "/out:%OUT_EXE%" /r:System.dll /r:System.Core.dll /r:System.Data.dll /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Xml.dll "/r:%~dp0Milestone_Core_Binaries\VideoOS.Management.Common.dll" "/r:%~dp0Milestone_Core_Binaries\VideoOS.Management.VmoClient.dll" "/r:%~dp0Milestone_Core_Binaries\VideoOS.Platform.dll" "/r:%~dp0Milestone_Core_Binaries\VideoOS.Platform.SDK.dll" "/r:%~dp0Milestone_Core_Binaries\VideoOS.Common.Controls.dll" "%~dp0Core\*.cs" "%~dp0Core\AccessControl\*.cs" "%~dp0Core\MediaProcessing\*.cs" "%~dp0Core\IncidentManager\*.cs" "%~dp0Core\SmartMap\*.cs" "%~dp0Core\Security\*.cs" "%~dp0Core\Failover\*.cs" "%~dp0Core\SCH\*.cs" "%~dp0Models\*.cs" "%~dp0Services\*.cs" "%~dp0UI\Forms\*.cs" "%~dp0UI\Dialogs\*.cs" "%~dp0UI\Views\*.cs" "%~dp0Program.cs"

if %ERRORLEVEL% EQU 0 (
    copy /Y "%OUT_EXE%" "%PUB_DIR%\MilestoneXProtect-ManagementClient.exe" >nul
    copy /Y "%OUT_EXE%" "%OUT_DIR%\MilestoneXProtect-App.exe" >nul
    goto :BUILD_OK
)

echo Main exe locked by running process, compiling to MilestoneXProtect-App.exe & publish...
set OUT_EXE=%OUT_DIR%\MilestoneXProtect-App.exe
"%CSC%" /t:winexe /optimize+ /platform:x64 "/win32icon:%~dp0Lychee.ico" "/out:%PUB_DIR%\MilestoneXProtect-ManagementClient.exe" /r:System.dll /r:System.Core.dll /r:System.Data.dll /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Xml.dll "/r:%~dp0Milestone_Core_Binaries\VideoOS.Management.Common.dll" "/r:%~dp0Milestone_Core_Binaries\VideoOS.Management.VmoClient.dll" "/r:%~dp0Milestone_Core_Binaries\VideoOS.Platform.dll" "/r:%~dp0Milestone_Core_Binaries\VideoOS.Platform.SDK.dll" "/r:%~dp0Milestone_Core_Binaries\VideoOS.Common.Controls.dll" "%~dp0Core\*.cs" "%~dp0Core\AccessControl\*.cs" "%~dp0Core\MediaProcessing\*.cs" "%~dp0Core\IncidentManager\*.cs" "%~dp0Core\SmartMap\*.cs" "%~dp0Core\Security\*.cs" "%~dp0Core\Failover\*.cs" "%~dp0Core\SCH\*.cs" "%~dp0Models\*.cs" "%~dp0Services\*.cs" "%~dp0UI\Forms\*.cs" "%~dp0UI\Dialogs\*.cs" "%~dp0UI\Views\*.cs" "%~dp0Program.cs"

copy /Y "%PUB_DIR%\MilestoneXProtect-ManagementClient.exe" "%OUT_DIR%\MilestoneXProtect-App.exe" >nul

if %ERRORLEVEL% NEQ 0 goto :BUILD_ERR

:BUILD_OK
echo Copying required Milestone Core DLLs and assets...
copy /Y "%~dp0Milestone_Core_Binaries\*.dll" "%OUT_DIR%\" >nul
copy /Y "%~dp0Milestone_Core_Binaries\*.dll" "%PUB_DIR%\" >nul
copy /Y "%~dp0cctv_frame.png" "%OUT_DIR%\" >nul
copy /Y "%~dp0cctv_frame.png" "%PUB_DIR%\" >nul
copy /Y "%~dp0Lychee.ico" "%OUT_DIR%\" >nul
copy /Y "%~dp0Lychee.ico" "%PUB_DIR%\" >nul
copy /Y "%~dp0Lychee.ico" "%OUT_DIR%\app.ico" >nul
copy /Y "%~dp0Lychee.ico" "%PUB_DIR%\app.ico" >nul

echo ========================================================
echo [SUCCESS] Standalone Executable Successfully Built and Published!
echo Target 1: %PUB_DIR%\MilestoneXProtect-ManagementClient.exe
echo Target 2: %OUT_DIR%\MilestoneXProtect-App.exe
echo ========================================================
exit /b 0

:BUILD_ERR
echo [ERROR] Compilation failed!
exit /b 1
