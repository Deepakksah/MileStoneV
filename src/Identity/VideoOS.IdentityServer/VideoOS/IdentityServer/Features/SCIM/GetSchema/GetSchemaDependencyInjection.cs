using Microsoft.Extensions.DependencyInjection;
using VideoOS.IdentityServer.Endpoints;
using VideoOS.IdentityServer.Features.SCIM.Shared;

namespace VideoOS.IdentityServer.Features.SCIM.GetSchema;

public static class GetSchemaDependencyInjection
{
	public static IServiceCollection AddGetScimSchema(this IServiceCollection services)
	{
		services.AddScoped<GetUserSchemaService>();
		services.AddTransient<IEndpoint, GetSchemaEndpoint>();
		services.AddTransient<IEndpoint, GetSchemaListEndpoint>();
		return services;
	}
}
