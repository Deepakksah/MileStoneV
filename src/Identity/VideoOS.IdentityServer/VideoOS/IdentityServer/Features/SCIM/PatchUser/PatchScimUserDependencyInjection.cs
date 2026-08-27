using Microsoft.Extensions.DependencyInjection;
using VideoOS.IdentityServer.Endpoints;

namespace VideoOS.IdentityServer.Features.SCIM.PatchUser;

public static class PatchScimUserDependencyInjection
{
	public static IServiceCollection AddPatchScimUser(this IServiceCollection services)
	{
		services.AddTransient<IEndpoint, PatchScimUserEndpoint>();
		return services;
	}
}
