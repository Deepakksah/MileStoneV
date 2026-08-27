using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml.Linq;
using Newtonsoft.Json;
using Server.UACParts;
using VideoOS.CertificateRegistration.Common;
using VideoOS.Common.Crypto;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Common.Integration;
using VideoOS.Management.VmoClient;
using VideoOS.Server.Admin.Utilities;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.Server.CertificateRegistration;

public class ManagementServerSystemConfigurationRegistrar : WindowsServiceConfiguratorBase, ISystemConfiguration, ISystemConfigurationCleanUp
{
	private readonly IFileService _fileService;

	private readonly IRegistrationIisManagementService _registrationIisManagementService;

	private readonly Func<ILogger, IisHelper> _iisHelperFactory;

	private readonly Func<Uri, Uri, NetworkCredential, IVmoSystemSettingClient> _vmoSystemSettingClientFactory;

	public ManagementServerSystemConfigurationRegistrar()
		: this(OemResources.Instance.Server.ServiceName, new FileService(), new RegistrationIisManagementService(), IisHelperDefaultFactory, (Uri serverAddress, Uri identityProvider, NetworkCredential networkCredential) => new VmoSystemSettingClient(serverAddress, identityProvider, networkCredential), new ServiceControllerFactory())
	{
	}

	public ManagementServerSystemConfigurationRegistrar(string serviceName, IFileService fileService, IRegistrationIisManagementService registrationIisManagementService, Func<ILogger, IisHelper> iisHelperFactory, Func<Uri, Uri, NetworkCredential, IVmoSystemSettingClient> vmoSystemSettingClientFactory, IServiceControllerFactory serviceControllerFactory)
		: base(serviceName, new TimeSpan(0, 3, 0), serviceControllerFactory)
	{
		_fileService = fileService;
		_registrationIisManagementService = registrationIisManagementService;
		_iisHelperFactory = iisHelperFactory;
		_vmoSystemSettingClientFactory = vmoSystemSettingClientFactory;
	}

	private static IisHelper IisHelperDefaultFactory(ILogger logger)
	{
		return new IisHelper(new ILoggerWrapper(logger));
	}

