using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace VideoOS.IdentityServer.Features.SCIM.Shared;

public sealed class ScimDisableStatusCodeFeatureMiddleware(RequestDelegate next)
{
	public async Task InvokeAsync(HttpContext httpContext)
	{
		if (IsScimRequest(httpContext.Request))
		{
			IStatusCodePagesFeature statusCodePagesFeature = httpContext.Features.Get<IStatusCodePagesFeature>();
			if (statusCodePagesFeature != null && statusCodePagesFeature.Enabled)
			{
				statusCodePagesFeature.Enabled = false;
			}
		}
		await next(httpContext);
	}

	private static bool IsScimRequest(HttpRequest request)
	{
		return request.Path.StartsWithSegments("/scim", StringComparison.OrdinalIgnoreCase);
	}
}
