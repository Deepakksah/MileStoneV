using System;
using System.Threading;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Config;

namespace VideoOS.Platform.SDK.Platform.ServerSlaveInfo;

public interface IServerSlaveInfo
{
	ManualResetEvent DoneEvent { get; }

	ISystemInfo MasterInfo { get; }

	Uri ServerUri { get; }

	bool SecureOnly { get; }

	Guid ServerId { get; }

	UserContext UserContext { get; }

	bool LoggedIn { get; set; }

	LoginSettings GetLoginSettings();
}
