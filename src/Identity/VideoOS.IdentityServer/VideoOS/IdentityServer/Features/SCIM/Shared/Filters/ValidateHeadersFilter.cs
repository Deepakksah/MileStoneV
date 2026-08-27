using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VideoOS.IdentityServer.Features.SCIM.Shared.Filters;

public class ValidateHeadersFilter : IEndpointFilter
{
	public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
	{
		HttpContext httpContext = context.HttpContext;
		if (!httpContext.Request.Headers.TryGetValue("Content-Type", out var value) || !value.ToString().StartsWith("application/scim+json", StringComparison.OrdinalIgnoreCase))
		{
			httpContext.Response.StatusCode = 415;
			ScimErrorResponse scimErrorResponse = new ScimErrorResponse();
			scimErrorResponse.Schemas = new string[1] { "urn:ietf:params:scim:api:messages:2.0:Error" };
			scimErrorResponse.Status = 415.ToString();
			scimErrorResponse.Detail = "The Content-Type header must be 'application/scim+json'.";
			return scimErrorResponse;
		}
		return await next(context);
	}
}
