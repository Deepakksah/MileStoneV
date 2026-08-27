using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.CertificateRegistration.Common;
using VideoOS.GatewayService.Utilities;
using VideoOS.GatewayService.Utilities.Oem;
using VideoOS.Server.Admin.Utilities;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.GatewayService.Registrars;

public class GatewayServiceEndpointRegistrar : ServerEndpointRegistrarBase
{
	private readonly IGatewayServiceOemResource _oemResources;

	private const string GatewayServiceApplicationName = "API";

	private const string DefaultWebSiteId = "1";

	public IRegistryHelper RegistryHelper{ get; set; }

	public ILogger LogComponent{ get; set; }

	public IIisHelperWrapper IisHelper{ get; set; }

	public ISslHelperWrapper SslHelper{ get; set; }

	public GatewayServiceEndpointRegistrar()
	{
		_oemResources = new GatewayServiceOemResource();
		RegistryHelper = new RegistryHelper(_oemResources);
		LogComponent = (ILogger)(object)new ILoggerWrapper(base.Logger);
		IisHelper = new IisHelperWrapper(LogComponent);
		SslHelper = new SslHelperWrapper();
	}

	public GatewayServiceEndpointRegistrar(IRegistryHelper registryHelper, ILogger logComponent, IIisHelperWrapper iisHelper, ISslHelperWrapper sslHelper)
	{
		RegistryHelper = registryHelper;
		LogComponent = logComponent;
		IisHelper = iisHelper;
		SslHelper = sslHelper;
	}

	protected override Status ModifyConfiguration(bool encryptionEnabled, string thumbprint)
	{
		string websiteId = GetConfiguredWebsiteId();
		if (encryptionEnabled)
		{
			try
			{
				if (!IisHelper.CheckSecureBindings(websiteId) && !string.Equals(websiteId, "1"))
				{
					IisWebsite iisWebsite = IisHelper.GetIisWebsites(LogComponent).Single((IisWebsite item) => item.Id == long.Parse(websiteId));
					return LoggedStatus(Status.ErrorStatus(StatusCode.UnspecifiedErrorEncryption), FormattableString.Invariant($"Setting certificate for API Gateway failed. If you want to secure the communication, configure the HTTPS port on {iisWebsite.Name} website."), "ModifyConfiguration");
				}
				IisHelper.CreateCertificateAndHttpsBindingIfNeeded(websiteId, thumbprint, ignoreExistingCertificate: true);
				SslHelper.EnableSsl(websiteId, "API");
				return LoggedStatus(Status.SuccessStatus(), FormattableString.Invariant($"Setting certificate for API Gateway done."), "ModifyConfiguration");
			}
			catch (Exception exception)
			{
				return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorEncryption, FormattableString.Invariant($"Setting certificate for API Gateway failed."), "ModifyConfiguration");
			}
		}
		try
		{
			SslHelper.DisableSsl(websiteId, "API");
		}
		catch (Exception exception2)
		{
			return LoggedErrorStatus(exception2, StatusCode.UnspecifiedErrorEncryption, FormattableString.Invariant($"Disabling SSL for API Gateway failed."), "ModifyConfiguration");
		}
		return LoggedStatus(Status.SuccessStatus(), FormattableString.Invariant($"Setting certificate for API Gateway skipped - encryption is not enabled."), "ModifyConfiguration");
	}

	private string GetConfiguredWebsiteId()
	{
		return RegistryHelper.ReadKey(RegistryKeys.WebsiteId);
	}
}
