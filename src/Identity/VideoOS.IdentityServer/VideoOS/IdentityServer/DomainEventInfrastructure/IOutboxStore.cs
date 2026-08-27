using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public interface IOutboxStore
{
	Task<OutboxMessageResult> ClaimMessages(string instanceId, CancellationToken cancellationToken);

	Task UpdateMessages(List<MessageUpdate> updates, CancellationToken cancellationToken);

	Task<int> DeleteLogsOlderThanAsync(DateTimeOffset dateTimeOffset);
}
