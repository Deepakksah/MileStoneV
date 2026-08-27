$connString = "Server=(localdb)\MSSQLLocalDB;Database=Surveillance;Integrated Security=True;TrustServerCertificate=True"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
try {
    $conn.Open()
    Write-Host "Connected successfully to MSSQL Surveillance DB!" -ForegroundColor Green
    
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME"
    $reader = $cmd.ExecuteReader()
    $tables = @()
    while ($reader.Read()) {
        $tables += $reader.GetString(0)
    }
    $reader.Close()

    Write-Host "Total Base Tables in Surveillance: $($tables.Count)" -ForegroundColor Cyan
    Write-Host "Key Tables:"
    $tables | Where-Object { $_ -match 'Device|Hardware|Server|Storage|Rule|User|Role|Log' } | ForEach-Object { Write-Host " - $_" -ForegroundColor Yellow }

} catch {
    Write-Error $_.Exception.Message
} finally {
    $conn.Close()
}
