using System;

namespace VideoOS.Management.Server;

public class GeminiDatabaseSettingsService : IGeminiServerService
{
	public DatabaseSettings ReadSettings(GeminiRequestContext requestContext)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		return new DatabaseSettings
		{
			ConnectionTimeout = 0,
			RequestTimeout = 0,
			DeadlockRetries = 3,
			DeadlockPause = 0,
			LoggingOptions = GeminiDatabaseLoggingOptions.All
		};
	}

	void IGeminiServerService.StartService(IGeminiRequestContext requestContext)
	{
	}

	void IGeminiServerService.StopService(IGeminiRequestContext requestContext)
	{
	}
}
