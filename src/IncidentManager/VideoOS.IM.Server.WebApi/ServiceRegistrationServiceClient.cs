using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using videoos.net._2.XProtectCSServiceRegistration;

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

	public Task<ServiceInfo[]> GetServicesAsync()
	{
		return base.Channel.GetServicesAsync();
	}
}
