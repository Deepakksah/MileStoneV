using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.Server.Webservice.Shared.DataCollectorService;

public static class LiveDataCollectionManagerFactory
{
	public static ILiveDataCollectionManager CreateInstance(string hostName, bool secure, ITokenCache tokenCache)
	{
		return new LiveDataCollectionManager(hostName, secure, tokenCache);
	}
}
