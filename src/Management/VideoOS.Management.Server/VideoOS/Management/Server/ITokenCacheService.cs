using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.Management.Server;

public interface ITokenCacheService
{
	ITokenCache IdpTokenCache { get; }

	ITokenCache ManagementServerTokenCache { get; }

	ITokenCache DataCollectorTokenCache { get; }

	ITokenCache ServerLogsTokenCache { get; }
}
