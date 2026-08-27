using System;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IServerCapabilities
{
	string ProtocolVersion { get; }

	TimeSpan WatchDogTimeout { get; }

	bool EulaAccepted { get; }

	string ChangePasswordRelativePath { get; }

	bool WebSocketsAvailable { get; }

	bool SecurityEnabled { get; }

	bool ChapSupported { get; }

	bool JsonCommandsSupported { get; }

	string ServerProductCode { get; }

	Guid ServerId { get; }

	Guid OriginalServerId { get; }

	bool PlainAuthentication { get; }

	bool ExternalUsersSupported { get; }

	string IdpRelativePath { get; }

	int ConnectionTimeoutS { get; }
}
