using System;

namespace VideoOS.Management.VmoClient;

public class SiteAddressWithError
{
	public Uri ServerAddress { get; }

	public Exception ExceptionFound { get; }

	public SiteAddressWithError(Uri serverAddress, Exception exceptionFound)
	{
		ServerAddress = serverAddress;
		ExceptionFound = exceptionFound;
	}
}
