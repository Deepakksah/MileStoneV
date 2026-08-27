using System;
using System.ServiceModel;

namespace VideoOS.Platform.Util;

[ServiceContract(Namespace = "http://videoos.net/2/XProtectCSServiceRegistration")]
public interface IServiceRegistration : IDisposable
{
	[OperationContract(Action = "http://videoos.net/2/XProtectCSServiceRegistration/RegisterService")]
	bool RegisterService(Guid type, Guid instance, ArrayOfString uri, string name, string description, string customData);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServiceRegistration/UnregisterService")]
	void UnregisterService(Guid instance);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServiceRegistration/GetServices")]
	ServiceInfo[] GetServices();

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServiceRegistration/SetServiceData")]
	bool SetServiceData(Guid instance, string customData);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServiceRegistration/GetServiceData")]
	string GetServiceData(Guid instance);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServiceRegistration/RegisterServiceWithEndpoints")]
	bool RegisterServiceWithEndpoints(Guid type, Guid instance, ArrayOfString uri, string name, string description, string customData, string endpointDefinitions);
}
