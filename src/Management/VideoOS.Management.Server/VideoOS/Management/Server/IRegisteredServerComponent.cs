using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public interface IRegisteredServerComponent : IDisposable
{
	RegisteredServer QueryRegisteredServer(Guid serverId);

	RegisteredServer[] QueryRegisteredServersByType(Guid serviceType);

	RegisteredServer[] QueryRegisteredServersWithoutDataCollectorId(string[] ruleServiceTypes);

	void UpdateDataCollectors(Guid serverId, int dataCollectorId);

	RegisteredServer[] QueryAllRegisteredServers(bool includeDisabled);

	void EnableService(Guid serverId, bool enabled);

	void RegisterService(Guid serviceType, Guid serverId, ServerAddress[] addresses, string name, string description, string customData, string endpointDefinition);

	void UnregisterService(Guid serverId);

	void SetServiceData(Guid serverId, string customData);
}
