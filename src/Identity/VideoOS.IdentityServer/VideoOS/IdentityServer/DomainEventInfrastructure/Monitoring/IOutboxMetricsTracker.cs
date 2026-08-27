using System;
using System.Threading.Tasks;

namespace VideoOS.IdentityServer.DomainEventInfrastructure.Monitoring;

public interface IOutboxMetricsTracker
{
	Task<OutboxMessageResult> MonitorBatch(Func<Task<OutboxMessageResult>> work, string instanceId);
}
