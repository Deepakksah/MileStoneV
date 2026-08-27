using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace VideoOS.Management.Server;

public class GeminiEndpointBehavior : IEndpointBehavior
{
	public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
	{
		GeminiTrace.Verbose("Entering IContractBehavior.AddBindingParameters");
	}

	public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
	{
		GeminiTrace.Verbose("Entering IContractBehavior.ApplyClientBehavior");
	}

	public void Validate(ServiceEndpoint endpoint)
	{
		GeminiTrace.Verbose("Entering IContractBehavior.Validate");
	}

	public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
	{
		if (endpointDispatcher == null)
		{
			throw new ArgumentNullException("endpointDispatcher");
		}
		endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new GeminiDispatchMessageInspector());
	}
}
