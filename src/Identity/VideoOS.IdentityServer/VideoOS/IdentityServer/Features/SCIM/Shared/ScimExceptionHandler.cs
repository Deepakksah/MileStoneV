using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace VideoOS.IdentityServer.Features.SCIM.Shared;

public class ScimExceptionHandler : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		string text = httpContext.Features.Get<IExceptionHandlerPathFeature>()?.Path;
		if (text == null || !text.StartsWith("/scim", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		var value = new
		{
			schemas = new string[1] { "urn:ietf:params:scim:api:messages:2.0:Error" },
			detail = "An unexpected error occurred.",
			status = "500"
		};
		httpContext.Response.StatusCode = 500;
		await httpContext.Response.WriteAsJsonAsync(value, cancellationToken);
		return true;
	}
}
