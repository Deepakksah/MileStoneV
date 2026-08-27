$connString = "Server=(localdb)\MSSQLLocalDB;Database=Surveillance;Integrated Security=True;TrustServerCertificate=True"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
try {
    $conn.Open()
    Write-Host "Syncing Milestone inventory to MSSQL dbo.Devices..." -ForegroundColor Cyan

    $cameras = @(
        @{ Name="FRONT MAIN GATE CAM 01"; Group="ENTRANCE & PERIMETER"; Res="1920x1080 (1080p FHD)"; Fps=25; RetainDays=30 },
        @{ Name="VISITOR LOBBY DOME CAM 02"; Group="ENTRANCE & PERIMETER"; Res="1920x1080 (1080p FHD)"; Fps=25; RetainDays=30 },
        @{ Name="SERVER ROOM RACK CAM 01"; Group="DATA CENTER & IT"; Res="3840x2160 (4K UHD)"; Fps=30; RetainDays=90 },
        @{ Name="UPS & BATTERY ROOM CAM 02"; Group="DATA CENTER & IT"; Res="1920x1080 (1080p FHD)"; Fps=25; RetainDays=60 },
        @{ Name="FACTORY SHOP FLOOR CAM 01"; Group="PRODUCTION & ASSEMBLY"; Res="2560x1440 (2K QHD)"; Fps=25; RetainDays=30 },
        @{ Name="FINAL ASSEMBLY LINE PTZ 02"; Group="PRODUCTION & ASSEMBLY"; Res="1920x1080 (1080p FHD)"; Fps=30; RetainDays=30 },
        @{ Name="WAREHOUSE BAY A CAM 01"; Group="WAREHOUSE & LOGISTICS"; Res="1920x1080 (1080p FHD)"; Fps=15; RetainDays=60 },
        @{ Name="TRUCK LOADING DOCK CAM 02"; Group="WAREHOUSE & LOGISTICS"; Res="2560x1440 (2K QHD)"; Fps=25; RetainDays=60 },
        @{ Name="EXECUTIVE BOARDROOM CAM 01"; Group="ADMINISTRATION & OFFICES"; Res="3840x2160 (4K UHD)"; Fps=30; RetainDays=30 },
        @{ Name="HR & FINANCE HALLWAY CAM 02"; Group="ADMINISTRATION & OFFICES"; Res="1920x1080 (1080p FHD)"; Fps=20; RetainDays=30 },
        @{ Name="VIP CAR PARKING CAM 01"; Group="PARKING & ROADWAYS"; Res="1920x1080 (1080p FHD)"; Fps=25; RetainDays=30 },
        @{ Name="ANPR ENTRY BARRIER CAM 02"; Group="PARKING & ROADWAYS"; Res="1920x1080 (1080p FHD)"; Fps=60; RetainDays=90 }
    )

    foreach ($c in $cameras) {
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE Name = @Name)
BEGIN
    INSERT INTO dbo.Devices (
        IDDevice, IDHardware, Name, Description, Location, DeviceType, Enabled, Settings, Streams,
        Shortcut, MulticastEnabled, DeviceIndex, SubType, FailoverLiveOnly, DisableOnFailover,
        PtzManualSessionTimeout, PtzReservedSessionTimeout, PtzExtendedManualSessionTimeout,
        GisPoint, ShortName, Icon, CoverageDirection, CoverageDepth, CoverageFieldOfView,
        EdgeStorageEnabled, MulticastSettings, LastModified
    ) VALUES (
        NEWID(), NEWID(), @Name, 'Milestone IP Camera - ' + @Res, @Group, 'Camera', 1, 
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
    UPDATE dbo.Devices SET Enabled = 1, Location = @Group, LastModified = GETUTCDATE() WHERE Name = @Name;
END
"@
        $cmd.Parameters.AddWithValue("@Name", $c.Name) | Out-Null
        $cmd.Parameters.AddWithValue("@Group", $c.Group) | Out-Null
        $cmd.Parameters.AddWithValue("@Res", $c.Res) | Out-Null
        $cmd.Parameters.AddWithValue("@Fps", $c.Fps) | Out-Null
        $cmd.ExecuteNonQuery() | Out-Null
    }

    # Storage Sync
    $cmdStorage = $conn.CreateCommand()
    $cmdStorage.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM dbo.RecordingStorage WHERE path = 'D:\Milestone\MediaDatabase')
BEGIN
    INSERT INTO dbo.RecordingStorage (pkid, recordingserver_id, name, description, path, is_default, retain_minutes, max_size_mb, created_date, last_modified)
    VALUES (NEWID(), NEWID(), 'Primary Media Storage Pool', 'Media database archive', 'D:\Milestone\MediaDatabase', 1, 43200, 8000000, GETUTCDATE(), GETUTCDATE());
END
"@
    $cmdStorage.ExecuteNonQuery() | Out-Null

    Write-Host "MSSQL Inventory Sync SUCCESSFUL!" -ForegroundColor Green

} catch {
    Write-Error $_.Exception.Message
} finally {
    $conn.Close()
}
