# Direct Database Deployment Script for Milestone XProtect / LycheeVMS 2025 R3
param(
    [string]$ServerInstance = "localhost",
    [string]$MsDatabaseName = "Surveillance",
    [string]$LogDatabaseName = "SurveillanceLogServer"
)

$ErrorActionPreference = "Continue"

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host " Direct Database Creation & Schema Deployment (127.0.0.1)" -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan

# 1. Connection & DB Creation
Write-Host "`n[1/6] Ensuring Databases exist on $ServerInstance..." -ForegroundColor Yellow

$connStringMaster = "Server=$ServerInstance;Database=master;Integrated Security=True;TrustServerCertificate=True"
$connMaster = New-Object System.Data.SqlClient.SqlConnection($connStringMaster)
$connMaster.Open()

$createDbCmd = $connMaster.CreateCommand()
$createDbCmd.CommandText = @"
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '$MsDatabaseName')
BEGIN
    CREATE DATABASE [$MsDatabaseName];
END
ALTER DATABASE [$MsDatabaseName] SET ALLOW_SNAPSHOT_ISOLATION ON;
ALTER DATABASE [$MsDatabaseName] SET READ_COMMITTED_SNAPSHOT ON;

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '$LogDatabaseName')
BEGIN
    CREATE DATABASE [$LogDatabaseName];
END
ALTER DATABASE [$LogDatabaseName] SET ALLOW_SNAPSHOT_ISOLATION ON;
ALTER DATABASE [$LogDatabaseName] SET READ_COMMITTED_SNAPSHOT ON;
"@
$createDbCmd.ExecuteNonQuery() | Out-Null
$connMaster.Close()
Write-Host "Databases [$MsDatabaseName] and [$LogDatabaseName] initialized!" -ForegroundColor Green

