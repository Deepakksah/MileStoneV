using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public static class DomainEventInfrastructureApplicationExtensions
{
	public static void UseDomainEventRegistrations(this IApplicationBuilder app)
	{
		DomainEventsRegistry requiredService = app.ApplicationServices.GetRequiredService<DomainEventsRegistry>();
		IEnumerable<IDomainEventRegistration> services = app.ApplicationServices.GetServices<IDomainEventRegistration>();
		foreach (IDomainEventRegistration item in services)
		{
			item.Register(requiredService);
		}
	}
}
