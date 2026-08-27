using System;
using VideoOS.Common.Integration;
using VideoOS.WebService.Common.ServiceRegistrationService;

namespace VideoOS.Server;

public interface IServerRegistrationService
{
	bool RegisterService(Guid type, Guid instance, string[] uri, string name, string description, string customData, BasicCallContextInfo basicCallContextInfo);

	bool RegisterServiceWithEndpoints(Guid type, Guid instance, string[] uri, string name, string description, string customData, string endpointDefinition, BasicCallContextInfo basicCallContextInfo);

	void EnableService(Guid instance, bool enabled, BasicCallContextInfo basicCallContextInfo);

	void UnregisterService(Guid instance, BasicCallContextInfo basicCallContextInfo);

	ServiceInfo[] GetServices(string serverHostAddress, BasicCallContextInfo basicCallContextInfo);

	ServiceInfo[] GetServicesIncludingDisabled(string serverHostAddress, BasicCallContextInfo basicCallContextInfo);

	bool SetServiceData(Guid instance, string customData, BasicCallContextInfo basicCallContextInfo);

	string GetServiceData(Guid instance);
}
