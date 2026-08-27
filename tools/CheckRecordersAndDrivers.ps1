$connString = "Server=(localdb)\MSSQLLocalDB;Database=Surveillance;Integrated Security=True;TrustServerCertificate=True"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
try {
    $conn.Open()
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT TOP 1 IDRecorder, Name FROM dbo.Recorders"
    $r = $cmd.ExecuteReader()
    if ($r.Read()) {
        Write-Host "Recorder: $($r[0]) - $($r[1])"
    } else {
        Write-Host "Recorders table empty"
    }
    $r.Close()

    $cmd.CommandText = "SELECT TOP 1 IDHardwareDriver, Name FROM dbo.HardwareDefinition"
    $r = $cmd.ExecuteReader()
    if ($r.Read()) {
        Write-Host "HardwareDriver: $($r[0]) - $($r[1])"
    } else {
        Write-Host "HardwareDefinition empty"
    }
    $r.Close()

} catch {
    Write-Error $_.Exception.Message
} finally {
    $conn.Close()
}
