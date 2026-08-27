using System;
using System.Threading;
using VideoOS.Platform.Login;
using VideoOS.Platform.OAuth;
using VideoOS.Platform.SDK.Config;

namespace VideoOS.Platform.SDK.Platform.ServerSlaveInfo;

public class SlaveServerInfoWithTokenCache : SlaveServerInfoBase
{
	public IMipTokenCache TokenCache { get; private set; }

	public SlaveServerInfoWithTokenCache(Uri uri, Guid serverId, bool secureOnly, IMipTokenCache tokenCache, ManualResetEvent doneEvent, UserContext userContext, ISystemInfo masterInfo)
	{
		base.ServerUri = uri;
		base.ServerId = serverId;
		base.SecureOnly = secureOnly;
		TokenCache = tokenCache;
		base.DoneEvent = doneEvent;
		base.UserContext = userContext;
		base.MasterInfo = masterInfo;
	}

	public override LoginSettings GetLoginSettings()
	{
		return new LoginSettings(base.ServerUri, TokenCache, base.ServerId, base.ServerUri.DnsSafeHost, base.SecureOnly);
	}
}
