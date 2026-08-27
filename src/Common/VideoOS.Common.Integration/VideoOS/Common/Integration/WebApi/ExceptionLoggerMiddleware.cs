using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Owin;

namespace VideoOS.Common.Integration.WebApi;

public class ExceptionLoggerMiddleware : OwinMiddleware
{
	private readonly ILogger _logger;

	private readonly WebApiStatus _apiStatus;

	public ExceptionLoggerMiddleware(OwinMiddleware next, ILogger logger, WebApiStatus apiStatus)
		: base(next)
	{
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		_logger = logger;
		_apiStatus = apiStatus;
	}

	public override async Task Invoke(IOwinContext context)
	{
		try
		{
			await ((OwinMiddleware)this).Next.Invoke(context);
		}
		catch (ObjectDisposedException ex)
		{
			if (!_apiStatus.IsWebApiStopping)
			{
				LoggerExtensions.LogError(_logger, "{Exception}", new object[1] { ex.ToString() });
			}
			throw;
		}
		catch (Exception ex2)
		{
			LoggerExtensions.LogError(_logger, "{Exception}", new object[1] { ex2.ToString() });
			throw;
		}
	}
}
