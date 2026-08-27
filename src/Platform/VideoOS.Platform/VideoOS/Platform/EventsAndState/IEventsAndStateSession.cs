using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Platform.EventsAndState;

public interface IEventsAndStateSession : IDisposable
{
	Task SessionTask { get; }

	ConnectionState ConnectionState { get; }

	bool SendStatefulEventsOnReconnect { get; set; }

	Task<Guid> AddSubscriptionAsync(IEnumerable<SubscriptionRule> subscriptionRules, CancellationToken cancellationToken);

	Task<IEnumerable<State>> GetCurrentStateAsync(CancellationToken cancellationToken);

	Task SendAllStatefulEventsAsync(CancellationToken cancellationToken);

	Task RemoveSubscriptionAsync(Guid subscriptionId, CancellationToken cancellationToken);
}
