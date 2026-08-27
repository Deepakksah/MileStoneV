using System;
using System.Net;
using System.Security;
using System.Threading;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Config;

namespace VideoOS.Platform.SDK.Platform.ServerSlaveInfo;

public class SlaveServerInfoWithCredentials : SlaveServerInfoBase
{
	public string UserName { get; private set; }

	public SecureString Password { get; private set; }

	public bool IsBasicUser { get; private set; }

	public NetworkCredential ServerCredential { get; private set; }

	public SlaveServerInfoWithCredentials(Uri uri, Guid serverId, bool secureOnly, string userName, SecureString password, bool isBasicUser, ManualResetEvent doneEvent, UserContext userContext, ISystemInfo masterInfo)
	{
		base.ServerUri = uri;
		base.ServerId = serverId;
		base.SecureOnly = secureOnly;
		UserName = userName;
		Password = password;
		IsBasicUser = isBasicUser;
		base.DoneEvent = doneEvent;
		base.UserContext = userContext;
		base.MasterInfo = masterInfo;
	}

	public override LoginSettings GetLoginSettings()
	{
		CredentialCache networkCredentialCache = SDKConfiguration.GetNetworkCredentialCache(base.ServerUri, UserName, Password, IsBasicUser);
		return new LoginSettings(base.ServerUri, networkCredentialCache, base.ServerId, base.ServerUri.DnsSafeHost, base.SecureOnly);
	}
}
