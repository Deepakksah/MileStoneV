$w = New-Object -ComObject WScript.Shell
$desktop = [Environment]::GetFolderPath('Desktop')
$lnk = $w.CreateShortcut("$desktop\Milestone XProtect Management Client.lnk")
$lnk.TargetPath = "$PSScriptRoot\bin\MilestoneXProtect-ManagementClient.exe"
$lnk.WorkingDirectory = "$PSScriptRoot\bin"
$lnk.IconLocation = "$PSScriptRoot\app.ico,0"
$lnk.Description = "Milestone XProtect Management Client 2025 R3 (.NET C#)"
$lnk.Save()
Write-Host "Desktop shortcut created successfully at: $desktop\Milestone XProtect Management Client.lnk"
