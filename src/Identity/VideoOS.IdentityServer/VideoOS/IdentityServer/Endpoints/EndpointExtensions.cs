using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace VideoOS.IdentityServer.Endpoints;

public static class EndpointExtensions
{
	public static IApplicationBuilder MapEndpoints(this IApplicationBuilder app)
	{
		app.UseEndpoints(delegate(IEndpointRouteBuilder endpoints)
		{
			IEnumerable<IEndpoint> requiredService = app.ApplicationServices.GetRequiredService<IEnumerable<IEndpoint>>();
			foreach (IEndpoint item in requiredService)
			{
				item.MapEndpoint(endpoints);
			}
		});
		return app;
	}
}
