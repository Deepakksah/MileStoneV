$connString = "Server=(localdb)\MSSQLLocalDB;Database=Surveillance;Integrated Security=True;TrustServerCertificate=True"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
try {
    $conn.Open()
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT TOP 5 C18NDigest, DriverNumber, DriverProxyName FROM dbo.HardwareDefinition"
    $r = $cmd.ExecuteReader()
    Write-Host "HardwareDefinitions in DB:"
    while ($r.Read()) {
        Write-Host " - C18NDigest: $($r[0]) | DriverNumber: $($r[1]) | Proxy: $($r[2])"
    }
} catch {
    Write-Error $_.Exception.Message
} finally {
    $conn.Close()
}
