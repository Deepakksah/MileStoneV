using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public class DomainEventsRegistry
{
	private readonly Dictionary<(Type entityType, EntityState entityState), (Type eventType, Func<object, object>)> _registrations = new Dictionary<(Type, EntityState), (Type, Func<object, object>)>();

	public void Register<TEntity, TDomainEvent>(Func<TEntity, TDomainEvent> factory, EntityState entityState) where TEntity : class where TDomainEvent : DomainEvent
	{
		if (factory == null)
		{
			throw new ArgumentNullException("factory");
		}
		(Type, EntityState) key = (typeof(TEntity), entityState);
		if (_registrations.ContainsKey(key))
		{
			throw new InvalidOperationException($"A domain event is already registered for entity type '{typeof(TEntity).Name}' and state '{entityState}'");
		}
		_registrations[key] = (typeof(TDomainEvent), delegate(object entity)
		{
			if (entity is TEntity arg)
			{
				return factory(arg);
			}
			throw new InvalidCastException("Entity is not of type " + typeof(TEntity).Name);
		});
	}

	public bool TryGetRegistration(Type entityType, EntityState entityState, out (Type eventType, Func<object, object> factory) registration)
	{
		return _registrations.TryGetValue((entityType, entityState), out registration);
	}
}
