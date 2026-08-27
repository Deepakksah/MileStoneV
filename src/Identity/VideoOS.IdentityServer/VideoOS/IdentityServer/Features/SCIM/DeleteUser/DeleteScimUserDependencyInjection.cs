using Microsoft.Extensions.DependencyInjection;
using VideoOS.IdentityServer.Endpoints;

namespace VideoOS.IdentityServer.Features.SCIM.DeleteUser;

public static class DeleteScimUserDependencyInjection
{
	public static IServiceCollection AddDeleteScimUser(this IServiceCollection services)
	{
		services.AddTransient<IEndpoint, DeleteScimUserEndpoint>();
		return services;
	}
}
