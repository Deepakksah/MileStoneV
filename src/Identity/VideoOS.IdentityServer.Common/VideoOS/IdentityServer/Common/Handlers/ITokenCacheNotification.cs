namespace VideoOS.IdentityServer.Common.Handlers;

public interface ITokenCacheNotification
{
	void TokenExpired();
}
