#define TRACE
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using VideoOS.Server.Webservice.Shared.DataCollectorService;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.Management.Server;

public static class LiveDataCollector
{
	public static void PostUpdatedCounterStateRuleConfigurations(IGeminiRequestContext requestContext, string hostName, CounterStateRulesConfigurationUpdate[] updates)
	{
		try
		{
			using ILiveDataCollectionManager liveDataCollectionManager = LiveDataCollectionManagerFactory.CreateInstance(hostName, GetSecure(), requestContext.GetTokenCacheService().DataCollectorTokenCache);
			liveDataCollectionManager.PostUpdatedCounterStateRuleConfigurations(updates);
		}
		catch (Exception ex)
		{
			Trace.WriteLine("PostUpdatedCounterStateRuleConfigurations: Failed: " + ex);
		}
	}

	private static bool GetSecure()
	{
		return new Uri(Config.Instance.GetAppSetting("AuthorizationServerUri", "http://localhost:80/IDP")).Scheme == Uri.UriSchemeHttps;
	}

	public static async Task<bool> IsDataCollectorAlive(IGeminiRequestContext requestContext, string hostName)
	{
		bool result = false;
		try
		{
			using ILiveDataCollectionManager client = LiveDataCollectionManagerFactory.CreateInstance(hostName, GetSecure(), requestContext.GetTokenCacheService().DataCollectorTokenCache);
			return await client.IsAlive();
		}
		catch (Exception ex)
		{
			Trace.WriteLine("IsDataCollectorAlive: Failed: " + ex);
		}
		return result;
	}
}
