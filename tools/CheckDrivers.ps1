$connString = "Server=(localdb)\MSSQLLocalDB;Database=Surveillance;Integrated Security=True;TrustServerCertificate=True"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
try {
    $conn.Open()
    
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT TOP 1 IDHardwareDriver, Name FROM dbo.HardwareDriver"
    $r = $cmd.ExecuteReader()
    if ($r.Read()) {
        Write-Host "HardwareDriver: $($r[0]) - $($r[1])"
    } else {
        Write-Host "HardwareDriver empty"
    }
    $r.Close()

    $cmd.CommandText = "SELECT TOP 1 IDRecorder, Name FROM dbo.RecordingServers"
    $r = $cmd.ExecuteReader()
    if ($r.Read()) {
        Write-Host "RecordingServer: $($r[0]) - $($r[1])"
    } else {
        Write-Host "RecordingServer empty"
    }
    $r.Close()

} catch {
    Write-Error $_.Exception.Message
} finally {
    $conn.Close()
}
