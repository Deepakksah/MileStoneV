using System;

namespace VideoOS.Mobile.LoginThread;

public interface ILoginConfiguration
{
	int HttpMetaChannelPort { get; }

	int HttpSecureMetaChannelPort { get; }

	bool IsSecure { get; }

	string SystemInfoUsername { get; }

	string SystemInfoPassword { get; }

	TimeSpan ManagementServerConnectionTimeout { get; }

	TimeSpan ManagementServerSuccessLoginTimeout { get; }

	TimeSpan ManagementServerFailLoginTimeout { get; }

	Guid ServerIdentificationId { get; }
}
