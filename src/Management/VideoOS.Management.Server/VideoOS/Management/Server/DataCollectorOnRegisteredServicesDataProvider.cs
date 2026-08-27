#define TRACE
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using VideoOS.Server.Webservice.Shared.DataCollectorService;

namespace VideoOS.Management.Server;

public class DataCollectorOnRegisteredServicesDataProvider : IDataCollectorOnRegisteredServicesDataProvider
{
	private IGeminiRequestContext RequestContext { get; set; }

	public DataCollectorOnRegisteredServicesDataProvider(IGeminiRequestContext requestContext)
	{
		RequestContext = requestContext;
	}

	private static bool GetSecure()
	{
		return new Uri(Config.Instance.GetAppSetting("AuthorizationServerUri", "http://localhost:80/IDP")).Scheme == Uri.UriSchemeHttps;
	}

	public async Task<int> GetIdFromDataCollector(string hostName)
	{
		try
		{
			using ILiveDataCollectionManager client = LiveDataCollectionManagerFactory.CreateInstance(hostName, GetSecure(), RequestContext.GetTokenCacheService().DataCollectorTokenCache);
			return await client.GetDataCollectorId();
		}
		catch (Exception ex)
		{
			Trace.WriteLine("GetIdFromDataCollector: Failed: " + ex);
		}
		return 0;
	}

	public RegisteredServer[] QueryRegisteredServersWithoutDataCollectorId()
	{
		return RegisteredServer.QueryRegisteredServersWithoutDataCollectorId(RequestContext);
	}

	public void UpdateDataCollectorsForRegisteredServers(Guid serverId, int dataCollectorId)
	{
		RegisteredServer.UpdateDataCollectorsForRegisteredServers(RequestContext, serverId, dataCollectorId);
	}
}
