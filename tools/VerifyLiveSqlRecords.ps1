$connString = "Server=(localdb)\MSSQLLocalDB;Database=Surveillance;Integrated Security=True;TrustServerCertificate=True"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
try {
    $conn.Open()
    Write-Host "=== MSSQL Surveillance LIVE RECORD CHECK ===" -ForegroundColor Green
    
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT COUNT(*) FROM dbo.Devices"
    $count = $cmd.ExecuteScalar()
    Write-Host "Total Devices in dbo.Devices: $count" -ForegroundColor Cyan

    $cmd.CommandText = "SELECT TOP 10 Name, Enabled, DeviceType, Settings, LastModified FROM dbo.Devices"
    $reader = $cmd.ExecuteReader()
    while ($reader.Read()) {
        Write-Host " - Device: $($reader['Name']) | Enabled: $($reader['Enabled']) | Type: $($reader['DeviceType']) | Modified: $($reader['LastModified'])"
    }
    $reader.Close()

    Write-Host "`n=== Storage Table Check ===" -ForegroundColor Green
    $cmd.CommandText = "SELECT TOP 5 name, path, retain_minutes, max_size_mb FROM dbo.RecordingStorage"
    $reader = $cmd.ExecuteReader()
    while ($reader.Read()) {
        $days = [math]::Round($reader['retain_minutes'] / 1440)
        Write-Host " - Storage: $($reader['name']) | Path: $($reader['path']) | Retention: $days Days ($($reader['retain_minutes']) mins)"
    }
    $reader.Close()

} catch {
    Write-Error $_.Exception.Message
} finally {
    $conn.Close()
}
