using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Timers;
using System.Xml;
using Microsoft.Extensions.Logging;
using VideoOS.GatewayService.InstallerPlugin.GatewayServiceInstallerUtils;
using VideoOS.GatewayService.Utilities;
using VideoOS.GatewayService.Utilities.Oem;
using VideoOS.Installer.Common.Dism;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.ServerConfigurator;
using VideoOS.Server.Admin.Utilities;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.GatewayService.InstallerPlugin;

public class ServerConfigurator : IServerConfigurator
{
	private object _threadLock = new object();

	private Dictionary<ServerParameters, string> _serverParameters;

	private const int _numbTasksInstallation = 3;

	private const int _iisUpdateSteps = 15;

	private const int _numbTasksUninstallation = 3;

	private int _iisUpdateProgress;

	private int _progress;

	private int _maxProgress;

	private IRegistryHelper _registryHelper;

	private ITimerWrapper _iisUpdateTimer;

	private IEncryptHelperWrapper _encryptHelper;

	private IXmlHelper _xmlHelper;

	private ISecureFolderFactory _secureFolderFactory;

	private ISetupIISFactory _setupIIISFactory;

	private IServerConfigurationDataModelProviderWrapper _serverConfigurationDataModelProviderWrapper;

	private IDismSessionFactory _dismSessionFactory;

	private IGatewayServiceRegistrationWrapper _gatewayServiceRegistrationWrapper;

	private IUriHelper _uriHelper;

	private IDnsHelper _dnsHelper;

	private ILogger LogComponent { get; }

	private IGatewayServiceOemResource OemResource { get; }

	public int MaxProgress => _maxProgress;

	public bool RebootRequiered { get; private set; }

	public event ProgressChangedEventHandler OnProgressChanged;

	public ServerConfigurator(IEnumerable<IPackageParameter> installParameters, bool installation)
	{
		OemResource = new GatewayServiceOemResource();
		_registryHelper = new RegistryHelper(OemResource);
		_maxProgress = (installation ? 18 : 3);
		LogComponent = LogManager.InnerLog;
		_iisUpdateTimer = new TimerWrapper();
		_encryptHelper = new EncryptHelperWrapper();
		_xmlHelper = new XmlHelper();
		_secureFolderFactory = new SecureFolderFactory();
		_setupIIISFactory = new SetupIISFactory();
		_serverConfigurationDataModelProviderWrapper = new ServerConfigurationDataModelProviderWrapper();
		_dismSessionFactory = new DismSessionFactory();
		_gatewayServiceRegistrationWrapper = new GatewayServiceRegistrationWrapper();
		_uriHelper = new UriHelper(LogComponent);
		_dnsHelper = new DnsHelper();
		InitParams(installParameters, installation);
	}

	public ServerConfigurator(IGatewayServiceOemResource oemResource, IRegistryHelper registryHelper, ILogger logComponent, ITimerWrapper timer, IEncryptHelperWrapper encryptHelper, IXmlHelper xmlHelper, ISecureFolderFactory secureFolderFactory, ISetupIISFactory setupIISFactory, IServerConfigurationDataModelProviderWrapper serverConfigurationDataModelProviderWrapper, IDismSessionFactory dismSessionFactory, IGatewayServiceRegistrationWrapper gatewayServiceRegistrationWrapper, IUriHelper uriHelper, IDnsHelper dnsHelper, IEnumerable<IPackageParameter> installParameters, bool installation)
	{
		OemResource = oemResource;
		_registryHelper = registryHelper;
		LogComponent = logComponent;
		_iisUpdateTimer = timer;
		_encryptHelper = encryptHelper;
		_xmlHelper = xmlHelper;
		_secureFolderFactory = secureFolderFactory;
		_setupIIISFactory = setupIISFactory;
		_serverConfigurationDataModelProviderWrapper = serverConfigurationDataModelProviderWrapper;
		_dismSessionFactory = dismSessionFactory;
		_gatewayServiceRegistrationWrapper = gatewayServiceRegistrationWrapper;
		_uriHelper = uriHelper;
		_dnsHelper = dnsHelper;
		InitParams(installParameters, installation);
		_maxProgress = (installation ? 18 : 3);
	}

