using Microsoft.Extensions.DependencyInjection;
using VideoOS.IdentityServer.Endpoints;

namespace VideoOS.IdentityServer.Features.SCIM.PutUser;

public static class PutScimUserDependencyInjection
{
	public static IServiceCollection AddPutScimUser(this IServiceCollection services)
	{
		services.AddTransient<IEndpoint, PutScimUserEndpoint>();
		return services;
	}
}
