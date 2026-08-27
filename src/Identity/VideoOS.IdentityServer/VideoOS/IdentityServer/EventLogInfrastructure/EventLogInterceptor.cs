using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using VideoOS.IdentityServer.Entities;

namespace VideoOS.IdentityServer.EventLogInfrastructure;

public class EventLogInterceptor(IEnumerable<IAuditStrategyHandler> strategyHandlers, ILogger<EventLogInterceptor> logger) : SaveChangesInterceptor
{
	private readonly Dictionary<Type, IAuditStrategyHandler> _handlerMap = strategyHandlers.ToDictionary((IAuditStrategyHandler h) => h.EntityType);

	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default(CancellationToken))
	{
		DbContext dataContext = eventData.Context ?? throw new InvalidOperationException("DbContext is unexpectedly null");
		List<EventLog> eventLogs = new List<EventLog>();
		foreach (EntityEntry item in dataContext.ChangeTracker.Entries())
		{
			Type entityType = item.Entity.GetType();
			if (!_handlerMap.TryGetValue(entityType, out IAuditStrategyHandler value))
			{
				continue;
			}
			List<EventLog> list = (await value.ProcessEntity(item).ConfigureAwait(continueOnCapturedContext: false)).ToList();
			eventLogs.AddRange(list);
			if (logger.IsEnabled(LogLevel.Debug))
			{
				List<string> values = list.Select((EventLog log) => $"{log.Id}:{log.Event}").ToList();
				logger.LogDebug("Added {Count} EventLogs for entity {entityType}: {Logs}", list.Count, entityType, string.Join(", ", values));
			}
		}
		dataContext.Set<EventLog>().AddRange(eventLogs);
		return result;
	}
}
