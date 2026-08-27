using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace VideoOS.EventServer.Shared.SessionHeader;

public class CustomHeaderEndpointBehavior : IEndpointBehavior
{
	private IEnumerable<MessageHeaderFactoryMethod> _factoryMethods;

	public CustomHeaderEndpointBehavior(params MessageHeaderFactoryMethod[] factoryMethods)
	{
		_factoryMethods = factoryMethods;
	}

	public void Validate(ServiceEndpoint endpoint)
	{
	}

	public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
	{
	}

	public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
	{
	}

	public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
	{
		if (clientRuntime == null)
		{
			throw new ArgumentException("clientRuntime was null");
		}
		clientRuntime.ClientMessageInspectors.Add(new CustomHeaderMessageInspector(_factoryMethods));
	}
}
