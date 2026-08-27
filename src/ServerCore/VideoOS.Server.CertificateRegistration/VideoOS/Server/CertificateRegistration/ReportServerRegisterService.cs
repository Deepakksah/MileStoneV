using System;
using VideoOS.CertificateRegistration.Common;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Common.Integration;
using VideoOS.Server.Admin.Utilities;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.Server.CertificateRegistration;

public class ReportServerRegisterService : ServerRegisterServiceBase
{
	public ReportServerRegisterService()
		: this(new ServiceRegistrationFactory(), new DnsService(), OemResources.Instance.Server.ServiceName)
	{
	}

	public ReportServerRegisterService(IServiceRegistrationFactory serviceRegistrationServiceFactory, IDnsService dnsService, string serviceName)
		: base(serviceRegistrationServiceFactory, dnsService, serviceName, "Report Server", ServiceTypes.ServiceType_ReportService, handleServiceStartStop: false)
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
