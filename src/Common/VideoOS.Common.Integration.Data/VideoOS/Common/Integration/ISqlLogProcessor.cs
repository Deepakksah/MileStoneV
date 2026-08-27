using System;

namespace VideoOS.Common.Integration;

public interface ISqlLogProcessor : IDisposable
{
	void LogSession(Guid sessionId, string userCredentials, string remoteIPAddress);
}
