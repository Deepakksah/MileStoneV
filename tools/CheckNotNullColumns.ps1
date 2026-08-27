$connString = "Server=(localdb)\MSSQLLocalDB;Database=Surveillance;Integrated Security=True;TrustServerCertificate=True"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
try {
    $conn.Open()
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT COLUMN_NAME, DATA_TYPE, COLUMN_DEFAULT FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Devices' AND IS_NULLABLE = 'NO'"
    $reader = $cmd.ExecuteReader()
    Write-Host "Non-nullable columns in dbo.Devices:"
    while ($reader.Read()) {
        Write-Host " - $($reader['COLUMN_NAME']) ($($reader['DATA_TYPE'])) Default: $($reader['COLUMN_DEFAULT'])"
    }
} catch {
    Write-Error $_.Exception.Message
} finally {
    $conn.Close()
}
