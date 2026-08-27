using Microsoft.Extensions.Logging;

namespace VideoOS.SqlUpdate.Core.Providers;

public interface IDatabaseVersionProviderFactory
{
	IDatabaseVersionProvider Create(ILogger logger, string dbSchema);
}
