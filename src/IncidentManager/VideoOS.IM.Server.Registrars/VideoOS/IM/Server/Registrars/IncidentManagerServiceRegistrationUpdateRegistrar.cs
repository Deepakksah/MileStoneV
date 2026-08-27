using System;
using VideoOS.CertificateRegistration.Common;
using VideoOS.Common.EnvironmentServices;
using VideoOS.IM.Server.Service.WindowsRegistries;
using VideoOS.Server.Admin.Utilities;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.IM.Server.Registrars;

public sealed class IncidentManagerServiceRegistrationUpdateRegistrar : ServerRegisterServiceBase
{
	private const string IncidentManagerAppPoolName = "VideoOS IM AppPool";

	private static readonly Guid ServiceType = ServiceTypes.ServiceType_IncidentManagerService;

	public IncidentManagerServiceRegistrationUpdateRegistrar()
		: base(new ServiceRegistrationFactory(), new DnsService(), null, "VideoOS IM AppPool", ServiceType, handleServiceStartStop: false)
	{
	}

	public IncidentManagerServiceRegistrationUpdateRegistrar(IWinRegistryService winRegistryService, IDnsService dnsService, IServiceRegistrationFactory serviceRegistrationFactory)
		: base(serviceRegistrationFactory, dnsService, null, "VideoOS IM AppPool", ServiceTypes.ServiceType_IncidentManagerService, handleServiceStartStop: false)
	{
	}

	protected override Uri UpdateUri(Uri currentUri, bool isSecure)
	{
		string configuredWebsiteId = IisHelper.GetConfiguredWebsiteId(new RegistryService());
		IisHelper iisHelper = new IisHelper(new ILoggerWrapper(base.Logger));
		int port = (isSecure ? iisHelper.GetHttpsPort(configuredWebsiteId) : iisHelper.GetHttpPort(configuredWebsiteId));
		return new UriBuilder(base.UpdateUri(currentUri, isSecure))
		{
			Scheme = (isSecure ? Uri.UriSchemeHttps : Uri.UriSchemeHttp),
			Port = port
		}.Uri;
	}
}
