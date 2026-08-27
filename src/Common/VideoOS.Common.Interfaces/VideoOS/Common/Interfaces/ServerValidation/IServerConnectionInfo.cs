using System;
using System.Threading;

namespace VideoOS.Common.Interfaces.ServerValidation;

public interface IServerConnectionInfo
{
	event EventHandler<EventArgs> RetrieveLocalUserOptions;

	bool IsSecureConnection(Uri serverAddress);

	bool IsSecureConnection(Uri serverAddress, CancellationToken cancellationToken);
}
