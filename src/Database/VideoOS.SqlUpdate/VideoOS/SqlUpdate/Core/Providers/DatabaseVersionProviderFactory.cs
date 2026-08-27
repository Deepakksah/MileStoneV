using Microsoft.Extensions.Logging;

namespace VideoOS.SqlUpdate.Core.Providers;

public class DatabaseVersionProviderFactory : IDatabaseVersionProviderFactory
{
	public IDatabaseVersionProvider Create(ILogger logger, string dbSchema)
	{
		return new DatabaseVersionProvider(logger, dbSchema);
	}
}
