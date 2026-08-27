using Microsoft.Extensions.Logging;

namespace VideoOS.Utilities.Telemetry;

public static class ILoggerExtensions
{
	public static LogLevel HighestEnabledLogLevel(this ILogger logger)
	{
		for (int num = 5; num > 0; num--)
		{
			if (logger.IsEnabled((LogLevel)num))
			{
				return (LogLevel)num;
			}
		}
		return (LogLevel)6;
	}
}
