using System;
using System.CodeDom.Compiler;
using System.ServiceModel;

namespace VideoOS.Platform.Util.Svc;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01", ConfigurationName = "IServiceRegistrationService")]
public interface IServiceRegistrationService
{
	[OperationContract(Action = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/RegisterService", ReplyAction = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/RegisterServiceResponse")]
	bool RegisterService(Guid type, Guid instance, string[] uri, string name, string description, string customData);

	[OperationContract(Action = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/RegisterServiceWithEndpoints", ReplyAction = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/RegisterServiceWithEndpointsResponse")]
	bool RegisterServiceWithEndpoints(Guid type, Guid instance, string[] uri, string name, string description, string customData, string endpointDefinition);

	[OperationContract(Action = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/UnregisterService", ReplyAction = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/UnregisterServiceResponse")]
	void UnregisterService(Guid instance);

	[OperationContract(Action = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/EnableService", ReplyAction = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/EnableServiceResponse")]
	void EnableService(Guid instance, bool enabled);

	[OperationContract(Action = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/GetServices", ReplyAction = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/GetServicesResponse")]
	ServiceInfo[] GetServices();

	[OperationContract(Action = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/GetServicesIncludingDisabled", ReplyAction = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/GetServicesIncludingDisabledResponse")]
	ServiceInfo[] GetServicesIncludingDisabled();

	[OperationContract(Action = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/SetServiceData", ReplyAction = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/SetServiceDataResponse")]
	bool SetServiceData(Guid instance, string customData);

	[OperationContract(Action = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/GetServiceData", ReplyAction = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/GetServiceDataResponse")]
	string GetServiceData(Guid instance);
}