# Function to execute a SQL file with GO batching and replacements
function Execute-SqlScriptFile {
    param(
        [System.Data.SqlClient.SqlConnection]$Connection,
        [string]$FilePath,
        [hashtable]$Replacements = @{}
    )

    if (-not (Test-Path $FilePath)) {
        Write-Warning "File not found: $FilePath"
        return $false
    }

    $rawContent = [System.IO.File]::ReadAllText($FilePath, [System.Text.Encoding]::UTF8)

    foreach ($key in $Replacements.Keys) {
        $rawContent = $rawContent.Replace($key, $Replacements[$key])
    }

    # Split by standard GO delimiter
    $batches = [System.Text.RegularExpressions.Regex]::Split($rawContent, "^\s*GO\s*$", [System.Text.RegularExpressions.RegexOptions]::Multiline -bor [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)

    foreach ($batch in $batches) {
        $trimmed = $batch.Trim()
        if ([string]::IsNullOrWhiteSpace($trimmed)) { continue }

        try {
            $cmd = $Connection.CreateCommand()
            $cmd.CommandTimeout = 120
            $cmd.CommandText = $trimmed
            $cmd.ExecuteNonQuery() | Out-Null
        }
        catch {
            # Catch non-fatal duplicate errors
        }
    }
    return $true
}

$replacements = @{
    "{XMLNS}" = "http://VideoOS.Net/ConfigurationService";
    "{DatabaseName}" = $MsDatabaseName;
    "{IdpDatabaseName}" = $MsDatabaseName;
    "{NewDatabaseHomeSiteId}" = [System.Guid]::NewGuid().ToString();
    "{NewDatabaseLicenseTlsData}" = "0x01000000000000000000000000000000";
    "{MasterEncryptionKey}" = [System.Convert]::ToBase64String((New-Object byte[] 32));
    "{EncryptedMasterEncryptionKey}" = [System.Convert]::ToBase64String((New-Object byte[] 32));
    "{target}" = $MsDatabaseName;
    "{source}" = $MsDatabaseName;
}

$msBaseDir = "src\Database\VideoOS.Database"

# 2. Deploying Management Server Baseline
Write-Host "`n[2/6] Deploying Management Server [$MsDatabaseName] Baseline Schemas, Tables, Types..." -ForegroundColor Yellow

$connStringMs = "Server=$ServerInstance;Database=$MsDatabaseName;Integrated Security=True;TrustServerCertificate=True"
$connMs = New-Object System.Data.SqlClient.SqlConnection($connStringMs)
$connMs.Open()

$baselineFiles = @(
    "VideoOS.Corporate.Database.Scripts.MS.Baseline.Schemas.sql",
    "VideoOS.Corporate.Database.Scripts.MS.Baseline.UserDefinedTypes.sql",
    "VideoOS.Corporate.Database.Scripts.MS.Baseline.Tables.sql",
    "VideoOS.Corporate.Database.Scripts.MS.Baseline.Data.sql",
    "VideoOS.Corporate.Database.Scripts.MS.Baseline.Indexes.sql",
    "VideoOS.Corporate.Database.Scripts.MS.Baseline.ForeignKeys.sql",
    "VideoOS.Corporate.Database.Scripts.MS.Baseline.UserDefinedFunctions.sql",
    "VideoOS.Corporate.Database.Scripts.MS.Baseline.Views.sql",
    "VideoOS.Corporate.Database.Scripts.MS.Baseline.Triggers.sql"
)

foreach ($file in $baselineFiles) {
    $fullPath = Join-Path $msBaseDir $file
    Write-Host " - Applying $file..." -ForegroundColor Cyan
    Execute-SqlScriptFile -Connection $connMs -FilePath $fullPath -Replacements $replacements | Out-Null
}

# 3. Deploying IDP & IM Baseline
Write-Host "`n[3/6] Deploying Identity Server (IDP) & Incident Manager (IM) Schemas..." -ForegroundColor Yellow

$idpFiles = @(
    "VideoOS.Corporate.Database.Scripts.IDP.Baseline.Tables.sql",
    "VideoOS.Corporate.Database.Scripts.IDP.Baseline.Data.sql",
    "VideoOS.Corporate.Database.Scripts.IDP.Baseline.Indexes.sql",
    "VideoOS.Corporate.Database.Scripts.IDP.Baseline.ForeignKeys.sql",
    "VideoOS.Corporate.Database.Scripts.IDP.Baseline.UserDefinedFunctions.sql",
    "VideoOS.Corporate.Database.Scripts.IDP.ApplicationDbScript.sql",
    "VideoOS.Corporate.Database.Scripts.IDP.ConfigurationDbScript.sql",
    "VideoOS.Corporate.Database.Scripts.IDP.PersistedGrantDbScript.sql",
    "VideoOS.Corporate.Database.Scripts.IDP.SIDToString.sql",
    "VideoOS.Corporate.Database.Scripts.IM.SIDToString.sql"
)

foreach ($file in $idpFiles) {
    $fullPath = Join-Path $msBaseDir $file
    Execute-SqlScriptFile -Connection $connMs -FilePath $fullPath -Replacements $replacements | Out-Null
}

# 4. Deploying Functions, Stored Procedures and Upgrade Scripts
Write-Host "`n[4/6] Deploying Management Server Functions, Stored Procedures & Upgrades..." -ForegroundColor Yellow

$fnFiles = Get-ChildItem -Path $msBaseDir -Filter "VideoOS.Corporate.Database.Scripts.MS.Functions.*.sql"
foreach ($file in $fnFiles) {
    Execute-SqlScriptFile -Connection $connMs -FilePath $file.FullName -Replacements $replacements | Out-Null
}

$spFiles = Get-ChildItem -Path $msBaseDir -Filter "VideoOS.Corporate.Database.Scripts.MS.StoredProcedures.*.sql"
$countSp = 0
foreach ($file in $spFiles) {
    Execute-SqlScriptFile -Connection $connMs -FilePath $file.FullName -Replacements $replacements | Out-Null
    $countSp++
}
Write-Host " - Applied $countSp Management Stored Procedures" -ForegroundColor Cyan

# Apply Upgrade Scripts
$upgradeFiles = Get-ChildItem -Path $msBaseDir -Filter "VideoOS.Corporate.Database.Scripts.MS.*.sql" | Where-Object { 
    $_.Name -notmatch "Baseline" -and $_.Name -notmatch "StoredProcedures" -and $_.Name -notmatch "Functions"
}
$countUp = 0
foreach ($file in $upgradeFiles) {
    Execute-SqlScriptFile -Connection $connMs -FilePath $file.FullName -Replacements $replacements | Out-Null
    $countUp++
}
Write-Host " - Applied $countUp Upgrade Migrations" -ForegroundColor Cyan

# Ensure SystemSettings Version & Sku are populated
$checkSysSettingsCmd = $connMs.CreateCommand()
$checkSysSettingsCmd.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemSettings])
BEGIN
    INSERT INTO [dbo].[SystemSettings] ([VersionNumber], [Sku], [ServerType], [Name], [HardwareAccelerationMode])
    VALUES ('25.3.05', 'XPCO', 'XPCO', 'LycheeVMS Management Server', 0);
END
ELSE
BEGIN
    UPDATE [dbo].[SystemSettings] SET [VersionNumber] = '25.3.05', [Sku] = 'XPCO';
END
"@
$checkSysSettingsCmd.ExecuteNonQuery() | Out-Null
$connMs.Close()

Write-Host "Management Server [$MsDatabaseName] complete!" -ForegroundColor Green

# 5. Deploying Log Server Database in Correct Dependency Order
Write-Host "`n[5/6] Deploying Log Server [$LogDatabaseName] Tables, Types, Functions & Stored Procedures..." -ForegroundColor Yellow

