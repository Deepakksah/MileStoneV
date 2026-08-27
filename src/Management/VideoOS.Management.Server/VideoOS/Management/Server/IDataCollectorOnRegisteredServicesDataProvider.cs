using System;
using System.Threading.Tasks;

namespace VideoOS.Management.Server;

public interface IDataCollectorOnRegisteredServicesDataProvider
{
	RegisteredServer[] QueryRegisteredServersWithoutDataCollectorId();

	void UpdateDataCollectorsForRegisteredServers(Guid serverId, int dataCollectorId);

	Task<int> GetIdFromDataCollector(string hostName);
}
