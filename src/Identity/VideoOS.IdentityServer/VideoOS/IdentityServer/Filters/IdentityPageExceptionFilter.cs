using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace VideoOS.IdentityServer.Filters;

public class IdentityPageExceptionFilter : ExceptionFilterAttribute
{
	public override void OnException(ExceptionContext context)
	{
		ILoggerFactory requiredService = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
		ILogger<IdentityPageExceptionFilter> logger = requiredService.CreateLogger<IdentityPageExceptionFilter>();
		logger.LogError(context.Exception, "An unhandled exception occurred on page {Page}. TraceId: {TraceId}", context.ActionDescriptor.DisplayName, Activity.Current?.Id ?? context.HttpContext.TraceIdentifier);
		context.ExceptionHandled = true;
		context.Result = new RedirectToPageResult("/Error");
	}
}
