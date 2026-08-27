using System;
using VideoOS.IM.Server.Registrars.Properties;
using VideoOS.Utilities.Configuration;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Registrars.Configuration;

public sealed class IncidentManagerOpenIdConnectConfiguration : IOpenIdConnectConfiguration
{
	private readonly string _authorizationServerUri = "AuthorizationServerUri";

	private readonly IConfigurationFileController _configurationFileController;

	public Uri AuthorizationServerAddress
	{
		get
		{
			return _configurationFileController.GetUriAppSetting(_authorizationServerUri);
		}
		set
		{
			_configurationFileController.SetOrUpdateAppSetting(_authorizationServerUri, (value == null) ? null : value.ToString());
		}
	}

	public IncidentManagerOpenIdConnectConfiguration([NotNull] IConfigurationFileController configurationFileController)
	{
		configurationFileController.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Registrars\\Configuration\\IncidentManagerOpenIdConnectConfiguration.cs", 19);
		_configurationFileController = configurationFileController;
	}
}
