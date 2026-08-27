using System;
using VideoOS.WebService.Common.Framework;

namespace VideoOS.WebService.Common.ServiceRegistrationService;

public interface IServiceRegistrationServiceProxy : ITimeoutSettings, IAdditionalMessageHeaders, IDisposable
{
	bool RegisterService(Guid type, Guid instance, string[] uri, string name, string description, string customData);

	bool RegisterServiceWithEndpoints(Guid type, Guid instance, string[] uri, string name, string description, string customData, string endpointDefinition);

	void UnregisterService(Guid instance);

	void EnableService(Guid instance, bool enabled);

	ServiceInfo[] GetServices();

	ServiceInfo[] GetServicesIncludingDisabled();

	bool SetServiceData(Guid instance, string customData);

	string GetServiceData(Guid instance);
}
