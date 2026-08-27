using System;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client;

public interface IGatewayProvider
{
	Uri Gateway { get; }
}
