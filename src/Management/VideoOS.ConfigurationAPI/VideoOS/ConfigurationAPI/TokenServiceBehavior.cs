using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace VideoOS.ConfigurationAPI;

public class TokenServiceBehavior : Attribute, IEndpointBehavior, IServiceBehavior
{
	private TokenHelper _tokenHelper;

	public TokenServiceBehavior(TokenHelper tokenHelper)
	{
		_tokenHelper = tokenHelper;
	}

	void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
	{
		clientRuntime.MessageInspectors.Add(new TokenMessageInspector(_tokenHelper));
	}

	void IEndpointBehavior.ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
	{
		endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new TokenMessageInspector(_tokenHelper));
	}

	void IEndpointBehavior.AddBindingParameters(ServiceEndpoint s, BindingParameterCollection c)
	{
	}

	void IEndpointBehavior.Validate(ServiceEndpoint s)
	{
	}

	void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription desc, ServiceHostBase host)
	{
		foreach (ChannelDispatcher channelDispatcher in host.ChannelDispatchers)
		{
			foreach (EndpointDispatcher endpoint in channelDispatcher.Endpoints)
			{
				endpoint.DispatchRuntime.MessageInspectors.Add(new TokenMessageInspector(_tokenHelper));
			}
		}
	}

	void IServiceBehavior.AddBindingParameters(ServiceDescription s, ServiceHostBase b, Collection<ServiceEndpoint> c, BindingParameterCollection bc)
	{
	}

	void IServiceBehavior.Validate(ServiceDescription s, ServiceHostBase b)
	{
	}
}
