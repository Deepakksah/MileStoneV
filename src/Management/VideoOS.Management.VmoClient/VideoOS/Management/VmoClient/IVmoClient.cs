using System;

namespace VideoOS.Management.VmoClient;

public interface IVmoClient : IDisposable
{
	ManagementServer ManagementServer { get; }

	IServerConnection ServerConnection { get; }

	VmoRepositories Repositories { get; }
}
