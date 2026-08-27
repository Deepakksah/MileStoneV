using Microsoft.Extensions.DependencyInjection;
using VideoOS.IdentityServer.Endpoints;

namespace VideoOS.IdentityServer.Features.SCIM.CreateUser;

public static class ScimFeatureExtensions
{
	public static IServiceCollection AddCreateScimUser(this IServiceCollection services)
	{
		services.AddTransient<IEndpoint, CreateScimUserEndpoint>();
		return services;
	}
}
