$connString = "Server=(localdb)\MSSQLLocalDB;Database=Surveillance;Integrated Security=True;TrustServerCertificate=True"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
try {
    $conn.Open()
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Recorders'"
    $r = $cmd.ExecuteReader()
    Write-Host "Columns in dbo.Recorders:"
    while ($r.Read()) {
        Write-Host " - $($r[0]) ($($r[1]))"
    }
} catch {
    Write-Error $_.Exception.Message
} finally {
    $conn.Close()
}
