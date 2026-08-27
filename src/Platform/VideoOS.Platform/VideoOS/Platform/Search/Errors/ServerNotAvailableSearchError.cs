using System;

namespace VideoOS.Platform.Search.Errors;

public class ServerNotAvailableSearchError : SearchError
{
	public string ServerName { get; private set; }

	public ServerNotAvailableSearchError(string serverName)
	{
		ServerName = serverName;
		base.Message = FormattableString.Invariant($"Server '{serverName}' is not available");
	}
}
