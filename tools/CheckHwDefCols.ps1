$connString = "Server=(localdb)\MSSQLLocalDB;Database=Surveillance;Integrated Security=True;TrustServerCertificate=True"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
try {
    $conn.Open()
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HardwareDefinition'"
    $r = $cmd.ExecuteReader()
    Write-Host "HardwareDefinition Columns:"
    while ($r.Read()) {
        Write-Host " - $($r[0]) ($($r[1])) Nullable: $($r[2])"
    }
} catch {
    Write-Error $_.Exception.Message
} finally {
    $conn.Close()
}
