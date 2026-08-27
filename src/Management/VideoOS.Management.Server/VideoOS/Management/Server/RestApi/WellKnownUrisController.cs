using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using VideoOS.Common.Integration;
using VideoOS.Common.Services;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server.RestApi;

[Route(".well-known/uris")]
public class WellKnownUrisController : ApiController
{
	private readonly IRestApiRequestContextFactory _requestContextFactory;

	private readonly IConfig _config;

	private readonly IServerWebServiceManagerFactory _serverWebServiceManagerFactory;

	public WellKnownUrisController()
		: this(new RestApiRequestContextFactory(), Config.Instance, new ServerWebServiceManagerFactory())
	{
	}

	public WellKnownUrisController(IRestApiRequestContextFactory requestContextFactory, IConfig config, IServerWebServiceManagerFactory serverWebServiceManagerFactory)
	{
		_requestContextFactory = requestContextFactory;
		_config = config;
		_serverWebServiceManagerFactory = serverWebServiceManagerFactory;
	}

	[HttpGet]
	[AllowAnonymous]
	public IHttpActionResult Get()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		Version version = AssemblyProductInfo.Version;
		AppendObjectIfDefined("ProductVersion", version, dictionary);
		Site site;
		using (IRestApiRequestContext restApiRequestContext = _requestContextFactory.GetRequestContext(base.Request))
		{
			using ISiteComponent siteComponent = restApiRequestContext.CreateSiteComponent();
			site = siteComponent.QueryHomeSite();
		}
		string host = new Uri(site.Addresses.FirstOrDefault((ServerAddress x) => x.Type == ServerAddressType.Server).URL).Host;
		int vmoServerPort;
		int vmoServerSecurePort;
		using (IServerWebServiceManager serverWebServiceManager = _serverWebServiceManagerFactory.CreateServerWebServiceManager())
		{
			vmoServerPort = serverWebServiceManager.GetVmoServerPort();
			vmoServerSecurePort = serverWebServiceManager.GetVmoServerSecurePort();
		}
		if (vmoServerPort == 0 || vmoServerSecurePort == 0)
		{
			return InternalServerError();
		}
		Uri uri = new UriBuilder(Uri.UriSchemeHttp, host, vmoServerPort).Uri;
		Uri uri2 = new UriBuilder(Uri.UriSchemeHttps, host, vmoServerSecurePort).Uri;
		AppendObjectIfDefined("UnsecureManagementServer", uri, dictionary);
		AppendObjectIfDefined("SecureManagementServer", uri2, dictionary);
		string appSetting = _config.GetAppSetting("AuthorizationServerUri", null);
		Uri obj = ((appSetting != null) ? new Uri(appSetting) : null);
		AppendObjectIfDefined("IdentityProvider", obj, dictionary);
		RegisteredServer[] array;
		using (IRestApiRequestContext restApiRequestContext2 = _requestContextFactory.GetRequestContext(base.Request))
		{
			using IRegisteredServerComponent registeredServerComponent = restApiRequestContext2.CreateRegisteredServerComponent();
			array = registeredServerComponent.QueryRegisteredServersByType(ServiceTypes.ServiceType_GatewayService);
		}
		if (array != null && array.Any())
		{
			List<string> list = new List<string>();
			RegisteredServer[] array2 = array;
			foreach (RegisteredServer registeredServer in array2)
			{
				list.AddRange(registeredServer.Addresses.Select((ServerAddress address) => address.URL.ToString()));
			}
			dictionary.Add("ApiGateways", list);
		}
		return Ok(dictionary);
	}

	private static void AppendObjectIfDefined(string key, object obj, Dictionary<string, object> result)
	{
		if (obj != null)
		{
			result.Add(key, obj.ToString());
		}
	}
}
