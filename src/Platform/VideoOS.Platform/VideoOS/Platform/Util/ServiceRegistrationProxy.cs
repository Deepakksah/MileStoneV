using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.Platform.Util;

public class ServiceRegistrationProxy : ClientBase<IServiceRegistration>, IServiceRegistration, IDisposable
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

	public bool RegisterService(Guid type, Guid instance, ArrayOfString uri, string name, string description, string customData)
	{
		return base.Channel.RegisterService(type, instance, uri, name, description, customData);
	}

	bool IServiceRegistration.RegisterServiceWithEndpoints(Guid type, Guid instance, ArrayOfString uri, string name, string description, string customData, string endpointDefinition)
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

	public bool SetServiceData(Guid instance, string customData)
	{
		return base.Channel.SetServiceData(instance, customData);
	}

	public string GetServiceData(Guid instance)
	{
		return base.Channel.GetServiceData(instance);
	}
}