	public Status PostConfigureSystem()
	{
		try
		{
			if (IsSecondaryInFailoverCluster())
			{
				base.Logger?.LogInfo("Failover is enabled and this node does not own the virtual IP. Skipping Management Server start.");
				return Status.SuccessStatus();
			}
			return LoggedIfSuccessOrWarning(StartServiceAndWait(), "PostConfigureSystem");
		}
		catch (ServiceConfigurationException exception)
		{
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorRegistration, null, "PostConfigureSystem");
		}
		catch (Exception exception2)
		{
			return LoggedErrorStatus(exception2, StatusCode.UnhandledException, null, "PostConfigureSystem");
		}
	}

	public Status PreConfigureSystem()
	{
		try
		{
			return LoggedIfSuccessOrWarning(PreExecute(), "PreConfigureSystem");
		}
		catch (ServiceConfigurationException exception)
		{
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorRegistration, null, "PreConfigureSystem");
		}
	}

	public Status CleanUp()
	{
		try
		{
			return LoggedIfSuccessOrWarning(ExecuteCleanup(), "CleanUp");
		}
		catch (ServiceConfigurationException exception)
		{
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorRegistration, null, "CleanUp");
		}
	}

	private Status ExecuteCleanup()
	{
		base.Logger?.LogInfo(FormattableString.Invariant($"About to ExecuteCleanup"));
		try
		{
			_registrationIisManagementService.CleanupIisManagementService();
		}
		catch (RemoveFileSecretException ex)
		{
			base.Logger?.LogError(ex.ToString());
		}
		base.Logger?.LogInfo(FormattableString.Invariant($"ExecuteCleanup has been done."));
		return Status.SuccessStatus();
	}

	public Status ConfigureSystem(string oldHostName, Uri configurationAddress, Uri communicationAddress, Uri authorizationServer, Credential credentials)
	{
		if (configurationAddress == null)
		{
			throw new ArgumentNullException("configurationAddress");
		}
		try
		{
			return LoggedIfSuccessOrWarning(Execute(oldHostName, communicationAddress, authorizationServer, credentials), "ConfigureSystem");
		}
		catch (Exception exception)
		{
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorRegistration, null, "ConfigureSystem");
		}
	}

	private Status Execute(string oldHostName, Uri communicationAddress, Uri authorizationServer, NetworkCredential networkCredential)
	{
		IisHelper iisHelper = _iisHelperFactory(base.Logger);
		Status status = UpdateIdpAppSetting(authorizationServer, iisHelper);
		string iisConfigFilePath = ServerTrayUtility.ResolveManagementServerWebConfig(iisHelper);
		try
		{
			_registrationIisManagementService.RegisterIisManagementService(iisConfigFilePath, authorizationServer, base.Logger);
		}
		catch (Exception ex) when (!(ex is OutOfMemoryException))
		{
			var (status2, description) = ClientEndpointRegistrarBase.ResolveException(ex, out var _);
			return LoggedStatus(status2, description, "Execute");
		}
		if (status.StatusCode == StatusCode.Success)
		{
			status = UpdateServerAddresses(oldHostName, communicationAddress, authorizationServer, networkCredential);
		}
		return status;
	}

	private Status UpdateIdpAppSetting(Uri authorizationAddress, IisHelper iisHelper)
	{
		string configPath = ResolveIdpAppSettings(iisHelper);
		UpdateAppSettingsIdentityProvider(authorizationAddress, configPath);
		base.Logger.LogInfo(FormattableString.Invariant(FormattableStringFactory.Create("Recycling the App Pool \"{0}\"", "VideoOS IDP AppPool")));
		AppPoolState appPoolState = iisHelper.RecycleAppPool("VideoOS IDP AppPool");
		long num = 3000L;
		Stopwatch stopwatch = Stopwatch.StartNew();
		while (appPoolState != AppPoolState.Started && stopwatch.ElapsedMilliseconds < num)
		{
			appPoolState = iisHelper.GetAppPoolState("VideoOS IDP AppPool");
			Thread.Sleep(10);
		}
		if (appPoolState != AppPoolState.Started)
		{
			base.Logger.LogError(FormattableString.Invariant(FormattableStringFactory.Create("The application pool \"{0}\", did not get started within {1} msecs", "VideoOS IDP AppPool", num)));
			return RestartIis(iisHelper);
		}
		base.Logger.LogInfo(FormattableString.Invariant(FormattableStringFactory.Create("App Pool \"{0}\" Recycled", "VideoOS IDP AppPool")));
		return Status.SuccessStatus();
	}

	private Status RestartIis(IisHelper iisHelper)
	{
		base.Logger.LogInfo("Restarting IIS, to get IDP up and running");
		iisHelper.Control(IisAction.Restart);
		base.Logger.LogInfo("IIS was restarted");
		return Status.SuccessStatus();
	}

	private static string ResolveIdpAppSettings(IisHelper iisHelper)
	{
		return Path.Combine(iisHelper.GetPhysicalPath(IisHelper.ConfiguredWebSiteId, "/IDP"), "appsettings.json");
	}

	private string UpdateAppSettingsIdentityProvider(Uri address, string configPath)
	{
		dynamic val = JsonConvert.DeserializeObject(_fileService.ReadAllText(configPath));
		string text = val["IdentityProviderSettings"]["Authority"];
		if (text != null)
		{
			val["IdentityProviderSettings"]["Authority"] = IdpUriParser.FormatAuthority(address, text.ToString());
		}
		else
		{
			val["IdentityProviderSettings"]["Authority"] = IdpUriParser.FormatAuthority(address);
		}
		string contents = JsonConvert.SerializeObject(val, Formatting.Indented);
		_fileService.WriteAllText(configPath, contents);
		return text;
	}

	private Status UpdateServerAddresses(string oldHostName, Uri newManagementServerAddress, Uri authorizationServer, NetworkCredential networkCredential)
	{
		if (oldHostName.Equals(newManagementServerAddress.Host, StringComparison.OrdinalIgnoreCase))
		{
			return Status.SuccessStatus();
		}
		try
		{
			using IVmoSystemSettingClient vmoSystemSettingClient = _vmoSystemSettingClientFactory(newManagementServerAddress, authorizationServer, networkCredential);
			vmoSystemSettingClient.Connect();
			if (Uri.TryCreate(vmoSystemSettingClient.GetSystemSetting("ServerLanAddress"), UriKind.Absolute, out var result) && oldHostName.Equals(result.Host, StringComparison.OrdinalIgnoreCase))
			{
				UriBuilder uriBuilder = new UriBuilder(result);
				uriBuilder.Host = newManagementServerAddress.Host;
				vmoSystemSettingClient.SetSystemSetting("ServerLanAddress", uriBuilder.ToString());
				base.Logger?.LogInfo(FormattableString.Invariant($"Updated ServerLanAddress system setting. Host name of Url has changed, address set in database: {uriBuilder}"));
			}
			List<Address> list = new List<Address>();
			foreach (Address managementServerAddress in vmoSystemSettingClient.ManagementServerAddresses)
			{
				if (oldHostName.Equals(managementServerAddress.ServerUrl.Host, StringComparison.OrdinalIgnoreCase) && managementServerAddress.AddressType != AddressType.Server)
				{
					list.Add(managementServerAddress);
				}
			}
			if (list.Count > 0)
			{
				vmoSystemSettingClient.RemoveSiteAddresses(list);
			}
		}
		catch (VmoException ex) when (ex is VmoUnauthorizedAccessException || ex is VmoInsufficientPermissionsException || ex is UserNamePasswordException)
		{
			return Status.ErrorStatus(StatusCode.VmsAdminCredentialsNeeded);
		}
		catch (ConnectionFailureException)
		{
			return Status.ErrorStatus(StatusCode.ManagementServerNotFound);
		}
		return Status.SuccessStatus();
	}

	private static string GetConfigPath()
	{
		string path = "ServerConfig.xml";
		string commonApplicationDataFolder = OemResources.Instance.Server.CommonApplicationDataFolder;
		string text = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);
		string text2 = Path.Combine(commonApplicationDataFolder, path);
		if (!File.Exists(text2))
		{
			if (!File.Exists(text))
			{
				return null;
			}
			return text;
		}
		return text2;
	}

	private static bool IsFailoverClusterMember()
	{
		string configPath = GetConfigPath();
		if (configPath == null)
		{
			return false;
		}
		return string.Equals(XDocument.Load(configPath).Root?.Element("Failover")?.Element("ClusterMember")?.Value?.Trim(), "true", StringComparison.OrdinalIgnoreCase);
	}

	private static string GetFailoverVirtualIP()
	{
		string configPath = GetConfigPath();
		if (configPath == null)
		{
			return null;
		}
		return XDocument.Load(configPath).Root?.Element("Failover")?.Element("VirtualAddress")?.Value?.Trim();
	}

	private static bool IsSecondaryInFailoverCluster()
	{
		if (IsFailoverClusterMember())
		{
			string failoverVirtualIP = GetFailoverVirtualIP();
			if (!string.IsNullOrEmpty(failoverVirtualIP) && !(from ip in Dns.GetHostAddresses(Dns.GetHostName())
				select ip.ToString()).Contains(failoverVirtualIP))
			{
				return true;
			}
		}
		return false;
	}
}
