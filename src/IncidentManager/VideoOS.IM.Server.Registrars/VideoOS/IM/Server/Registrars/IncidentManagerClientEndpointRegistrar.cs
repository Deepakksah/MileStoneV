using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using VideoOS.CertificateRegistration.Common;
using VideoOS.Common.Crypto;
using VideoOS.IM.Server.Registrars.Configuration;
using VideoOS.IM.Server.Registrars.Identities;
using VideoOS.IM.Server.Registrars.Properties;
using VideoOS.IM.Server.Service.OEM;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.Utilities.Configuration;
using VideoOS.Utilities.Security;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Registrars;

public sealed class IncidentManagerClientEndpointRegistrar : ClientEndpointRegistrarBase
{
	private const string IncidentManagerAppPoolName = "VideoOS IM AppPool";

	[NotNull]
	private readonly IOemResourceService _oemResourceService;

	[NotNull]
	private readonly IOpenIdConnectConfiguration _openIdConnectConfiguration;

	[NotNull]
	private readonly IAppPoolIdentityProvider _appPoolIdentityProvider;

	private static string CurrentBinFolderPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

	private static string ConfigFileFolderPath => Path.GetFullPath(Path.Combine(CurrentBinFolderPath, ".."));

	private static string ConfigFilePath => Path.Combine(ConfigFileFolderPath, "Web.config");

	public IncidentManagerClientEndpointRegistrar()
		: this(OemResourceService.Instance, new IncidentManagerOpenIdConnectConfiguration(new ConfigurationFileController(ConfigFilePath)), new ExeConfigurationRegistrationParametersHandler(ConfigFilePath, "Incident Manager Server", new List<string> { "managementserver" }), new FileSecretProvider(OemResourceService.Instance.CommonApplicationDataFolder, "idp.tmp", "idp.sec"), "VideoOS IM AppPool", new AppPoolIdentityProvider(new AccountWrapper()))
	{
	}

	public IncidentManagerClientEndpointRegistrar([NotNull] IOemResourceService oemResourceService, [NotNull] IOpenIdConnectConfiguration openIdConnectConfiguration, [NotNull] IRegistrationParametersHandler registrationParametersHandler, [NotNull] ISecretProvider secretProvider, [NotNull] string serviceName, [NotNull] IAppPoolIdentityProvider appPoolIdentityProvider)
		: base(registrationParametersHandler, secretProvider, serviceName, handleServiceStartStop: false)
	{
		oemResourceService.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Registrars\\IncidentManagerClientEndpointRegistrar.cs", 51);
		openIdConnectConfiguration.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Registrars\\IncidentManagerClientEndpointRegistrar.cs", 52);
		appPoolIdentityProvider.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Registrars\\IncidentManagerClientEndpointRegistrar.cs", 53);
		_oemResourceService = oemResourceService;
		_openIdConnectConfiguration = openIdConnectConfiguration;
		_appPoolIdentityProvider = appPoolIdentityProvider;
	}

	protected override Status Execute([CanBeNull] NetworkCredential identityServerCredentials, Uri authorizationService)
	{
		if (!Directory.Exists(_oemResourceService.CommonApplicationDataFolder))
		{
			Directory.CreateDirectory(_oemResourceService.CommonApplicationDataFolder);
		}
		Status status = base.Execute(identityServerCredentials, authorizationService);
		if (status.StatusCode == StatusCode.Success)
		{
			_openIdConnectConfiguration.AuthorizationServerAddress = new UriBuilder(_openIdConnectConfiguration.AuthorizationServerAddress)
			{
				Scheme = authorizationService.Scheme,
				Host = authorizationService.Host,
				Port = authorizationService.Port,
				Path = authorizationService.AbsolutePath
			}.Uri;
		}
		return status;
	}

	protected override string GetAccountUser()
	{
		return _appPoolIdentityProvider.GetAppPoolIdentity("VideoOS IM AppPool");
	}
}
