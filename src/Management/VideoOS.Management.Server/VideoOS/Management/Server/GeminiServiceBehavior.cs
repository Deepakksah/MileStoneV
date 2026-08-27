using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace VideoOS.Management.Server;

public class GeminiServiceBehavior : IServiceBehavior
{
	public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
	{
	}

	public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
	{
	}

	public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
	{
		if (serviceDescription == null)
		{
			throw new ArgumentNullException("serviceDescription");
		}
		foreach (ServiceEndpoint endpoint in serviceDescription.Endpoints)
		{
			endpoint.Behaviors.Add(new GeminiEndpointBehavior());
		}
	}
}
