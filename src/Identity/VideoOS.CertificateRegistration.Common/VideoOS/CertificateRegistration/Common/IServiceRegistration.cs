using System;
using VideoOS.WebService.Common.ServiceRegistrationService;

namespace VideoOS.CertificateRegistration.Common;

public interface IServiceRegistration : IDisposable
{
	void EnableService(Guid instance, bool enabled);

	string GetServiceData(Guid instance);

	ServiceInfo[] GetServices();

	ServiceInfo[] GetServicesIncludingDisabled();

	bool RegisterService(Guid type, Guid instance, string[] uri, string name, string description, string customData);

	bool RegisterServiceWithEndpoints(Guid type, Guid instance, string[] uri, string name, string description, string customData, string endpointDefinition);

	bool SetServiceData(Guid instance, string customData);

	void UnregisterService(Guid instance);
}
