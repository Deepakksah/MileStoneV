using Microsoft.Extensions.DependencyInjection;

namespace VideoOS.IdentityServer.Services.WellKnownConfigurationRetriever;

public static class StartupExtension
{
	public static IServiceCollection AddWellKnownConfigurationRetriever(this IServiceCollection services)
	{
		services.AddHttpClient();
		services.AddScoped<IHttpClientExtensionsHelper, HttpClientExtensionsHelper>();
		services.AddScoped<IWellKnownConfigurationRetriever, WellKnownConfigurationRetriever>();
		return services;
	}
}
