using System;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.GatewayService.InstallerPlugin.GatewayServiceInstallerUtils;

public interface IGatewayTokenCache : ITokenCache, IDisposable
{
}
