$connString = "Server=(localdb)\MSSQLLocalDB;Database=Surveillance;Integrated Security=True;TrustServerCertificate=True"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
try {
    $conn.Open()
    Write-Host "=========================================================" -ForegroundColor Cyan
    Write-Host " FULL DIRECT MSSQL PERSISTENCE SYNC (SURVEILLANCE DB)" -ForegroundColor Cyan
    Write-Host "=========================================================" -ForegroundColor Cyan

    # 1. Primary Recording Server in dbo.Recorders
    $recorderId = "11111111-2222-3333-4444-555555555555"
    $cmdRec = $conn.CreateCommand()
    $cmdRec.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM dbo.Recorders WHERE IDRecorder = '$recorderId')
BEGIN
    INSERT INTO dbo.Recorders (
        IDRecorder, Name, Description, Enabled, MulticastSettings, SynchronizationTime,
        FailoverId, IPScanRangeWarning, DevicePackInfo, FailoverHostName, x64, LastModified
    ) VALUES (
        '$recorderId', 'REC-SRV-01 (Primary)', 'Milestone Recording Server 2025 R3', 1, '<MulticastSettings />',
        60, '00000000-0000-0000-0000-000000000000', 0, 'Milestone Device Pack 13.4a', '', 1, GETUTCDATE()
    );
END
"@
    $cmdRec.ExecuteNonQuery() | Out-Null
    Write-Host " [1/5] Recording Server REC-SRV-01 inserted in dbo.Recorders" -ForegroundColor Green

    # 2. Storage Pool in dbo.RecordingStorage
    $cmdStor = $conn.CreateCommand()
    $cmdStor.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM dbo.RecordingStorage WHERE path = 'D:\Milestone\MediaDatabase')
BEGIN
    INSERT INTO dbo.RecordingStorage (
        pkid, recordingserver_id, name, description, path, is_default, retain_minutes, max_size_mb, created_date, last_modified, bank_id
    ) VALUES (
        NEWID(), '$recorderId', 'Primary Media Storage Pool', 'Default Recording Database', 'D:\Milestone\MediaDatabase',
        1, 43200, 8000000, GETUTCDATE(), GETUTCDATE(), NEWID()
    );
END
"@
    $cmdStor.ExecuteNonQuery() | Out-Null
    Write-Host " [2/5] Storage Pool (30-60-90 Days) inserted in dbo.RecordingStorage" -ForegroundColor Green

    # 3. Hardware Definition (Universal Driver)
    $digest = "UNIVERSAL_ONVIF_DRIVER_2025"
    $cmdHwDef = $conn.CreateCommand()
    $cmdHwDef.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM dbo.HardwareDefinition WHERE C18NDigest = '$digest')
BEGIN
    INSERT INTO dbo.HardwareDefinition (C18NDigest, Definition)
    VALUES ('$digest', '<HardwareDefinition><DriverName>Universal 1-Channel ONVIF</DriverName><DriverVersion>13.4a</DriverVersion></HardwareDefinition>');
