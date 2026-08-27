using System;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client;

public class GatewayProvider : IGatewayProvider
{
	public Uri Gateway { get; }

	public GatewayProvider(Uri gateway)
	{
		Gateway = gateway;
	}
}
