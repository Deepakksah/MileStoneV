$connString = "Server=(localdb)\MSSQLLocalDB;Database=Surveillance;Integrated Security=True;TrustServerCertificate=True"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
try {
    $conn.Open()
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT TOP 5 C18NDigest FROM dbo.HardwareDefinition"
    $r = $cmd.ExecuteReader()
    Write-Host "C18NDigests in DB:"
    while ($r.Read()) {
        Write-Host " - '$($r[0])'"
    }
} catch {
    Write-Error $_.Exception.Message
} finally {
    $conn.Close()
}
