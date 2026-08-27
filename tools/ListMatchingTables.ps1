$connString = "Server=(localdb)\MSSQLLocalDB;Database=Surveillance;Integrated Security=True;TrustServerCertificate=True"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
try {
    $conn.Open()
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE '%Driver%' OR TABLE_NAME LIKE '%Recorder%' OR TABLE_NAME LIKE '%Hardware%'"
    $r = $cmd.ExecuteReader()
    while ($r.Read()) {
        Write-Host "Table: $($r[0])"
    }
} catch {
    Write-Error $_.Exception.Message
} finally {
    $conn.Close()
}
