using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VideoOS.IdentityServer.Entities;

namespace VideoOS.IdentityServer.EventLogInfrastructure;

public abstract class AuditStrategy<TEntity> : IAuditStrategyHandler where TEntity : class
{
	public Type EntityType => typeof(TEntity);

	public async Task<IEnumerable<EventLog>> ProcessEntity(EntityEntry entry)
	{
		return (entry.Entity is TEntity entity) ? (await ProcessEntityStateChange(entry, entity).ConfigureAwait(continueOnCapturedContext: false)) : Array.Empty<EventLog>();
	}

	protected abstract Task<IEnumerable<EventLog>> ProcessEntityStateChange(EntityEntry entry, TEntity entity);
}
