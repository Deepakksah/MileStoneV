using System;
using System.Web.Http.ExceptionHandling;
using Microsoft.Extensions.Logging;

namespace VideoOS.Common.Integration.WebApi;

public class UnhandledExceptionLogger : ExceptionLogger
{
	private ILogger _logger { get; }

	public UnhandledExceptionLogger(ILogger logger)
	{
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		_logger = logger;
	}

	public override void Log(ExceptionLoggerContext context)
	{
		if (context != null && context.Exception != null)
		{
			string text = context.Exception.ToString();
			LoggerExtensions.LogInformation(_logger, "An unhandled exception occurred : {Exception}", new object[1] { text });
		}
	}
}
