using System;

namespace VideoOS.Management.Server;

public interface IHostingServerComponent : IDisposable
{
	int CreateHostingServer(HostingServer hostingServerToCreate, Guid sessionId, string hostingServerType);

	string[] QueryHostingServerIds();

	HostingServer QueryHostingServer(int hostingServerId);

	HostingServer[] QueryHostingServersByIds(int[] hostingServerIds);

	void UpdateHostingServer(HostingServer hostingServer, Guid sessionId);

	void DeleteHostingServer(int hostingServerId, Guid sessionId);
}
