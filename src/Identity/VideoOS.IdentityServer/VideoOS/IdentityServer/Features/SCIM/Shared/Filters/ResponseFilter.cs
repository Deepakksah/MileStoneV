using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VideoOS.IdentityServer.Features.SCIM.Shared.Filters;

public class ResponseFilter : IEndpointFilter
{
	public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
	{
		object result = await next(context);
		HttpContext httpContext = context.HttpContext;
		if (httpContext.Request.Path.StartsWithSegments("/scim/v2"))
		{
			httpContext.Response.OnStarting(delegate
			{
				if (httpContext.Response.StatusCode != 204)
				{
					httpContext.Response.ContentType = "application/scim+json";
				}
				return Task.CompletedTask;
			});
		}
		return result;
	}
}