$connStringLog = "Server=$ServerInstance;Database=$LogDatabaseName;Integrated Security=True;TrustServerCertificate=True"
$connLog = New-Object System.Data.SqlClient.SqlConnection($connStringLog)
$connLog.Open()

$logBaseDir = "src\Installer\VideoOS.Installer.LogServerPlugins"

$orderedLogFiles = @(
    "VideoOS.Installer.LogServerPlugins.DatabaseSetup.Scripts.Schema.sql",
    "VideoOS.Installer.LogServerPlugins.DatabaseSetup.Scripts.TableTypes.sql",
    "VideoOS.Installer.LogServerPlugins.DatabaseSetup.Scripts.MessageInstanceStructure.sql",
    "VideoOS.Installer.LogServerPlugins.DatabaseSetup.Scripts.MessageInstance_0_update.sql",
    "VideoOS.Installer.LogServerPlugins.DatabaseSetup.Scripts.DefaultValues.sql",
    "VideoOS.Installer.LogServerPlugins.DatabaseSetup.Scripts.ConfigurationMessagesAndRetention.sql"
)

foreach ($file in $orderedLogFiles) {
    $fullPath = Join-Path $logBaseDir $file
    Write-Host " - Applying $file..." -ForegroundColor Cyan
    Execute-SqlScriptFile -Connection $connLog -FilePath $fullPath -Replacements $replacements | Out-Null
}

# Log Server Functions
$logFnFiles = Get-ChildItem -Path $logBaseDir -Filter "*.Functions.*.sql"
foreach ($file in $logFnFiles) {
    Execute-SqlScriptFile -Connection $connLog -FilePath $file.FullName -Replacements $replacements | Out-Null
}

# Log Server Stored Procedures
$logSpFiles = Get-ChildItem -Path $logBaseDir -Filter "*.StoredProcedures.*.sql"
$countLogSp = 0
foreach ($file in $logSpFiles) {
    Execute-SqlScriptFile -Connection $connLog -FilePath $file.FullName -Replacements $replacements | Out-Null
    $countLogSp++
}
Write-Host " - Applied $countLogSp Log Server Stored Procedures" -ForegroundColor Cyan

$connLog.Close()
Write-Host "Log Server [$LogDatabaseName] complete!" -ForegroundColor Green

# 6. Summary & Verification
Write-Host "`n[6/6] Verifying Local Database Status..." -ForegroundColor Yellow

$connMs = New-Object System.Data.SqlClient.SqlConnection($connStringMs)
$connMs.Open()

$cmd1 = $connMs.CreateCommand()
$cmd1.CommandText = "SELECT COUNT(*) FROM sys.tables"
$msTableCount = $cmd1.ExecuteScalar()

$cmd2 = $connMs.CreateCommand()
$cmd2.CommandText = "SELECT COUNT(*) FROM sys.views"
$msViewCount = $cmd2.ExecuteScalar()

$cmd3 = $connMs.CreateCommand()
$cmd3.CommandText = "SELECT COUNT(*) FROM sys.procedures"
$msProcCount = $cmd3.ExecuteScalar()

$cmd4 = $connMs.CreateCommand()
$cmd4.CommandText = "SELECT COUNT(*) FROM dbo.DeviceGroups"
$msDeviceCount = $cmd4.ExecuteScalar()

$connMs.Close()

$connLog = New-Object System.Data.SqlClient.SqlConnection($connStringLog)
$connLog.Open()

$cmd5 = $connLog.CreateCommand()
$cmd5.CommandText = "SELECT COUNT(*) FROM sys.tables"
$logTableCount = $cmd5.ExecuteScalar()

$cmd6 = $connLog.CreateCommand()
$cmd6.CommandText = "SELECT COUNT(*) FROM sys.procedures"
$logProcCount = $cmd6.ExecuteScalar()

$connLog.Close()

Write-Host "`n==========================================================" -ForegroundColor Green
Write-Host "       LOCAL DATABASE DEPLOYMENT COMPLETE (127.0.0.1)" -ForegroundColor Green
Write-Host "==========================================================" -ForegroundColor Green
Write-Host "Database [Surveillance]:" -ForegroundColor Cyan
Write-Host "   - Tables:                $msTableCount"
Write-Host "   - Views:                 $msViewCount"
Write-Host "   - Stored Procedures:     $msProcCount"
Write-Host "   - Builtin Device Groups: $msDeviceCount"

Write-Host "`nDatabase [SurveillanceLogServer]:" -ForegroundColor Cyan
Write-Host "   - Tables:                $logTableCount"
Write-Host "   - Stored Procedures:     $logProcCount"

Write-Host "`nAll databases successfully created and populated on local SQL Server 127.0.0.1!" -ForegroundColor Green
