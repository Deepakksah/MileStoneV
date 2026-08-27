using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public class DomainEventsInterceptor(DomainEventsRegistry domainEventsRegistry, IValidator<OutboxMessage> validator, ILogger<DomainEventsInterceptor> logger) : SaveChangesInterceptor
{
	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default(CancellationToken))
	{
		DbContext context = eventData.Context ?? throw new InvalidOperationException("DbContext is unexpectedly null");
		List<(EntityEntry, (Type, Func<object, object>))> list = (from entry in context.ChangeTracker.Entries()
			select ((EntityEntry entry, bool hasRegistration, (Type eventType, Func<object, object> factory) registration))(entry: entry, hasRegistration: domainEventsRegistry.TryGetRegistration(entry.Entity.GetType(), entry.State, out (Type, Func<object, object>) registration), registration: registration) into tuple3
			where tuple3.hasRegistration
			select (entry: tuple3.entry, registration: tuple3.registration)).ToList();
		logger.LogInformation("Processing {Count} entries with domain events", list.Count);
		foreach (var item in list)
		{
			var (entityEntry, _) = item;
			var (type, func) = item.Item2;
			if (func == null)
			{
				throw new InvalidOperationException("Factory method is null for entity type " + entityEntry.Entity.GetType().Name);
			}
			object obj = func(entityEntry.Entity);
			OutboxMessage outboxMessage = new OutboxMessage
			{
				EventType = type.FullName,
				Payload = JsonSerializer.Serialize(obj),
				Status = OutboxMessageStatus.Created,
				CompatibilityLevel = (int)((DomainEvent)obj).CompatibilityLevel
			};
			await validator.ValidateAndThrowAsync(outboxMessage, cancellationToken);
			context.Set<OutboxMessage>().Add(outboxMessage);
			logger.LogDebug("Added OutboxMessage for event type {EventType}", outboxMessage.EventType);
		}
		return result;
	}
}
