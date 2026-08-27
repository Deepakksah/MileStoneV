using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace VideoOS.Installer.LogServerWarningPlugin.SReg;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class ServiceRegistrationServiceClient : ClientBase<IServiceRegistrationService>, IServiceRegistrationService
{
	public ServiceRegistrationServiceClient()
	{
	}

	public ServiceRegistrationServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public ServiceRegistrationServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ServiceRegistrationServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ServiceRegistrationServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public bool RegisterService(Guid type, Guid instance, string[] uri, string name, string description, string customData)
	{
		return base.Channel.RegisterService(type, instance, uri, name, description, customData);
	}

	public Task<bool> RegisterServiceAsync(Guid type, Guid instance, string[] uri, string name, string description, string customData)
	{
		return base.Channel.RegisterServiceAsync(type, instance, uri, name, description, customData);
	}

	public bool RegisterServiceWithEndpoints(Guid type, Guid instance, string[] uri, string name, string description, string customData, string endpointDefinition)
	{
		return base.Channel.RegisterServiceWithEndpoints(type, instance, uri, name, description, customData, endpointDefinition);
	}

	public Task<bool> RegisterServiceWithEndpointsAsync(Guid type, Guid instance, string[] uri, string name, string description, string customData, string endpointDefinition)
	{
		return base.Channel.RegisterServiceWithEndpointsAsync(type, instance, uri, name, description, customData, endpointDefinition);
	}

	public void UnregisterService(Guid instance)
	{
		base.Channel.UnregisterService(instance);
	}

	public Task UnregisterServiceAsync(Guid instance)
	{
		return base.Channel.UnregisterServiceAsync(instance);
	}

	public void EnableService(Guid instance, bool enabled)
	{
		base.Channel.EnableService(instance, enabled);
	}

	public Task EnableServiceAsync(Guid instance, bool enabled)
	{
		return base.Channel.EnableServiceAsync(instance, enabled);
	}

	public ServiceInfo[] GetServices()
	{
		return base.Channel.GetServices();
	}

	public Task<ServiceInfo[]> GetServicesAsync()
	{
		return base.Channel.GetServicesAsync();
	}

	public ServiceInfo[] GetServicesIncludingDisabled()
	{
		return base.Channel.GetServicesIncludingDisabled();
	}

	public Task<ServiceInfo[]> GetServicesIncludingDisabledAsync()
	{
		return base.Channel.GetServicesIncludingDisabledAsync();
	}

	public bool SetServiceData(Guid instance, string customData)
	{
		return base.Channel.SetServiceData(instance, customData);
	}

	public Task<bool> SetServiceDataAsync(Guid instance, string customData)
	{
		return base.Channel.SetServiceDataAsync(instance, customData);
	}

	public string GetServiceData(Guid instance)
	{
		return base.Channel.GetServiceData(instance);
	}

	public Task<string> GetServiceDataAsync(Guid instance)
	{
		return base.Channel.GetServiceDataAsync(instance);
	}
}
