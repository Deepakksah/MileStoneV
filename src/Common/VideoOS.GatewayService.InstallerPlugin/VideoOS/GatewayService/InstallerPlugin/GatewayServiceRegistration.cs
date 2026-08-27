using System;
using System.Globalization;
using System.Net;
using System.ServiceModel;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.GatewayService.InstallerPlugin.GatewayServiceInstallerUtils;
using VideoOS.GatewayService.Utilities;
using VideoOS.Installer.Common;
using VideoOS.Installer.Logging;
using VideoOS.WebService.Common.ServiceRegistrationService;

namespace VideoOS.GatewayService.InstallerPlugin;

public class GatewayServiceRegistration
{
	public delegate void RegistrationHandler(RegistrationParameters registrationParameters);

	private const int MaxServiceRegistrationRetries = 3;

	private static TimeSpan RetryTimeOut = TimeSpan.FromMinutes(2.0);

	private static Guid ServiceTypeGatewayService = new Guid("E46B7BF9-03CE-44EB-BBDC-8BA16D0AAA80");

	private static readonly string GatewayServiceNameToLog = string.Format(CultureInfo.InvariantCulture, "{0}({1})", "API Gateway Service", Dns.GetHostName());

	public IServiceRegistrationServiceProxyFactory RegistrationServiceProxyFactory{ get; set; }

	public ITokenHandlerFactory TokenHandlerFactory{ get; set; }

	public IGatewayTokenCacheFactory TokenCacheFactory{ get; set; }

	public IGatewayServerHelper ServerHelper{ get; set; }

	public IRegistryHelperFactory RegistryHelperFactory{ get; set; }

	public ILogger Log{ get; set; }

	public IUriHelper UriHelper{ get; set; }

	public IDnsHelper DnsHelper{ get; set; }

	public GatewayServiceRegistration()
	{
		RegistrationServiceProxyFactory = new ServiceRegistrationServiceProxyFactory();
		TokenHandlerFactory = new TokenHandlerFactory();
		TokenCacheFactory = new GatewayTokenCacheFactory();
		ServerHelper = new GatewayServerHelper();
		RegistryHelperFactory = new RegistryHelperFactory();
		Log = LogManager.InnerLog;
		UriHelper = new UriHelper(Log);
		DnsHelper = new DnsHelper();
	}

	public GatewayServiceRegistration(TimeSpan retryTimeOut, IServiceRegistrationServiceProxyFactory serviceRegistrationServiceProxyFactory, IGatewayTokenCacheFactory tokenCacheFactory, ITokenHandlerFactory tokenHandlerFactory, IGatewayServerHelper gatewayServerHelper, IRegistryHelperFactory registryHelperFactory, ILogger log, IUriHelper uriHelper, IDnsHelper dnsHelper)
	{
		RetryTimeOut = retryTimeOut;
		RegistrationServiceProxyFactory = serviceRegistrationServiceProxyFactory;
		TokenCacheFactory = tokenCacheFactory;
		TokenHandlerFactory = tokenHandlerFactory;
		ServerHelper = gatewayServerHelper;
		RegistryHelperFactory = registryHelperFactory;
		Log = log;
		UriHelper = uriHelper;
		DnsHelper = dnsHelper;
	}

