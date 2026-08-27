using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Proxy.RestApi;

public interface IServiceRegistrationClient
{
	List<ServiceInformation> GetServices();

	ServiceInformation GetService(Guid instance);

	void UpdateService(Guid instance, bool? enabled, string customData, string[] uris);

	void RegisterService(Guid serviceType, Guid instance, Uri[] uris, string name, string description, string customData);

	void RegisterService(Guid serviceType, Guid instance, Uri[] uris, string name, string description, string customData, List<ServiceInformationEndpoint> endpoints);

	void UnregisterService(Guid instance);

	void Dispose();
}
