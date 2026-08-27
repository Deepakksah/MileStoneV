using System;
using VideoOS.Platform.SDK.Util;

namespace VideoOS.Platform.SDK.Facades;

public class ServerOfflineCacheFacade : IServerOfflineCacheFacade
{
	public void Clear()
	{
		ServerOfflineCache.Clear();
	}

	public void Remove(string serverAddress)
	{
		ServerOfflineCache.RemoveServer(new Uri(serverAddress));
	}
}
