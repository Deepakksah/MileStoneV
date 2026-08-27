using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.SDK.Portable.OnlineService;

public interface IAddressBookService
{
	bool Enabled { set; }

	string ExternalIp { get; }

	void Initialize(UriBuilder uri, bool isAllowed);

	ServerConfigurationParams UpdateServerCapabilities(ServerConfigurationParams serverConfigurationParams, TimeSpan timeout);
}
