using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VideoOS.IdentityServer.Entities;

namespace VideoOS.IdentityServer.EventLogInfrastructure;

public interface IAuditStrategyHandler
{
	Type EntityType { get; }

	Task<IEnumerable<EventLog>> ProcessEntity(EntityEntry entry);
}
