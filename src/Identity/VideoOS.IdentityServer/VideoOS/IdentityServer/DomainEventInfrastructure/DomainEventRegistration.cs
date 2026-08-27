using System;
using Microsoft.EntityFrameworkCore;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public class DomainEventRegistration<TEntity, TDomainEvent>(EntityState entityState, Func<TEntity, TDomainEvent> factory) : IDomainEventRegistration where TEntity : class where TDomainEvent : DomainEvent
{
	public void Register(DomainEventsRegistry registry)
	{
		registry.Register(factory, entityState);
	}
}
