using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace VideoOS.GisMapSharedData.CustomHeader;

public class CustomHeaderEndpointBehavior : IEndpointBehavior
{
	private IEnumerable<Func<MessageHeader>> _factoryMethods;

	public CustomHeaderEndpointBehavior(params Func<MessageHeader>[] factoryMethods)
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
			throw new ArgumentNullException("clientRuntime");
		}
		clientRuntime.ClientMessageInspectors.Add(new CustomHeaderMessageInspector(_factoryMethods));
	}
}
