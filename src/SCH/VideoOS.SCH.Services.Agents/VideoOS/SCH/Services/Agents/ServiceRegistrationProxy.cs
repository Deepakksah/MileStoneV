using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.SCH.Services.Agents;

public class ServiceRegistrationProxy : ClientBase<IServiceRegistration>, IServiceRegistration
{
	public ServiceRegistrationProxy()
	{
	}

	public ServiceRegistrationProxy(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public ServiceRegistrationProxy(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public bool RegisterServiceWithEndpoints(Guid type, Guid instance, string[] uri, string name, string description, string customData, string endpointDefinition)
	{
		return base.Channel.RegisterServiceWithEndpoints(type, instance, uri, name, description, customData, endpointDefinition);
	}

	public void UnregisterService(Guid instance)
	{
		base.Channel.UnregisterService(instance);
	}

	public ServiceInfo[] GetServices()
	{
		return base.Channel.GetServices();
	}
}
