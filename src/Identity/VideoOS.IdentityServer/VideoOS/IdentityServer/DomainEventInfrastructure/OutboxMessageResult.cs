using System;
using System.Collections.Generic;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public class OutboxMessageResult
{
	public IReadOnlyList<OutboxMessage> Messages { get; init; } = Array.Empty<OutboxMessage>();

	public bool IsFullBatch { get; init; }

	public bool HasPendingMessages { get; init; }
}
