using System;
using System.Web.Http.ExceptionHandling;
using Microsoft.Extensions.Logging;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.WebApi;

public sealed class ServerUnhandledExceptionLogger : ExceptionLogger
{
	private ILogger Logger { get; set; }

	public ServerUnhandledExceptionLogger(ILogger logger)
	{
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		Logger = logger;
	}

	public override void Log(ExceptionLoggerContext context)
	{
		if (context != null && context.Exception != null)
		{
			if (context.Exception is NotSupportedByLegacyRecorderException)
			{
				Logger.LogDebug("The operation is not supported for legacy recording servers: {Exception}", context.Exception);
			}
			else
			{
				Logger.LogInformation("An unhandled exception occurred : {Exception}", context.Exception);
			}
		}
	}
}
