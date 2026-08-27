using System;
using System.ServiceModel;

namespace VideoOS.SCH.Services.Agents;

[ServiceContract(Namespace = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01", ConfigurationName = "IServiceRegistrationService")]
public interface IServiceRegistration
{
	[OperationContract(Action = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/RegisterServiceWithEndpoints", ReplyAction = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/RegisterServiceWithEndpointsResponse")]
	bool RegisterServiceWithEndpoints(Guid type, Guid instance, string[] uri, string name, string description, string customData, string endpointDefinition);

	[OperationContract(Action = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/UnregisterService", ReplyAction = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/UnregisterServiceResponse")]
	void UnregisterService(Guid instance);

	[OperationContract(Action = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/GetServices", ReplyAction = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/GetServicesResponse")]
	ServiceInfo[] GetServices();
}
