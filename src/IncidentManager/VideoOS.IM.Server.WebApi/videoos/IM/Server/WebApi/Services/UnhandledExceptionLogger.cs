using System.Web.Http.ExceptionHandling;
using Microsoft.Extensions.Logging;
using VideoOS.IM.Server.Logger;

namespace VideoOS.IM.Server.WebApi.Services;

public class UnhandledExceptionLogger : ExceptionLogger
{
	public override void Log(ExceptionLoggerContext context)
	{
		LoggerProvider.Instance.Logger.LogError($"Unhandled exception caught: {context.Exception}");
	}
}
