using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.WebService.Common.ServiceRegistrationService;

namespace VideoOS.GatewayService.InstallerPlugin.GatewayServiceInstallerUtils;

public class ServiceRegistrationServiceProxyFactory : IServiceRegistrationServiceProxyFactory
{
	public IServiceRegistrationServiceProxy CreateNew(string host, int port, bool isSecure, ITokenCache tokenCache)
	{
		return ServiceRegistrationServiceProxy.CreateNew(host, port, isSecure, tokenCache);
	}
}
