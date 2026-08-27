@echo off
setlocal
echo ========================================================
echo Building Milestone XProtect Management Client (.NET C#)
echo ========================================================

set CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe
set OUT_DIR=%~dp0bin
set OUT_EXE=%OUT_DIR%\MilestoneXProtect-ManagementClient.exe

if not exist "%OUT_DIR%" mkdir "%OUT_DIR%"

echo Compiling all C# source files...
"%CSC%" /t:winexe /optimize+ /platform:x64 "/win32icon:%~dp0app.ico" "/out:%OUT_EXE%" /r:System.dll /r:System.Core.dll /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Xml.dll "/r:%~dp0Milestone_Core_Binaries\VideoOS.Management.Common.dll" "/r:%~dp0Milestone_Core_Binaries\VideoOS.Management.VmoClient.dll" "/r:%~dp0Milestone_Core_Binaries\VideoOS.Platform.dll" "/r:%~dp0Milestone_Core_Binaries\VideoOS.Platform.SDK.dll" "/r:%~dp0Milestone_Core_Binaries\VideoOS.Common.Controls.dll" "%~dp0Models\*.cs" "%~dp0Services\*.cs" "%~dp0UI\Views\*.cs" "%~dp0UI\LoginDialog.cs" "%~dp0UI\ManagementClientForm.cs" "%~dp0Program.cs"

if %ERRORLEVEL% EQU 0 goto :BUILD_OK

echo Main exe locked by running process, compiling to MilestoneXProtect-App.exe...
set OUT_EXE=%OUT_DIR%\MilestoneXProtect-App.exe
"%CSC%" /t:winexe /optimize+ /platform:x64 "/win32icon:%~dp0app.ico" "/out:%OUT_EXE%" /r:System.dll /r:System.Core.dll /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Xml.dll "/r:%~dp0Milestone_Core_Binaries\VideoOS.Management.Common.dll" "/r:%~dp0Milestone_Core_Binaries\VideoOS.Management.VmoClient.dll" "/r:%~dp0Milestone_Core_Binaries\VideoOS.Platform.dll" "/r:%~dp0Milestone_Core_Binaries\VideoOS.Platform.SDK.dll" "/r:%~dp0Milestone_Core_Binaries\VideoOS.Common.Controls.dll" "%~dp0Models\*.cs" "%~dp0Services\*.cs" "%~dp0UI\Views\*.cs" "%~dp0UI\LoginDialog.cs" "%~dp0UI\ManagementClientForm.cs" "%~dp0Program.cs"

if %ERRORLEVEL% NEQ 0 goto :BUILD_ERR

:BUILD_OK
echo Copying required Milestone Core DLLs...
copy /Y "%~dp0Milestone_Core_Binaries\*.dll" "%OUT_DIR%\" >nul
copy /Y "%~dp0cctv_frame.png" "%OUT_DIR%\" >nul
copy /Y "%~dp0app.ico" "%OUT_DIR%\" >nul

echo ========================================================
echo [SUCCESS] Build completed successfully: %OUT_EXE%
echo ========================================================
exit /b 0

:BUILD_ERR
echo [ERROR] Compilation failed!
exit /b 1
