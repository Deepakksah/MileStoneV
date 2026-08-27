using VideoOS.Common.EnvironmentServices;

namespace VideoOS.Utilities.Database;

public class EventServerSqlConnectionStorage : BaseSqlConnectionReadonlyStorage
{
	protected const string EventServerRegistryPath = "SOFTWARE\\Milestone\\XProtect Event Server";

	protected const string EventServerRegistryKeyName = "ConnectionString";

	public EventServerSqlConnectionStorage()
		: base("SOFTWARE\\Milestone\\XProtect Event Server", "ConnectionString")
	{
	}

	public EventServerSqlConnectionStorage(IRegistryService registryService)
		: base("SOFTWARE\\Milestone\\XProtect Event Server", "ConnectionString", registryService)
	{
	}
}
