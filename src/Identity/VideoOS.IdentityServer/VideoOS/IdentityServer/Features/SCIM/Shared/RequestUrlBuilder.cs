using System;
using Microsoft.AspNetCore.Http;

namespace VideoOS.IdentityServer.Features.SCIM.Shared;

public class RequestUrlBuilder(IHttpContextAccessor httpContextAccessor) : IRequestUrlBuilder
{
	private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException("httpContextAccessor");

	public string BuildBaseUrl()
	{
		HttpContext httpContext = _httpContextAccessor.HttpContext;
		if (httpContext == null)
		{
			throw new InvalidOperationException("HTTP context is not available");
		}
		HttpRequest request = httpContext.Request;
		return $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}";
	}
}
