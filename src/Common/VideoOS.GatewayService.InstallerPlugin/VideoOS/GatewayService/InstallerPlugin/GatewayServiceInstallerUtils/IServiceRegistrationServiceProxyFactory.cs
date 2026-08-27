using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.WebService.Common.ServiceRegistrationService;

namespace VideoOS.GatewayService.InstallerPlugin.GatewayServiceInstallerUtils;

public interface IServiceRegistrationServiceProxyFactory
{
	IServiceRegistrationServiceProxy CreateNew(string host, int port, bool isSecure, ITokenCache tokenCache);
}
