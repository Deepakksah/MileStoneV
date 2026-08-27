using System;
using System.ServiceModel;
using VideoOS.EventServer.Shared.SessionHeader;

namespace VideoOS.EventServer.Communication.Headers;

public static class ClientSessionIdHeaderHelper
{
	public static void AddClientSessionIdHeaderBehavior<T>(ClientBase<T> client) where T : class
	{
		if (client == null)
		{
			throw new ArgumentException("client was null");
		}
		AddClientSessionIdHeaderBehavior(client.ChannelFactory);
	}

	public static void AddClientSessionIdHeaderBehavior(ChannelFactory channelFactory)
	{
		if (channelFactory == null)
		{
			throw new ArgumentException("channelFactory was null");
		}
		channelFactory.Endpoint.EndpointBehaviors.Add(new CustomHeaderEndpointBehavior(ClientSessionManager.CreateClientSessionIdHeader));
	}
}
