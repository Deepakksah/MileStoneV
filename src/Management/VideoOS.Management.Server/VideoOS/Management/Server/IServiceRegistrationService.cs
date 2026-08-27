using System;
using System.ServiceModel;
using VideoOS.WebService.Common.ServiceRegistrationService;

namespace VideoOS.Management.Server;

[ServiceContract(Namespace = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01")]
public interface IServiceRegistrationService
{
	[OperationContract]
	bool RegisterService(Guid type, Guid instance, string[] uri, string name, string description, string customData);

	[OperationContract]
	bool RegisterServiceWithEndpoints(Guid type, Guid instance, string[] uri, string name, string description, string customData, string endpointDefinition);

	[OperationContract]
	void UnregisterService(Guid instance);

	[OperationContract]
	void EnableService(Guid instance, bool enabled);

	[OperationContract]
	ServiceInfo[] GetServices();

	[OperationContract]
	ServiceInfo[] GetServicesIncludingDisabled();

	[OperationContract]
	bool SetServiceData(Guid instance, string customData);

	[OperationContract]
	string GetServiceData(Guid instance);
}
