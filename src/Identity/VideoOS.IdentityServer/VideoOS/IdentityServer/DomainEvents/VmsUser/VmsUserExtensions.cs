using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VideoOS.IdentityServer.DomainEventInfrastructure;
using VideoOS.IdentityServer.Entities;

namespace VideoOS.IdentityServer.DomainEvents.VmsUser;

public static class VmsUserExtensions
{
	public static IServiceCollection AddVmsUserDomainEvents(this IServiceCollection services)
	{
		services.RegisterDomainEvent(EntityState.Deleted, (VideoOS.IdentityServer.Entities.VmsUser user) => new UserDeletedEvent(user.Id));
		return services;
	}
}