END
"@
    $cmdHwDef.ExecuteNonQuery() | Out-Null

    # 4. Hardware & Cameras
    $cameras = @(
        @{ Name="FRONT MAIN GATE CAM 01"; Group="ENTRANCE & PERIMETER"; Res="1920x1080 (1080p FHD)"; Fps=25; RetainDays=30; IP="192.168.1.111" },
        @{ Name="VISITOR LOBBY DOME CAM 02"; Group="ENTRANCE & PERIMETER"; Res="1920x1080 (1080p FHD)"; Fps=25; RetainDays=30; IP="192.168.1.112" },
        @{ Name="SERVER ROOM RACK CAM 01"; Group="DATA CENTER & IT"; Res="3840x2160 (4K UHD)"; Fps=30; RetainDays=90; IP="192.168.1.121" },
        @{ Name="UPS & BATTERY ROOM CAM 02"; Group="DATA CENTER & IT"; Res="1920x1080 (1080p FHD)"; Fps=25; RetainDays=60; IP="192.168.1.122" },
        @{ Name="FACTORY SHOP FLOOR CAM 01"; Group="PRODUCTION & ASSEMBLY"; Res="2560x1440 (2K QHD)"; Fps=25; RetainDays=30; IP="192.168.1.131" },
        @{ Name="FINAL ASSEMBLY LINE PTZ 02"; Group="PRODUCTION & ASSEMBLY"; Res="1920x1080 (1080p FHD)"; Fps=30; RetainDays=30; IP="192.168.1.132" },
        @{ Name="WAREHOUSE BAY A CAM 01"; Group="WAREHOUSE & LOGISTICS"; Res="1920x1080 (1080p FHD)"; Fps=15; RetainDays=60; IP="192.168.1.141" },
        @{ Name="TRUCK LOADING DOCK CAM 02"; Group="WAREHOUSE & LOGISTICS"; Res="2560x1440 (2K QHD)"; Fps=25; RetainDays=60; IP="192.168.1.142" },
        @{ Name="EXECUTIVE BOARDROOM CAM 01"; Group="ADMINISTRATION & OFFICES"; Res="3840x2160 (4K UHD)"; Fps=30; RetainDays=30; IP="192.168.1.151" },
        @{ Name="HR & FINANCE HALLWAY CAM 02"; Group="ADMINISTRATION & OFFICES"; Res="1920x1080 (1080p FHD)"; Fps=20; RetainDays=30; IP="192.168.1.152" },
        @{ Name="VIP CAR PARKING CAM 01"; Group="PARKING & ROADWAYS"; Res="1920x1080 (1080p FHD)"; Fps=25; RetainDays=30; IP="192.168.1.161" },
        @{ Name="ANPR ENTRY BARRIER CAM 02"; Group="PARKING & ROADWAYS"; Res="1920x1080 (1080p FHD)"; Fps=60; RetainDays=90; IP="192.168.1.162" }
    )

    $driverGuid = "00000000-0000-0000-0000-000000000001"

    foreach ($c in $cameras) {
        $hwGuid = [Guid]::NewGuid().ToString()
        $devGuid = [Guid]::NewGuid().ToString()

        # Insert Hardware
        $cmdHw = $conn.CreateCommand()
        $cmdHw.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM dbo.Hardware WHERE Name = @Name + ' HW')
BEGIN
    INSERT INTO dbo.Hardware (
        IDHardware, IDHardwareDriver, Name, Description, URI, LoginID, EncryptedPassword,
        IDRecorder, Enabled, C18NDigest, DriverNumber, DriverProxyName, SubType, LastModified
    ) VALUES (
        '$hwGuid', '$driverGuid', @Name + ' HW', 'ONVIF Conformance Driver Device', 'http://' + @IP + '/onvif/device_service',
        'root', 'encrypted_pass', '$recorderId', 1, '$digest', 1, 'Universal1Channel', 1, GETUTCDATE()
    );
END
"@
        $cmdHw.Parameters.AddWithValue("@Name", $c.Name) | Out-Null
        $cmdHw.Parameters.AddWithValue("@IP", $c.IP) | Out-Null
        $cmdHw.ExecuteNonQuery() | Out-Null

        # Select Hardware ID
        $cmdGetHw = $conn.CreateCommand()
        $cmdGetHw.CommandText = "SELECT TOP 1 IDHardware FROM dbo.Hardware WHERE Name = @Name + ' HW'"
        $cmdGetHw.Parameters.AddWithValue("@Name", $c.Name) | Out-Null
        $actualHwId = $cmdGetHw.ExecuteScalar()

        # Insert Device
        $cmdDev = $conn.CreateCommand()
        $cmdDev.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE Name = @Name)
BEGIN
    INSERT INTO dbo.Devices (
        IDDevice, IDHardware, Name, Description, Location, DeviceType, Enabled, Settings, Streams,
        Shortcut, MulticastEnabled, DeviceIndex, SubType, FailoverLiveOnly, DisableOnFailover,
        PtzManualSessionTimeout, PtzReservedSessionTimeout, PtzExtendedManualSessionTimeout,
        GisPoint, ShortName, Icon, CoverageDirection, CoverageDepth, CoverageFieldOfView,
        EdgeStorageEnabled, MulticastSettings, LastModified
    ) VALUES (
        NEWID(), '$actualHwId', @Name, 'Milestone IP Camera - ' + @Res, @Group, 'Camera', 1, 
        '<Settings><Fps>' + CAST(@Fps AS VARCHAR) + '</Fps><Codec>H.265</Codec><Prebuffer>5</Prebuffer></Settings>',
        '<Streams><Stream name="Main" res="' + @Res + '" fps="' + CAST(@Fps AS VARCHAR) + '" codec="H.265" /></Streams>',
        '', 0, 1, 1, 0, 0,
        30, 30, 60,
        geography::Point(0, 0, 4326), @Name, 0, 0, 0, 0,
        0, '<MulticastSettings><Ip>0.0.0.0</Ip><Port>8860</Port></MulticastSettings>', GETUTCDATE()
    );
