using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Platform.EventsAndState;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client;

public interface IEventsAndStateClient
{
	Task ConnectionTask { get; }

	Task ConnectAsync(Uri serverAddress, string jwt, CancellationToken cancellationToken);

	Task DisconnectAsync();

	Task<(Guid currentSessionId, int inactiveTimeoutSeconds)> CreateSession(CancellationToken cancellationToken);

	Task<(bool resumedSuccessfully, Guid currentSessionId, int inactiveTimeoutSeconds)> ResumeSession(Guid sessionId, CancellationToken cancellationToken);

	Task<(bool resumedSuccessfully, Guid currentSessionId, int inactiveTimeoutSeconds)> ResumeSession(Guid sessionId, Guid lastReceivedEvent, CancellationToken cancellationToken);

	Task<Guid> AddSubscriptionAsync(IEnumerable<SubscriptionRule> subscriptionRules, CancellationToken cancellationToken);

	Task RemoveSubscriptionAsync(Guid subscriptionId, CancellationToken cancellationToken);

	void AddEventReceiver(IEventReceiver eventReceiver);

	bool RemoveEventReceiver(IEventReceiver eventReceiver);

	Task<IEnumerable<State>> GetCurrentStateAsync(CancellationToken cancellationToken);

	Task SendAllStatefulEventsAsync(CancellationToken cancellationToken);
}
