using System;
using System.Security.Principal;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.Utilities.Formatting;

namespace VideoOS.CertificateRegistration.Common;

public abstract class HttpSysServerEndpointRegistrationBase : ServerEndpointRegistrarBase
{
	private readonly SecurityIdentifier _serviceUser;

	protected IHttpSysSettingsFactory HttpSysSettingsFactory { get; private set; }

	public HttpSysServerEndpointRegistrationBase(IHttpSysSettingsFactory httpSysSettingsFactory, string serviceName, SecurityIdentifier serviceUser)
		: this(httpSysSettingsFactory, serviceName)
	{
		_serviceUser = serviceUser ?? throw new ArgumentNullException("serviceUser");
	}

	public HttpSysServerEndpointRegistrationBase(IHttpSysSettingsFactory httpSysSettingsFactory, string serviceName)
		: base(serviceName)
	{
		HttpSysSettingsFactory = httpSysSettingsFactory ?? throw new ArgumentNullException("httpSysSettingsFactory");
	}

	public HttpSysServerEndpointRegistrationBase(IHttpSysSettingsFactory httpSysSettingsFactory)
	{
		HttpSysSettingsFactory = httpSysSettingsFactory ?? throw new ArgumentNullException("httpSysSettingsFactory");
	}

	protected override Status Execute(string thumbprint)
	{
		bool encryptionEnabled = !string.IsNullOrEmpty(thumbprint);
		IHttpSysSettings httpSysSettings = HttpSysSettingsFactory.Create();
		httpSysSettings.ServiceUserSid = _serviceUser;
		httpSysSettings.EncryptionEnabled = encryptionEnabled;
		httpSysSettings.CertificateHash = thumbprint?.ParseAsBytes();
		httpSysSettings.SaveSettings(force: true);
		return base.Execute(thumbprint);
	}

	protected override Status ExcecuteCleanup()
	{
		HttpSysSettingsFactory.Create().DeleteSettings();
		return base.ExcecuteCleanup();
	}
}
