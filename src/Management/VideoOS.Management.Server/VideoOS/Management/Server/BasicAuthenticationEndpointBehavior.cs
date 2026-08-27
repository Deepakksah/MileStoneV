using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace VideoOS.Management.Server;

public class BasicAuthenticationEndpointBehavior : IEndpointBehavior
{
	public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
	{
		if (endpoint == null)
		{
			throw new ArgumentNullException("endpoint");
		}
		if (endpointDispatcher == null)
		{
			throw new ArgumentNullException("endpointDispatcher");
		}
		ChannelDispatcher channelDispatcher = endpointDispatcher.ChannelDispatcher;
		if (channelDispatcher != null)
		{
			foreach (EndpointDispatcher endpoint2 in channelDispatcher.Endpoints)
			{
				endpoint2.DispatchRuntime.MessageInspectors.Add(new BasicAuthenticationMessageInspector(new BasicAuthenticationModule()));
			}
		}
		foreach (OperationDescription operation in endpoint.Contract.Operations)
		{
			if (!operation.Behaviors.Contains(typeof(BasicAuthenticationInvokerBehavior)))
			{
				operation.Behaviors.Add(new BasicAuthenticationInvokerBehavior());
			}
		}
	}

	public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
	{
	}

	public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
	{
	}

	public void Validate(ServiceEndpoint endpoint)
	{
	}
}
