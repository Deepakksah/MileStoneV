using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Util;

namespace VideoOS.Platform.SDK.Facades;

public class ServerTypeCacheFacade : IServerTypeCacheFacade
{
	public void RemoveServer(LoginSettings loginSettings)
	{
		ServerTypeCache.RemoveServer(loginSettings);
	}

	public void RemoveAllServers()
	{
		ServerTypeCache.RemoveAllServers();
	}
}
