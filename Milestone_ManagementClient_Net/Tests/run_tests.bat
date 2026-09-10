@echo off
setlocal
echo ========================================================
echo Building and Running Milestone Client Automated Tests
echo ========================================================

set CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe

if not exist bin mkdir bin

"%CSC%" /nologo /out:bin\MilestoneTests.exe /target:exe /reference:System.Windows.Forms.dll,System.Drawing.dll,System.Data.dll,System.Xml.dll Models\*.cs Services\*.cs Core\*.cs Tests\*.cs

if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Test suite compilation failed!
    exit /b %ERRORLEVEL%
)

echo.
bin\MilestoneTests.exe
echo.
pause
