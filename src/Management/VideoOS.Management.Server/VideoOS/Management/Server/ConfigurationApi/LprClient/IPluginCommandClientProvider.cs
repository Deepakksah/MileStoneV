using VideoOS.LPR.Common.WCF.Client;

namespace VideoOS.Management.Server.ConfigurationApi.LprClient;

public interface IPluginCommandClientProvider
{
	PluginCommandClientWrapper Client { get; }

	void Close();
}
