namespace VideoOS.Management.Server.GetConfiguration;

public class ResourceComponent
{
	protected string _connectionString;

	public ResourceComponent(string connectionString)
	{
		_connectionString = connectionString;
	}
}