	private void InitParams(IEnumerable<IPackageParameter> installParameters, bool installation)
	{
		_serverParameters = new Dictionary<ServerParameters, string>();
		foreach (IPackageParameter installParameter in installParameters)
		{
			switch (installParameter.Target)
			{
			case "INSTALLFOLDER":
				_serverParameters[ServerParameters.DestinationDir] = installParameter.Value;
				break;
			case "WebSiteId":
				_serverParameters[ServerParameters.WebSiteId] = installParameter.Value;
				break;
			case "WebSiteAddress":
				_serverParameters[ServerParameters.WebSiteName] = installParameter.Value;
				break;
			case "USERACCOUNT":
				_serverParameters[ServerParameters.UserAccount] = installParameter.Value;
				break;
			case "ACCOUNTNAME":
				_serverParameters[ServerParameters.AccountName] = installParameter.Value;
				break;
			case "ENCRYPTEDPASSWORD":
				if (installation)
				{
					_serverParameters[ServerParameters.AccountPassword] = _encryptHelper.DecryptPassword(installParameter.Value, unicodePassword: true);
				}
				break;
			case "MANAGEMENTSERVERURI":
				_serverParameters[ServerParameters.ManagementServerUri] = installParameter.Value;
				break;
			case "IS_UPGRADE":
				_serverParameters[ServerParameters.IsUpgrade] = installParameter.Value;
				break;
			case "SQL-CREATE-DATABASE":
				_serverParameters[ServerParameters.CreateDatabase] = installParameter.Value;
				break;
			}
		}
		if (!_serverParameters.ContainsKey(ServerParameters.DestinationDir))
		{
			LoggerExtensions.LogDebug(LogComponent, "An error has occurred while running the installation, no destination folder found", Array.Empty<object>());
		}
		LoggerExtensions.LogDebug(LogComponent, "Using following parameters", Array.Empty<object>());
		foreach (KeyValuePair<ServerParameters, string> serverParameter in _serverParameters)
		{
			if (serverParameter.Key.ToString().IndexOf("password", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, "{0}={1}", serverParameter.Key, "*"), Array.Empty<object>());
			}
			else
			{
				LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, "{0}={1}", serverParameter.Key, serverParameter.Value), Array.Empty<object>());
			}
		}
	}

	public void RunInstall()
	{
		UpdateProgress();
		UpdateGatewayConfig();
		UpdateNLogConfig();
		CreateSecureProgramDataFolder();
		StoreParametersInRegistry();
		InitTimerForIIS();
		try
		{
			ServerConfigure(RegisterGatewayService);
		}
		finally
		{
			StopTimerForIIS();
		}
		EnableWebSockets();
		UpdateProgress();
		if (!_gatewayServiceRegistrationWrapper.TryRegisterService(GetTrueFalseParameterAsBoolean(ServerParameters.IsUpgrade), GetYesNoParameterAsBoolean(ServerParameters.CreateDatabase), _serverParameters[ServerParameters.ManagementServerUri], _serverParameters[ServerParameters.WebSiteId]))
		{
			throw new GatewayServiceRegistrationException($"Gateway Service registration failed. Management Server Uri: {_serverParameters[ServerParameters.ManagementServerUri]}.");
		}
		UpdateProgress();
		LoggerExtensions.LogDebug(LogComponent, "Configuration is done", Array.Empty<object>());
	}

	private void EnableWebSockets()
	{
		IDismSession dismSession = null;
		DismStatus dismStatus = DismStatus.Success;
		try
		{
			LoggerExtensions.LogDebug(LogComponent, "Enable WebSockets for IIS", Array.Empty<object>());
			dismSession = _dismSessionFactory.CreateSession(LogComponent);
			dismSession.Open();
			dismStatus = dismSession.EnableFeature(Feature.IISWebSockets, enableAll: true);
			dismSession.Close();
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogDebug(LogComponent, ex.Message, Array.Empty<object>());
			LoggerExtensions.LogDebug(LogComponent, ex.StackTrace, Array.Empty<object>());
			throw;
		}
		finally
		{
			try
			{
				if (dismStatus == DismStatus.RebootRequired)
				{
					RebootRequiered = true;
				}
				dismSession?.Shutdown();
			}
			catch (Exception ex2)
			{
				LoggerExtensions.LogDebug(LogComponent, ex2.Message, Array.Empty<object>());
				LoggerExtensions.LogDebug(LogComponent, ex2.StackTrace, Array.Empty<object>());
				throw;
			}
		}
		LoggerExtensions.LogDebug(LogComponent, "Done enabling WebSockets for IIS", Array.Empty<object>());
	}

	public void RunUninstall()
	{
		UpdateProgress();
		try
		{
			LoggerExtensions.LogDebug(LogComponent, "Calling IIS to remove site", Array.Empty<object>());
			_setupIIISFactory.CreateSetupIIS().Uninstall(_serverParameters[ServerParameters.WebSiteId]);
			UpdateProgress();
			if (!GetTrueFalseParameterAsBoolean(ServerParameters.IsUpgrade))
			{
				if (!_gatewayServiceRegistrationWrapper.TryUnregisterService(_serverParameters[ServerParameters.ManagementServerUri]))
				{
					LoggerExtensions.LogDebug(LogComponent, $"Gateway Service unregistration failed. Management Server Uri: {_serverParameters[ServerParameters.ManagementServerUri]}.", Array.Empty<object>());
				}
				RemoveParametersFromRegistry();
			}
			UpdateProgress();
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogDebug(LogComponent, ex.Message, Array.Empty<object>());
			LoggerExtensions.LogDebug(LogComponent, ex.StackTrace, Array.Empty<object>());
			throw;
		}
	}

	private bool GetTrueFalseParameterAsBoolean(ServerParameters parameter)
	{
		if (!_serverParameters.ContainsKey(parameter))
		{
			return false;
		}
		if (!bool.TryParse(_serverParameters[parameter], out var result))
		{
			return false;
		}
		return result;
	}

	private bool GetYesNoParameterAsBoolean(ServerParameters parameter)
	{
		if (_serverParameters.TryGetValue(parameter, out var value))
		{
			return value.Equals("yes", StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	private void StoreParametersInRegistry()
	{
		LoggerExtensions.LogDebug(LogComponent, "Storing parameters in registry", Array.Empty<object>());
		try
		{
			_registryHelper.WriteKey(RegistryKeys.DestinationFolder, _serverParameters[ServerParameters.DestinationDir]);
			_registryHelper.WriteKey(RegistryKeys.WebsiteId, _serverParameters[ServerParameters.WebSiteId]);
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogDebug(LogComponent, "Failed to update registry. Exception: " + ex.Message, Array.Empty<object>());
			LoggerExtensions.LogDebug(LogComponent, ex.StackTrace, Array.Empty<object>());
			throw ex;
		}
	}

	private void RemoveParametersFromRegistry()
	{
		LoggerExtensions.LogDebug(LogComponent, "Clean up parameters in registry", Array.Empty<object>());
		try
		{
			_registryHelper.CleanUpRegistry();
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogDebug(LogComponent, "Failed clean registry. Exception: " + ex.Message, Array.Empty<object>());
			LoggerExtensions.LogDebug(LogComponent, ex.StackTrace, Array.Empty<object>());
			throw ex;
		}
	}

	private void ServerConfigure(Action registerGatewayService)
	{
		try
		{
			ISetupIIS setupIIS = _setupIIISFactory.CreateSetupIIS();
			setupIIS.SetupSecureBinding(_serverParameters[ServerParameters.WebSiteId], _serverParameters[ServerParameters.ManagementServerUri]);
			AccountInfo serviceAccount = new AccountInfo(_serverParameters[ServerParameters.AccountName], _serverParameters[ServerParameters.AccountPassword], requirePassword: false);
			LoggerExtensions.LogDebug(LogComponent, "Setting up IIS", Array.Empty<object>());
			setupIIS.Install(_serverParameters[ServerParameters.DestinationDir], _serverParameters[ServerParameters.WebSiteId], _serverParameters[ServerParameters.WebSiteName], serviceAccount, registerGatewayService);
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogDebug(LogComponent, ex.Message, Array.Empty<object>());
			LoggerExtensions.LogDebug(LogComponent, ex.StackTrace, Array.Empty<object>());
			if (ex.InnerException != null)
			{
				LoggerExtensions.LogDebug(LogComponent, ex.InnerException.Message, Array.Empty<object>());
				LoggerExtensions.LogDebug(LogComponent, ex.InnerException.StackTrace, Array.Empty<object>());
			}
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Error happened during Server Configure"), ex);
		}
	}

	private void UpdateProgress(int increaseBy = 1)
	{
		lock (_threadLock)
		{
			_progress += increaseBy;
			if (_progress > MaxProgress)
			{
				_progress = MaxProgress;
			}
			if (this.OnProgressChanged != null)
			{
				this.OnProgressChanged(this, new ProgressChangedEventArgs(_progress));
			}
		}
	}

	private void UpdateGatewayConfig()
	{
		try
		{
			LoggerExtensions.LogDebug(LogComponent, "Setting gateway configuration in file started", Array.Empty<object>());
			string gatewayConfigPath = _serverParameters[ServerParameters.DestinationDir] + "/appsettings.json";
			Uri uri = new UriBuilder(_serverParameters[ServerParameters.ManagementServerUri]).Uri;
			ConfigFileHelper configFileHelper = new ConfigFileHelper(LogComponent);
			string text = "with success";
			if (!configFileHelper.UpdateManagementServerUri(gatewayConfigPath, uri))
			{
				LoggerExtensions.LogDebug(LogComponent, "Failed to update uri of management server", Array.Empty<object>());
				text = "with failure";
			}
			string dllPath = _serverParameters[ServerParameters.DestinationDir];
			if (!configFileHelper.UpdateDllSearchPath(gatewayConfigPath, dllPath))
			{
				LoggerExtensions.LogDebug(LogComponent, "Failed to update path of Dlls", Array.Empty<object>());
				text = "with failure";
			}
			bool isSecure = uri.Scheme == Uri.UriSchemeHttps;
			string host = _dnsHelper.GetDnsHostName(Environment.MachineName).ToString();
			Uri gatewayServiceUri = _uriHelper.GetGatewayServiceUri(_serverParameters[ServerParameters.WebSiteId], isSecure, host);
			if (!configFileHelper.UpdateGatewayServiceUri(gatewayConfigPath, gatewayServiceUri))
			{
				LoggerExtensions.LogDebug(LogComponent, "Failed to update uri of gateway service", Array.Empty<object>());
				text = "with failure";
			}
			LoggerExtensions.LogDebug(LogComponent, "Setting gateway configuration in file ended - " + text, Array.Empty<object>());
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogError(LogComponent, ex, "An error has occurred while trying to update the Gateway configuration.", Array.Empty<object>());
			throw;
		}
	}

	private void UpdateNLogConfig()
	{
		try
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
			folderPath = Path.Combine(folderPath, OemResource.CompanyNameShort);
			folderPath = Path.Combine(folderPath, OemResource.ApiGatewayInstallDir);
			folderPath = Path.Combine(folderPath, "Logs");
			string path = _serverParameters[ServerParameters.DestinationDir] + "/nlog.config";
			XmlDocument xmlDocument = _xmlHelper.Load(path);
			_ = xmlDocument.DocumentElement;
			XmlNode xmlNode = (from XmlNode a in xmlDocument.GetElementsByTagName("variable")
				where a.Attributes["name"].Value.ToString() == "logDirectory"
				select a).First();
			if (xmlNode != null)
			{
				xmlNode.Attributes["value"].Value = folderPath;
			}
			_xmlHelper.Save(xmlDocument, path);
			LoggerExtensions.LogDebug(LogComponent, "NLog logging directory updated to " + folderPath, Array.Empty<object>());
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogError(LogComponent, ex, "An error has occurred while trying to update the NLog configuration.", Array.Empty<object>());
			throw;
		}
	}

	private void CreateSecureProgramDataFolder()
	{
		ISecureFolderWrapper secureFolder = _secureFolderFactory.GetSecureFolder();
		string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), OemResource.CompanyNameShort, OemResource.ApiGatewayInstallDir);
		secureFolder.CreateSecureFolder(folderPath, _serverParameters[ServerParameters.AccountName], userAccountReadOnly: false, exclusive: true);
	}

	private void RegisterGatewayService()
	{
		string path = _serverParameters[ServerParameters.DestinationDir];
		_serverConfigurationDataModelProviderWrapper.SetLocationOnEndpointToBeInstalled(ServerConfiguratorData.GatewayServiceId, Path.Combine(path, "Registrars"));
		_serverConfigurationDataModelProviderWrapper.SetLocationOnEndpointToBeInstalled(ServerConfiguratorData.GatewayServiceClientId, Path.Combine(path, "Registrars"));
		ApplyStatus applyStatus = _serverConfigurationDataModelProviderWrapper.ApplyCertificatesForEndpointsToBeInstalled(new Guid[1] { ServerConfiguratorData.GatewayServiceId });
		if (applyStatus.ResultType() != RegistrationResultType.Success)
		{
			throw new GatewayServiceCertificateRegistrationException(FormattableString.Invariant($"Failed while applying certificates, error status code: {applyStatus}"));
		}
		RegistrationResult registrationResult = _serverConfigurationDataModelProviderWrapper.ConfigureClientEndpointsToBeInstalled(new Guid[1] { ServerConfiguratorData.GatewayServiceClientId }, offlineRegistration: false);
		if (registrationResult.ResultType != RegistrationResultType.Success)
		{
			throw new GatewayServiceSetManagementServerException(FormattableString.Invariant($"Failed while registering GatewayServiceClientId for Set Management Server, error status code: {registrationResult}"));
		}
	}

	private void StopTimerForIIS()
	{
		_iisUpdateTimer.Stop();
		int num = 15 - _iisUpdateProgress;
		if (num > 0)
		{
			UpdateProgress(num);
		}
	}

	private void InitTimerForIIS()
	{
		_iisUpdateTimer.Elapsed += _certInstallTimer_Elapsed;
		_iisUpdateTimer.Interval = 5000.0;
		_iisUpdateTimer.Start();
	}

	private void _certInstallTimer_Elapsed(object sender, ElapsedEventArgs e)
	{
		_iisUpdateProgress++;
		if (_iisUpdateProgress < 15)
		{
			UpdateProgress();
		}
	}

	public void RemoveCertificateRegistration()
	{
		try
		{
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveServerEndpointInstalled(ServerConfiguratorData.GatewayServiceId);
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveClientEndpointInstalled(ServerConfiguratorData.GatewayServiceClientId);
		}
		catch (Exception arg)
		{
			LoggerExtensions.LogError(LogComponent, string.Format(CultureInfo.InvariantCulture, "RemoveCertificateRegistration(). Error occurred. {0}{1}", Environment.NewLine, arg), Array.Empty<object>());
		}
	}
}
