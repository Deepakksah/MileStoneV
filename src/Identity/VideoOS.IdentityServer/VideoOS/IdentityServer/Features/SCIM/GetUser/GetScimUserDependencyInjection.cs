using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoOS.IdentityServer.Endpoints;

namespace VideoOS.IdentityServer.Features.SCIM.GetUser;

public static class GetScimUserDependencyInjection
{
	public static IServiceCollection AddGetScimUser(this IServiceCollection services)
	{
		services.AddOptions<GetScimUserSettings>().Configure(delegate(GetScimUserSettings settings, IConfiguration configuration)
		{
			configuration.GetSection("ScimSettings:PaginationSettings").Bind(settings);
		});
		services.AddTransient<IEndpoint, GetScimUserEndpoint>();
		services.AddTransient<IEndpoint, GetScimUserListEndpoint>();
		return services;
	}
}
