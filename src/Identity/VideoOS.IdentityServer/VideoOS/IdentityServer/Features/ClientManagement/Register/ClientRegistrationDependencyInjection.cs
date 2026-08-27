using Microsoft.Extensions.DependencyInjection;
using VideoOS.IdentityServer.Endpoints;

namespace VideoOS.IdentityServer.Features.ClientManagement.Register;

public static class ClientRegistrationDependencyInjection
{
	public static IServiceCollection AddClientRegistration(this IServiceCollection services)
	{
		services.AddSingleton<ClientConfigurationService>();
		services.AddTransient<IEndpoint, ClientRegistrationEndpoint>();
		return services;
	}
}
