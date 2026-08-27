namespace VideoOS.Vms.BackupRestore;

public class SqlServerInfo
{
	public string MachineName { get; set; }

	public string SqlAccountName { get; set; }

	public SqlServerInfo(string machineName, string sqlAccountName)
	{
		MachineName = machineName;
		SqlAccountName = sqlAccountName;
	}
}