	public bool TryRegisterService(bool isUpgrade, bool createDatabase, string managementServerAddress, string gatewayServiceWebSiteId)
	{
		LoggerExtensions.LogDebug(Log, "GatewayServiceRegistration.RegisterService - START", Array.Empty<object>());
		try
		{
			bool isSecure = new UriBuilder(managementServerAddress).Scheme == Uri.UriSchemeHttps;
			Uri gatewayServiceUri = UriHelper.GetGatewayServiceUri(gatewayServiceWebSiteId, isSecure, DnsHelper.GetDnsHostName(Environment.MachineName));
			try
			{
				RegistryHelperFactory.CreateRegistryHelper().WriteKey(RegistryKeys.ServiceUrl, gatewayServiceUri.ToString());
				LoggerExtensions.LogDebug(Log, "Gateway Service URI added to registry: " + gatewayServiceUri.ToString(), Array.Empty<object>());
			}
			catch (Exception ex)
			{
				LoggerExtensions.LogInformation(Log, string.Format(CultureInfo.InvariantCulture, "Failed to add Gateway Service URI to registry: {0}. Error: {1}", gatewayServiceUri, ex.Message), Array.Empty<object>());
			}
			if (ShouldTryServiceRegistration(isUpgrade, createDatabase))
			{
				RegistrationHandler handler = RegisterService;
				return TryServiceRegistration(handler, managementServerAddress, gatewayServiceWebSiteId, isSecure);
			}
			LoggerExtensions.LogDebug(Log, "GatewayServiceRegistration.RegisterService - service registration skipped", Array.Empty<object>());
			return true;
		}
		catch (Exception ex2)
		{
			LoggerExtensions.LogError(Log, string.Format(CultureInfo.InvariantCulture, "{0} service registration failed. Error: {1}. Stack trace: {2}.", GatewayServiceNameToLog, ex2.Message, ex2.StackTrace), Array.Empty<object>());
			return false;
		}
	}

	private bool ShouldTryServiceRegistration(bool isUpgrade, bool createDatabase)
	{
		if (!(!isUpgrade || createDatabase))
		{
			return GetServiceInstance() == Guid.Empty;
		}
		return true;
	}

	public bool TryUnregisterService(string managementServerAddress)
	{
		LoggerExtensions.LogDebug(Log, "GatewayServiceRegistration.UnregisterService - START", Array.Empty<object>());
		try
		{
			bool isSecure = new UriBuilder(managementServerAddress).Scheme == Uri.UriSchemeHttps;
			RegistrationHandler handler = UnregisterService;
			return TryServiceRegistration(handler, managementServerAddress, "", isSecure);
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogDebug(Log, string.Format(CultureInfo.InvariantCulture, "{0} service unregistration failed. Error: {1}. Stack trace: {2}.", GatewayServiceNameToLog, ex.Message, ex.StackTrace), Array.Empty<object>());
			return false;
		}
	}

	private bool TryServiceRegistration(RegistrationHandler handler, string managementServerAddress, string gatewayServiceWebSiteId, bool isSecure)
	{
		UriBuilder uriBuilder = new UriBuilder(managementServerAddress);
		LoggerExtensions.LogDebug(Log, "Management Server URI: " + uriBuilder.ToString(), Array.Empty<object>());
		ManagementServerInfo managementServerInfo = GetManagementServerInfo(uriBuilder.Uri);
		if (managementServerInfo == null)
		{
			return false;
		}
		LoggerExtensions.LogDebug(Log, "Identity Server URI: " + managementServerInfo.IDPAddress.ToString(), Array.Empty<object>());
		using IGatewayIdentityServerTokenHandler tokenHandler = TokenHandlerFactory.CreateTokenHandler((NetworkCredential)CredentialCache.DefaultCredentials, managementServerInfo.IDPAddress);
		using IGatewayTokenCache tokenCache = TokenCacheFactory.CreateTokenCache(tokenHandler);
		using IServiceRegistrationServiceProxy serviceRegistrationProxy = RegistrationServiceProxyFactory.CreateNew(uriBuilder.Host, uriBuilder.Port, isSecure, tokenCache);
		RegistrationParameters registrationParameters = new RegistrationParameters
		{
			ManagementServerAddress = uriBuilder.ToString(),
			IsGatewayServiceSecure = isSecure,
			GatewayServiceWebSiteId = gatewayServiceWebSiteId,
			ServiceRegistrationProxy = serviceRegistrationProxy
		};
		handler(registrationParameters);
		return true;
	}

