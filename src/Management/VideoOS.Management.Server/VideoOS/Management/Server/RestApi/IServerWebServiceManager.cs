using System;

namespace VideoOS.Management.Server.RestApi;

public interface IServerWebServiceManager : IDisposable
{
	int GetVmoServerPort();

	int GetVmoServerSecurePort();
}
