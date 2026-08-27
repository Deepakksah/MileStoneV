using System.Threading.Tasks;
using VideoOS.LogServer.Communication.LogConfigurationServiceReference;

namespace VideoOS.LogServer.Communication.LogConfiguration;

public interface ILogConfigurationHandler
{
	Task<LogRetentions> GetLogRetentionsAsync();

	Task<LogRetentions> GetLogRetentionsAsync(string token);

	Task UpdateLogRetentionsAsync(LogRetentions logRetentions);

	Task UpdateLogRetentionsAsync(LogRetentions logRetentions, string token);

	Task<bool> IsLegacyInterfaceLoggingEnabledAsync();

	Task<bool> IsLegacyInterfaceLoggingEnabledAsync(string token);

	Task UpdateLegacyInterfaceLoggingAsync(bool enabled);

	Task UpdateLegacyInterfaceLoggingAsync(bool enabled, string token);
}