	private ManagementServerInfo GetManagementServerInfo(Uri managementServerUri)
	{
		ManagementServerInfo managementServerInfo = null;
		DateTime now = DateTime.Now;
		while (!ServerHelper.TryGetManagementServerInfo(managementServerUri, out managementServerInfo) && DateTime.Now.Subtract(now) < RetryTimeOut)
		{
			Thread.Sleep(1000);
		}
		return managementServerInfo;
	}

	private void RegisterService(RegistrationParameters registrationParameters)
	{
		Guid orSetServiceInstance = GetOrSetServiceInstance();
		LoggerExtensions.LogDebug(Log, string.Format(CultureInfo.InvariantCulture, "Starting service registration. Service name: {0}. Service instance ID: {1}.", GatewayServiceNameToLog, orSetServiceInstance), Array.Empty<object>());
		Uri gatewayServiceUri = UriHelper.GetGatewayServiceUri(registrationParameters.GatewayServiceWebSiteId, registrationParameters.IsGatewayServiceSecure, DnsHelper.GetDnsHostName(Environment.MachineName));
		LoggerExtensions.LogDebug(Log, "Gateway Service URI: " + gatewayServiceUri.ToString(), Array.Empty<object>());
		bool flag = false;
		int num = 0;
		while (!flag)
		{
			try
			{
				registrationParameters.ServiceRegistrationProxy.RegisterService(ServiceTypeGatewayService, orSetServiceInstance, new string[1] { gatewayServiceUri.ToString() }, UriHelper.GetHostName(), "API Gateway Service", string.Empty);
				flag = true;
			}
			catch (CommunicationException)
			{
				num++;
				if (num == 3)
				{
					throw;
				}
				LoggerExtensions.LogDebug(Log, string.Format("Registration of {0} failed. Retrying in 5 seconds...", "API Gateway Service"), Array.Empty<object>());
				Thread.Sleep(TimeSpan.FromSeconds(5.0));
			}
		}
		LoggerExtensions.LogDebug(Log, string.Format(CultureInfo.InvariantCulture, "{0} service registered successfully", GatewayServiceNameToLog), Array.Empty<object>());
		LoggerExtensions.LogDebug(Log, "GatewayServiceRegistration.RegisterService - END", Array.Empty<object>());
	}

	private void UnregisterService(RegistrationParameters registrationParameters)
	{
		Guid orSetServiceInstance = GetOrSetServiceInstance();
		LoggerExtensions.LogDebug(Log, string.Format(CultureInfo.InvariantCulture, "Starting service unregistration. Service name: {0}. Service instance ID: {1}.", GatewayServiceNameToLog, orSetServiceInstance), Array.Empty<object>());
		registrationParameters.ServiceRegistrationProxy.UnregisterService(orSetServiceInstance);
		LoggerExtensions.LogDebug(Log, string.Format(CultureInfo.InvariantCulture, "Service unregistered from Management Server. Service instance ID: {0}. Management Server URI: {1}", orSetServiceInstance, registrationParameters.ManagementServerAddress), Array.Empty<object>());
		LoggerExtensions.LogDebug(Log, "GatewayServiceRegistration.UnregisterService - END", Array.Empty<object>());
	}

	private Guid GetOrSetServiceInstance()
	{
		IRegistryHelper registryHelper = RegistryHelperFactory.CreateRegistryHelper();
		if (!Guid.TryParse(registryHelper.ReadKey(RegistryKeys.ServiceInstance), out var result))
		{
			result = Guid.NewGuid();
			registryHelper.WriteKey(RegistryKeys.ServiceInstance, result.ToString());
		}
		return result;
	}

	private Guid GetServiceInstance()
	{
		if (!Guid.TryParse(RegistryHelperFactory.CreateRegistryHelper().ReadKey(RegistryKeys.ServiceInstance), out var result))
		{
			return Guid.Empty;
		}
		return result;
	}
}
