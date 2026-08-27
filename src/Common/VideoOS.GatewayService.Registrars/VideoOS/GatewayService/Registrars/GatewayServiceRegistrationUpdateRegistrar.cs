using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using VideoOS.CertificateRegistration.Common;
using VideoOS.Common.EnvironmentServices;
using VideoOS.GatewayService.Utilities;
using VideoOS.GatewayService.Utilities.Oem;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.WebService.Common.ServiceRegistrationService;

namespace VideoOS.GatewayService.Registrars;

public class GatewayServiceRegistrationUpdateRegistrar : ServerRegisterServiceBase
{
	private static readonly string RegisteredServiceName = string.Format(CultureInfo.InvariantCulture, "{0}({1})", "API Gateway Service", Dns.GetHostName());

	private static readonly Guid ServiceType = ServiceTypes.ServiceType_GatewayService;

	public IRegistryHelper RegistryHelper{ get; set; }

	public IUriHelper UriHelper{ get; set; }

	public IDnsHelper DnsHelper{ get; set; }

	public IConfigFileHelper ConfigFileHelper{ get; set; }

	public GatewayServiceRegistrationUpdateRegistrar()
		: base(new ServiceRegistrationFactory(), new DnsService(), null, RegisteredServiceName, ServiceType, handleServiceStartStop: false)
	{
		RegistryHelper = new RegistryHelper(new GatewayServiceOemResource());
		UriHelper = new UriHelper(base.Logger);
		DnsHelper = new DnsHelper();
		ConfigFileHelper = new ConfigFileHelper(base.Logger);
	}

	public GatewayServiceRegistrationUpdateRegistrar(IRegistryHelper registryHelper, IUriHelper uriHelper, IServiceRegistrationFactory serviceRegistrationFactory, IDnsHelper dnsHelper, IConfigFileHelper configFileHelper)
		: base(serviceRegistrationFactory, new DnsService(), null, RegisteredServiceName, ServiceType, handleServiceStartStop: false)
	{
		RegistryHelper = registryHelper;
		UriHelper = uriHelper;
		DnsHelper = dnsHelper;
		ConfigFileHelper = configFileHelper;
	}

	protected override Uri UpdateUri(Uri currentUri, bool isSecure)
	{
		string text = (isSecure ? "https" : "http");
		base.Logger.LogInfo("GatewayServiceRegistrationUpdateRegistrar.UpdateUri, getting the address from the base and updating the scheme to: " + text);
		Uri uri = base.UpdateUri(currentUri, isSecure);
		string webSiteId = RegistryHelper.ReadKey(RegistryKeys.WebsiteId);
		return UriHelper.GetGatewayServiceUri(webSiteId, isSecure, uri.DnsSafeHost);
	}

	protected override Status Execute(Uri serverToRegisterTo, Uri unsecureServerToRegisterTo, Uri identityProvider, bool isSecure, NetworkCredential networkCredential)
	{
		base.Logger?.LogInfo(FormattableString.Invariant($"About to register service: \"{RegisteredServiceName}\""));
		string text = RegistryHelper.ReadKey(RegistryKeys.ServiceInstance);
		if (string.IsNullOrEmpty(text))
		{
			base.Logger?.LogError(FormattableString.Invariant($"Registry key not found: {RegistryKeys.ServiceInstance}"));
			return Status.ErrorStatus(StatusCode.UnspecifiedErrorRegistration);
		}
		using IServiceRegistration serviceRegistration = ServiceRegistrationServiceFactory.Create(serverToRegisterTo, unsecureServerToRegisterTo, identityProvider, networkCredential, base.Logger);
		bool flag = true;
		ServiceInfo serviceInfo;
		try
		{
			Guid serviceInstanceGuid = new Guid(text);
			serviceInfo = (from x in serviceRegistration.GetServices()
				where x.type == ServiceType && x.instance == serviceInstanceGuid
				select x).SingleOrDefault();
			if (serviceInfo == null)
			{
				flag = false;
				string webSiteId = RegistryHelper.ReadKey(RegistryKeys.WebsiteId);
				ServiceInfo serviceInfo2 = new ServiceInfo();
				serviceInfo2.instance = serviceInstanceGuid;
				serviceInfo2.uri = new string[1] { UriHelper.GetGatewayServiceUri(webSiteId, isSecure, DnsHelper.GetDnsHostName(Environment.MachineName)).ToString() };
				serviceInfo2.name = UriHelper.GetHostName();
				serviceInfo2.description = "API Gateway Service";
				serviceInfo = serviceInfo2;
				base.Logger?.LogInfo(FormattableString.Invariant($"Service with ServiceInstance: {text} was not found. Registering it for the first time."));
			}
		}
		catch (Exception ex)
		{
			base.Logger?.LogWarning(FormattableString.Invariant($"Registered service update skipped. ServiceInstance: {text}"));
			base.Logger?.LogError(FormattableString.Invariant($"{ex.Message}"));
			return Status.ErrorStatus(StatusCode.UnspecifiedErrorRegistration);
		}
		base.Logger?.LogInfo(FormattableString.Invariant($"Updating service: \"{RegisteredServiceName}\". ServiceInstance: {text}"));
		string text2 = (flag ? serviceRegistration.GetServiceData(serviceInfo.instance) : null);
		ServiceRegistrationParameters registrationParamters = new ServiceRegistrationParameters
		{
			Uris = serviceInfo?.uri,
			Name = serviceInfo?.name,
			Description = serviceInfo?.description,
			ServiceData = (string.IsNullOrWhiteSpace(text2) ? null : text2)
		};
		registrationParamters = UpdateRegistrationParameters(registrationParamters, isSecure);
		serviceRegistration.RegisterService(ServiceType, serviceInfo.instance, registrationParamters.Uris, registrationParamters.Name, registrationParamters.Description, registrationParamters.ServiceData);
		UpdateGatewayServiceUri(registrationParamters.MainUri);
		base.Logger?.LogInfo(FormattableString.Invariant($"Service registration finished. ServiceInstance: {text}"));
		return Status.SuccessStatus();
	}

	private void UpdateGatewayServiceUri(Uri gatewayServiceUri)
	{
		string gatewayConfigPath = Path.Combine(RegistryHelper.ReadKey(RegistryKeys.DestinationFolder), "appsettings.json");
		ConfigFileHelper.UpdateGatewayServiceUri(gatewayConfigPath, gatewayServiceUri);
		RegistryHelper.WriteKey(RegistryKeys.ServiceUrl, gatewayServiceUri.ToString());
	}
}
