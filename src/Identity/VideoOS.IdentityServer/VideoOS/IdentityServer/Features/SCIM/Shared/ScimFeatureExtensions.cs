using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace VideoOS.IdentityServer.Features.SCIM.Shared;

public static class ScimFeatureExtensions
{
	public static IServiceCollection AddScimFeatureServices(this IServiceCollection services)
	{
		services.AddAuthorization(delegate(AuthorizationOptions options)
		{
			options.AddPolicy("ScimPolicy", delegate(AuthorizationPolicyBuilder policy)
			{
				policy.RequireClaim("scope", "scim");
				policy.AddAuthenticationSchemes("IdentityServerAccessToken");
				policy.RequireAuthenticatedUser();
			});
		});
		services.AddScoped<IRequestUrlBuilder, RequestUrlBuilder>();
		services.AddScoped<IProviderInfoService, ScimProviderInfoService>();
		services.AddExceptionHandler<ScimModelBindingExceptionHandler>();
		services.AddExceptionHandler<ScimExceptionHandler>();
		return services;
	}

	public static IApplicationBuilder UseScimFeature(this IApplicationBuilder app)
	{
		return app.UseMiddleware<ScimDisableStatusCodeFeatureMiddleware>(Array.Empty<object>());
	}
}
