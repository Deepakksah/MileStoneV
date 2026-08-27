$connString = "Server=(localdb)\MSSQLLocalDB;Database=Surveillance;Integrated Security=True;TrustServerCertificate=True"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
try {
    $conn.Open()
    $tables = @('Devices', 'Hardware', 'RecordingStorage', 'ArchiveStorage', 'Rules', 'SecurityRoles')
    foreach ($tbl in $tables) {
        Write-Host "`n=== TABLE: $tbl ===" -ForegroundColor Cyan
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = "SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '$tbl'"
        $reader = $cmd.ExecuteReader()
        while ($reader.Read()) {
            Write-Host "  - $($reader.GetString(0)) ($($reader.GetString(1)))"
        }
        $reader.Close()
    }
} catch {
    Write-Error $_.Exception.Message
} finally {
    $conn.Close()
}
