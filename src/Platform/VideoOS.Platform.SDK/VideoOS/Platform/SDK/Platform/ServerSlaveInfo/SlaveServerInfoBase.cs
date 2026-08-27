using System;
using System.Threading;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Config;

namespace VideoOS.Platform.SDK.Platform.ServerSlaveInfo;

public abstract class SlaveServerInfoBase : IServerSlaveInfo
{
	public ManualResetEvent DoneEvent { get; protected set; }

	public ISystemInfo MasterInfo { get; protected set; }

	public Uri ServerUri { get; protected set; }

	public bool SecureOnly { get; protected set; }

	public Guid ServerId { get; protected set; }

	public UserContext UserContext { get; protected set; }

	public bool LoggedIn { get; set; }

	public abstract LoginSettings GetLoginSettings();
}
