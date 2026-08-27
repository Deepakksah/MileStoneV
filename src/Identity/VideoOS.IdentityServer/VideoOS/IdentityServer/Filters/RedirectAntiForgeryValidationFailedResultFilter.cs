using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using VideoOS.IdentityServer.Helper;

namespace VideoOS.IdentityServer.Filters;

public class RedirectAntiForgeryValidationFailedResultFilter : IAsyncAlwaysRunResultFilter, IAsyncResultFilter, IFilterMetadata
{
	public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
	{
		if (context.Result is AntiforgeryValidationFailedResult)
		{
			context.HttpContext.Session.DeleteSessionValue("ExpireSession");
			context.HttpContext.Session.SetSessionIntValue("CancelSession", 1);
			context.HttpContext.Response.Headers[HeaderNames.CacheControl] = "no-cache, no-store, max-age=0";
			context.HttpContext.Response.Headers[HeaderNames.Pragma] = "no-cache";
			context.HttpContext.Response.Headers[HeaderNames.Expires] = "-1";
			string value = context.HttpContext.Request.PathBase.Value;
			string value2 = context.HttpContext.Request.QueryString.Value;
			string url = value + "/Identity/Account/Login" + value2;
			context.Result = new RedirectResult(url);
		}
		return next();
	}
}
