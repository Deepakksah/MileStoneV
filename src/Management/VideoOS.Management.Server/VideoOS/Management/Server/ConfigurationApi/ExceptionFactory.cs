using System;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class ExceptionFactory
{
	public static ArgumentNullException CreateArgumentNullException(string parameterName)
	{
		return new ArgumentNullException(parameterName);
	}
}
