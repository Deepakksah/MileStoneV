using System;
using Microsoft.Extensions.Logging;

namespace VideoOS.IdentityServer;

public static class LogMessages
{
	private static readonly Action<ILogger, string, string, long, Exception> RoutePerformance;

	static LogMessages()
	{
		RoutePerformance = LoggerMessage.Define<string, string, long>(LogLevel.Debug, 0, "Measured {RouteName} {Method} took {ElapsedMilliseconds} ms.");
	}

	public static void LogRoutePerformance(this ILogger logger, string pageName, string method, long elapsedMilliseconds)
	{
		RoutePerformance(logger, pageName, method, elapsedMilliseconds, null);
	}
}
