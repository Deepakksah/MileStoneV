@echo off
setlocal
pushd "%~dp0"
echo ========================================================
echo Building Milestone XProtect License Generator (.NET C#)
echo ========================================================

set CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe

if not exist "..\bin" mkdir "..\bin"
if not exist "..\publish" mkdir "..\publish"

"%CSC%" /nologo /t:winexe /optimize+ /platform:x64 "/out:..\bin\Milestone-LicenseGenerator.exe" /r:System.dll /r:System.Core.dll /r:System.Data.dll /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Xml.dll /r:System.Xml.Linq.dll "Core\*.cs" "UI\*.cs" "Program.cs"

if %ERRORLEVEL% EQU 0 (
    copy /Y "..\bin\Milestone-LicenseGenerator.exe" "..\publish\Milestone-LicenseGenerator.exe" >nul
    echo ========================================================
    echo [SUCCESS] Milestone License Generator Built Successfully!
    echo Target 1: ..\bin\Milestone-LicenseGenerator.exe
    echo Target 2: ..\publish\Milestone-LicenseGenerator.exe
    echo ========================================================
    popd
    exit /b 0
) else (
    echo [ERROR] Compilation failed!
    popd
    exit /b 1
)
