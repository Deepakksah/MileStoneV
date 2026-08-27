using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace VideoOS.IdentityServer.Middleware;

public class ApiDisableStatusCodeFeatureMiddleware(RequestDelegate next)
{
	public async Task InvokeAsync(HttpContext httpContext)
	{
		if (httpContext.Request.Path.Value != null && httpContext.Request.Path.Value.StartsWith("/api/", StringComparison.OrdinalIgnoreCase))
		{
			IStatusCodePagesFeature statusCodePagesFeature = httpContext.Features.Get<IStatusCodePagesFeature>();
			if (statusCodePagesFeature != null && statusCodePagesFeature.Enabled)
			{
				statusCodePagesFeature.Enabled = false;
			}
		}
		await next(httpContext);
	}
}
