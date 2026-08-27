using System;
using Microsoft.AspNetCore.Http;

namespace VideoOS.IdentityServer.Features.SCIM.Shared;

public class ScimProviderInfoService(IHttpContextAccessor httpContextAccessor) : IProviderInfoService
{
	public (string ProviderId, string ProviderDisplayName) GetProviderInfo()
	{
		HttpContext httpContext = httpContextAccessor.HttpContext ?? throw new InvalidOperationException("The HTTP request context is not available.");
		string item = httpContext.Items["Scim.ProviderId"]?.ToString() ?? throw new InvalidOperationException("Provider ID is missing from the request context.");
		string item2 = httpContext.Items["Scim.ProviderDisplayName"]?.ToString() ?? throw new InvalidOperationException("Provider DisplayName is missing from the request context.");
		return (ProviderId: item, ProviderDisplayName: item2);
	}
}
