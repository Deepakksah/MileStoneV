using System;
using VideoOS.CertificateRegistration.Common;
using VideoOS.Common.EnvironmentServices;
using VideoOS.IM.Server.Registrars.IIS.SSL;
using VideoOS.IM.Server.Registrars.Properties;
using VideoOS.Server.Admin.Utilities;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Registrars;

public sealed class IncidentManagerRegisterEndpointRegistrar : ServerEndpointRegistrarBase
{
	private const string IncidentManagerApplicationName = "IncidentManager";

	[NotNull]
	private readonly ISslHelperWrapper _sslHelper;

	public IncidentManagerRegisterEndpointRegistrar()
		: this(new SslHelperWrapper())
	{
	}

	public IncidentManagerRegisterEndpointRegistrar([NotNull] ISslHelperWrapper sslHelper)
	{
		sslHelper.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Registrars\\IncidentManagerRegisterEndpointRegistrar.cs", 25);
		_sslHelper = sslHelper;
	}

	protected override Status ModifyConfiguration(bool encryptionEnabled, string thumbprint)
	{
		string configuredWebsiteId = IisHelper.GetConfiguredWebsiteId(new RegistryService());
		if (encryptionEnabled)
		{
			try
			{
				_sslHelper.EnableSsl(configuredWebsiteId, "IncidentManager");
				return LoggedStatus(Status.SuccessStatus(), FormattableString.Invariant($"Enabling SSL for IncidentManager done."), "ModifyConfiguration");
			}
			catch (Exception exception)
			{
				return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorEncryption, FormattableString.Invariant($"Enabling SSL for IncidentManager failed."), "ModifyConfiguration");
			}
		}
		try
		{
			_sslHelper.DisableSsl(configuredWebsiteId, "IncidentManager");
			return LoggedStatus(Status.SuccessStatus(), FormattableString.Invariant($"Disabling SSL for IncidentManager - encryption is not enabled."), "ModifyConfiguration");
		}
		catch (Exception exception2)
		{
			return LoggedErrorStatus(exception2, StatusCode.UnspecifiedErrorEncryption, FormattableString.Invariant($"Disabling SSL for IncidentManager failed."), "ModifyConfiguration");
		}
	}
}