END
ELSE
BEGIN
    UPDATE dbo.Devices 
    SET Enabled = 1, Location = @Group, LastModified = GETUTCDATE(),
        Settings = '<Settings><Fps>' + CAST(@Fps AS VARCHAR) + '</Fps><Codec>H.265</Codec><Prebuffer>5</Prebuffer></Settings>',
        Streams = '<Streams><Stream name="Main" res="' + @Res + '" fps="' + CAST(@Fps AS VARCHAR) + '" codec="H.265" /></Streams>'
    WHERE Name = @Name;
END
"@
        $cmdDev.Parameters.AddWithValue("@Name", $c.Name) | Out-Null
        $cmdDev.Parameters.AddWithValue("@Group", $c.Group) | Out-Null
        $cmdDev.Parameters.AddWithValue("@Res", $c.Res) | Out-Null
        $cmdDev.Parameters.AddWithValue("@Fps", $c.Fps) | Out-Null
        $cmdDev.ExecuteNonQuery() | Out-Null
    }
    Write-Host " [3/5] All 12 Hardware & Cameras synchronized to dbo.Hardware & dbo.Devices" -ForegroundColor Green

    # 4. Rules
    $rules = @(
        @{ Name="Rule 01: Motion -> Start Recording"; Desc="Start recording on motion" },
        @{ Name="Rule 02: ANPR Speed Violation Email Alert"; Desc="Send alert on speed violation" },
        @{ Name="Rule 03: Perimeter Line Crossing Alarm"; Desc="Trigger siren on perimeter breach" },
        @{ Name="Rule 04: Server Room Door Access Recording"; Desc="Record 60fps on door access" },
        @{ Name="Rule 05: Daily Midnight Archive Backup"; Desc="Move footage older than retention to archive" }
    )
    foreach ($r in $rules) {
        $cmdRule = $conn.CreateCommand()
        $cmdRule.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM dbo.Rules WHERE Name = @Name)
BEGIN
    INSERT INTO dbo.Rules (IDRule, Name, Description, Definition, Enabled, Priority, LastModified)
    VALUES (NEWID(), @Name, @Desc, '<Definition><Type>Standard</Type></Definition>', 1, 1, GETUTCDATE());
END
"@
        $cmdRule.Parameters.AddWithValue("@Name", $r.Name) | Out-Null
        $cmdRule.Parameters.AddWithValue("@Desc", $r.Desc) | Out-Null
        $cmdRule.ExecuteNonQuery() | Out-Null
    }
    Write-Host " [4/5] Event & Automation Rules synchronized to dbo.Rules" -ForegroundColor Green

    # 5. Security Roles
    $roles = @(
        @{ Name="Administrators"; Desc="Full administrative access" },
        @{ Name="Operators"; Desc="Live viewing and PTZ control" },
        @{ Name="Auditors / View Only"; Desc="Read-only live stream and log access" }
    )
    foreach ($ro in $roles) {
        $cmdRole = $conn.CreateCommand()
        $cmdRole.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM dbo.SecurityRoles WHERE Name = @Name)
BEGIN
    INSERT INTO dbo.SecurityRoles (
        IDRole, Name, Description, Builtin, Enabled, Type, DualRequired, PtzReservedAsAnonymous,
        AllowMobileClientLogOn, AllowSmartClientLogOn, AllowWebClientLogOn, LastModified
    ) VALUES (
        NEWID(), @Name, @Desc, 1, 1, 1, 0, 0, 1, 1, 1, GETUTCDATE()
    );
END
"@
        $cmdRole.Parameters.AddWithValue("@Name", $ro.Name) | Out-Null
        $cmdRole.Parameters.AddWithValue("@Desc", $ro.Desc) | Out-Null
        $cmdRole.ExecuteNonQuery() | Out-Null
    }
    Write-Host " [5/5] Security Roles synchronized to dbo.SecurityRoles" -ForegroundColor Green

    Write-Host "`n=========================================================" -ForegroundColor Green
    Write-Host " ALL MILESTONE DATA IS 100% PERSISTED IN MSSQL DATABASE!" -ForegroundColor Green
    Write-Host "=========================================================" -ForegroundColor Green

} catch {
    Write-Error $_.Exception.Message
} finally {
    $conn.Close()
}
