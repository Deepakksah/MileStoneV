using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.Web.Administration;
using VideoOS.CertificateRegistration.Common;
using VideoOS.Common.Crypto;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.GatewayService.Utilities;
using VideoOS.GatewayService.Utilities.Oem;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.GatewayService.Registrars;

public class GatewayServiceClientEndpointRegistrar : ClientEndpointRegistrarBase
{
	private readonly IIisAccountUtility _iisAccountUtility;

	private readonly Lazy<IHttpClient> _httpClient;

	public GatewayServiceClientEndpointRegistrar()
		: this(new GatewayServiceOemResource())
	{
	}

	public GatewayServiceClientEndpointRegistrar(IGatewayServiceOemResource gatewayServiceOemResource)
		: this(new RegistrationParametersHandler("API Gateway Service", new ConfigFileHelper(), Path.Combine(new RegistryHelper(gatewayServiceOemResource).ReadKey(RegistryKeys.DestinationFolder), "appsettings.json"), new List<string> { "managementserver" }), new FileSecretProvider(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), gatewayServiceOemResource.CompanyNameShort, gatewayServiceOemResource.ApiGatewayInstallDir), "idp.tmp", "idp.sec"), new IisAccountUtility(), new HttpClientFactory())
	{
	}

	public GatewayServiceClientEndpointRegistrar(IRegistrationParametersHandler parametersHandler, ISecretProvider secretProvider, IIisAccountUtility iisAccountUtility, IHttpClientFactory httpClientFactory)
		: base(parametersHandler, secretProvider, "API Gateway Service", handleServiceStartStop: false)
	{
		_iisAccountUtility = iisAccountUtility;
		_httpClient = new Lazy<IHttpClient>(() => httpClientFactory.Create());
	}

	protected override string GetAccountUser()
	{
		return _iisAccountUtility.GetAccountUser("VideoOS ApiGateway AppPool");
	}

	public override Status PostExecute()
	{
		Uri[] possibleApiGatewayAddresses = GetPossibleApiGatewayAddresses();
		foreach (Uri requestUri in possibleApiGatewayAddresses)
		{
			try
			{
				if (_httpClient.Value.GetAsync(requestUri, default(CancellationToken)).Result.IsSuccessStatusCode)
				{
					break;
				}
			}
			catch (Exception)
			{
			}
		}
		return base.PostExecute();
	}

	public virtual Uri[] GetPossibleApiGatewayAddresses()
	{
		using ServerManager serverManager = new ServerManager();
		Site site = null;
		site = ((serverManager.Sites.Count != 1) ? serverManager.Sites.FirstOrDefault((Site s) => s.Applications.Any((Application x) => x.ApplicationPoolName == "VideoOS ApiGateway AppPool")) : serverManager.Sites[0]);
		int? num = site?.Bindings.FirstOrDefault((Binding x) => x.Protocol == Uri.UriSchemeHttp)?.EndPoint.Port;
		int? num2 = site?.Bindings.FirstOrDefault((Binding x) => x.Protocol == Uri.UriSchemeHttps)?.EndPoint.Port;
		if (site == null)
		{
			base.Logger.LogError("Unable to get the site in IIS that hosts the app pool: VideoOS ApiGateway AppPool. Reverting to default site");
		}
		else
		{
			if (!num.HasValue)
			{
				base.Logger.LogInfo("Unable to get the http binding on site " + site.Name + ". Reverting to default port");
			}
			if (!num2.HasValue)
			{
				base.Logger.LogInfo("Unable to get the http binding on site " + site.Name + ". Reverting to default port");
			}
		}
		string hostName = Dns.GetHostEntry("localhost").HostName;
		string pathValue = "/api/.well-known/uris";
		return new Uri[2]
		{
			new UriBuilder(Uri.UriSchemeHttp, hostName, num ?? 80, pathValue).Uri,
			new UriBuilder(Uri.UriSchemeHttps, hostName, num2 ?? 443, pathValue).Uri
		};
	}
}
