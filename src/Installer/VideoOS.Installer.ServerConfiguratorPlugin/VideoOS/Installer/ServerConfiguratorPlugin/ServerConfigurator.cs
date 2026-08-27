using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.XPath;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using VideoOS.CertificateRegistration.Common;
using VideoOS.Common.Integration;
using VideoOS.Common.Services;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.DatabasePlugin;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.ServerConfigurator;
using VideoOS.Server.Admin;
using VideoOS.Server.Admin.Tasks;
using VideoOS.Server.Admin.Utilities;
using VideoOS.ServerConfigurator.Interfaces;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Database.KeyManagement;
using VideoOS.Utilities.Formatting;
using VideoOS.Utilities.Network;
using VideoOS.Utilities.Security;
using VideoOS.Utilities.Windows.Http;

namespace VideoOS.Installer.ServerConfiguratorPlugin;

public class ServerConfigurator
{
	public delegate void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs e);

	private const string SqlFilesFolderName = "Database";

	private const string IisFolderName = "IIS";

	private const string CertificateIssuerPath = "Tools\\CertificateIssuer\\CertificateIssuer.exe";

	private const string CertificateIssuerKey = "Tools\\CertificateIssuer\\PublicKey.xml";

	private const string Registry_SCCM_PrimaryRegistryKey = "Software\\VideoOS\\SSCM";

	private const string VideoOSServerSubkeyName = "SOFTWARE\\\\VideoOS\\\\Server";

	private const string ClrEnabledRegistrySetting = "ClrEnabledOriginalSetting";

	private const string ClrStrictSecurityRegistrySetting = "ClrStrictSecurityOriginalSetting";

	private const string Registry_SCCM_InstalledPath = "InstalledPath";

	private const string SSCM_RegisterTempFile = "SSCMRegister.temp";

	private Dictionary<ServerParameters, string> _serverParameters;

	private string _curDir;

	private string _sscmSuppressTempFileName;

	private const int _numbTasksInstallation = 11;

	private const int _numbTasksUninstallation = 4;

	private int _progress;

	private int _maxProgress;

	public int MaxProgress => _maxProgress;

	public event ProgressChangedEventHandler OnProgressChanged;

	public ServerConfigurator(List<IPackageParameter> installParameters, bool installation, string productSku)
	{
		_curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		_sscmSuppressTempFileName = Path.Combine(_curDir, "SSCMRegister.temp");
		InitParams(installParameters, productSku, installation);
		_maxProgress = (installation ? 11 : 4);
	}

	public ServerConfigurator(List<IPackageParameter> installParameters, bool installation)
		: this(installParameters, installation, null)
	{
	}

	private void InitParams(List<IPackageParameter> installParameters, string productSku, bool installation)
	{
		_serverParameters = new Dictionary<ServerParameters, string>();
		foreach (IPackageParameter installParameter in installParameters)
		{
			switch (installParameter.Target)
			{
			case "INSTALLLOCATION":
				_serverParameters[ServerParameters.DestinationDir] = installParameter.Value;
				break;
			case "SQL-KEEP-DATA":
				_serverParameters[ServerParameters.SqlKeepData] = installParameter.Value;
				break;
			case "SQL-CREATE-DATABASE":
				_serverParameters[ServerParameters.SqlCreateDatabase] = installParameter.Value;
				break;
			case "IS_EXTERNALLY_MANAGED":
				_serverParameters[ServerParameters.IsExternallyManaged] = installParameter.Value;
				break;
			case "SQL_CONNECTION_STRING_MS":
				_serverParameters[ServerParameters.SqlConnectionStringMs] = installParameter.Value;
				break;
			case "SQL_CONNECTION_STRING_IDP":
				_serverParameters[ServerParameters.SqlConnectionStringIdp] = installParameter.Value;
				break;
			case "SQL_CONNECTION_STRING_IM":
				_serverParameters[ServerParameters.SqlConnectionStringIm] = installParameter.Value;
				break;
			case "SQL_CONNECTION_STRING_ES":
				_serverParameters[ServerParameters.SqlConnectionStringEs] = installParameter.Value;
				break;
			case "SQL_CONNECTION_STRING_LOG":
				_serverParameters[ServerParameters.SqlConnectionStringLog] = installParameter.Value;
				break;
			case "WebSiteId":
				_serverParameters[ServerParameters.WebSiteId] = installParameter.Value;
				break;
			case "MANAGEMENTAPPNAME":
				_serverParameters[ServerParameters.WebSiteAddress] = installParameter.Value;
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
					_serverParameters[ServerParameters.AccountPassword] = EncryptHelper.DecryptPassword(installParameter.Value, unicodePassword: true);
				}
				break;
			case "SYSTEMPASSWORD":
				_serverParameters[ServerParameters.SystemPassword] = installParameter.Value;
				break;
			case "LICENSE":
			{
				string value = installParameter.Value.Substring(installParameter.Value.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) + 1);
				_serverParameters[ServerParameters.LicenseKey] = value;
				break;
			}
			case "USAGEDATA":
				_serverParameters[ServerParameters.CollectUsageData] = installParameter.Value;
				break;
			}
		}
		if (_serverParameters.ContainsKey(ServerParameters.DestinationDir))
		{
			_serverParameters[ServerParameters.SqlScriptsFolder] = Path.Combine(_curDir, "Database");
			_serverParameters[ServerParameters.IisFolder] = Path.Combine(_serverParameters[ServerParameters.DestinationDir], "IIS");
			_serverParameters[ServerParameters.CertificatePathIssuer] = Path.Combine(_serverParameters[ServerParameters.DestinationDir], "Tools\\CertificateIssuer\\CertificateIssuer.exe");
			_serverParameters[ServerParameters.CertificatePathKey] = Path.Combine(_serverParameters[ServerParameters.DestinationDir], "Tools\\CertificateIssuer\\PublicKey.xml");
		}
		else
		{
			LogManager.Write("Error ServerConfigurator: Management Server Destination dir is not found!");
		}
		if (_serverParameters[ServerParameters.SqlKeepData].Equals("no", StringComparison.OrdinalIgnoreCase) || _serverParameters[ServerParameters.SqlKeepData].Equals("false", StringComparison.OrdinalIgnoreCase))
		{
			_serverParameters[ServerParameters.SqlCreateDatabase] = "yes";
			LogManager.Write("Reseting SqlCreateDatabase to 'yes' because SqlKeepData was 'no' or 'false'.");
		}
		if (_serverParameters[ServerParameters.SqlKeepData].Equals("yes", StringComparison.OrdinalIgnoreCase) || _serverParameters[ServerParameters.SqlKeepData].Equals("true", StringComparison.OrdinalIgnoreCase))
		{
			_serverParameters[ServerParameters.SqlCreateDatabase] = "no";
			LogManager.Write("Reseting SqlCreateDatabase to 'no' because SqlKeepData was 'yes' or 'true'.");
		}
		if (!string.IsNullOrEmpty(productSku))
		{
			_serverParameters[ServerParameters.ProductSku] = productSku;
		}
		LogManager.Write("Parameters Configurator will run with");
		foreach (KeyValuePair<ServerParameters, string> serverParameter in _serverParameters)
		{
			if (serverParameter.Key.ToString().IndexOf("password", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "{0}={1}", serverParameter.Key, "*"));
			}
			else
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "{0}={1}", serverParameter.Key, serverParameter.Value));
			}
		}
	}

	public void RunInstall(bool isHWMM, bool isFinalizedMode)
	{
		if (!isFinalizedMode)
		{
			LogManager.Write("Calling RegisterDownloadManagerSite");
			RegisterDownloadManagerSite(_serverParameters[ServerParameters.DestinationDir]);
		}
		if (isHWMM)
		{
			_progress = MaxProgress;
			UpdateProgress();
			return;
		}
		UpdateProgress();
		string text = Path.Combine(LogManager.GetMsiLogPath(), "ConfiguratorI.log");
		ILogger logger = (ILogger)(object)new LogComponent(text);
		UpdateProgress();
		int port = new Uri(_serverParameters[ServerParameters.WebSiteAddress]).Port;
		DatabaseConnectionInfo databaseConnectionInfo = new DatabaseConnectionInfo(_serverParameters[ServerParameters.SqlConnectionStringMs]);
		SqlConnectionManager managementServerSqlConnectionManager = new SqlConnectionManager(databaseConnectionInfo, null);
		if (!_serverParameters[ServerParameters.IsExternallyManaged].Equals("true", StringComparison.OrdinalIgnoreCase))
		{
			WaitForSqlServer(databaseConnectionInfo.CloneWithMasterDatabaseName().ConnectionString);
		}
		WriteConnectionStrings();
		string sourceDirName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), Path.Combine(Path.Combine(OemResources.Instance.App.CompanyName, OemResources.Instance.Server.OldDefaultInstallDir), "ExternalPlugins"));
		LogManager.Write("Calling CreateUpdateConfigFile");
		ServerConfigFileUpdater.CreateUpdateConfigFile(_serverParameters);
		UpdateProgress();
		LogManager.Write("Calling CreateSettingsFile");
		TelemetrySettingsUpdater.CreateSettingsFile();
		UpdateProgress();
		LogManager.Write("Calling StartServerServiceTrayController");
		ServerConfiguratorTrayController.StartServerServiceTrayController(_serverParameters);
		UpdateProgress();
		LogManager.Write("Calling CreateShortcuts");
		ServerConfiguratorShortcuts.CreateShortcuts(_serverParameters);
		UpdateProgress();
		LogManager.Write("Calling CopyOldExternalPlugins");
		string destDirName = Path.Combine(_serverParameters[ServerParameters.DestinationDir], "ExternalPlugins");
		CopyOldExternalPlugins(sourceDirName, destDirName);
		IdpConfigUpdater.EnsureOldIdpAppSettingsValues(_serverParameters[ServerParameters.DestinationDir]);
		Uri managementServer = ServerConfigurationDataModelProvider.Instance.CertificateQueryModel.GetManagementServer();
		IdpConfigUpdater.EnsureOldIdpAppSettingsValues(_serverParameters[ServerParameters.DestinationDir]);
		IdpConfigUpdater.ConfigureIdpSettings(_serverParameters, managementServer);
		SecureLogFolder("IDP");
		SecureLogFolder(OemResources.Instance.IncidentManagerResources.IncidentManagerApplicationName);
		ServerConfigure(logger, port, text, RegisterManagementServer, managementServerSqlConnectionManager);
		UpdateProgress();
		LogManager.Write("Configuration is done");
	}

	private void SecureLogFolder(string applicationFolderName)
	{
		SecureFolder secureFolder = new SecureFolder();
		string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), OemResources.Instance.App.CompanyName, applicationFolderName);
		secureFolder.CreateSecureFolder(folderPath, _serverParameters[ServerParameters.AccountName]);
	}

	private void RegisterManagementServer()
	{
		string location = _serverParameters[ServerParameters.DestinationDir];
		ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.SetLocationOnEndpointToBeInstalled(ServerConfiguratorData.ManagementServerConfigurationElementId, location);
		SystemConfigurationStatus systemConfigurationStatus = ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.CommitConfigurationElementsToBeInstalled(new Guid[1] { ServerConfiguratorData.ManagementServerConfigurationElementId });
		if (systemConfigurationStatus.ResultType() != RegistrationResultType.Success)
		{
			throw new ServerConfiguratorFailedException(FormattableString.Invariant($"Failed while committing System configuration, error status code: {systemConfigurationStatus}"));
		}
		ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.SetLocationOnEndpointToBeInstalled(ServerConfiguratorData.ManagementServerId, location);
		ApplyStatus applyStatus = ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.ApplyCertificatesForEndpointsToBeInstalled(new Guid[1] { ServerConfiguratorData.ManagementServerId });
		if (applyStatus.ResultType() != RegistrationResultType.Success)
		{
			throw new ServerConfiguratorFailedException(FormattableString.Invariant($"Failed while applying certificates, error status code: {applyStatus}"));
		}
		ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.SetLocationOnEndpointToBeInstalled(ServerConfiguratorData.ManagementServerClientId, location);
		RegistrationResult registrationResult = ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.ConfigureClientEndpointsToBeInstalled(new Guid[1] { ServerConfiguratorData.ManagementServerClientId }, offlineRegistration: false);
		if (registrationResult.ResultType != RegistrationResultType.Success)
		{
			throw new ServerConfiguratorFailedException(FormattableString.Invariant($"Failed while registering client, error status code: {registrationResult}"));
		}
		ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.SetLocationOnEndpointToBeInstalled(ServerConfiguratorData.ReportServerId, location);
		applyStatus = ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.ApplyCertificatesForEndpointsToBeInstalled(new Guid[1] { ServerConfiguratorData.ReportServerId });
		if (applyStatus.ResultType() != RegistrationResultType.Success)
		{
			throw new ServerConfiguratorFailedException(FormattableString.Invariant($"Failed while applying certificates, error status code: {applyStatus}"));
		}
		ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.SetLocationOnEndpointToBeInstalled(ServerConfiguratorData.DownloadManagerClientId, location);
		registrationResult = ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.ConfigureClientEndpointsToBeInstalled(new Guid[1] { ServerConfiguratorData.DownloadManagerClientId }, offlineRegistration: false);
		if (registrationResult.ResultType != RegistrationResultType.Success)
		{
			throw new ServerConfiguratorFailedException(FormattableString.Invariant($"Failed while registering client, error status code: {registrationResult}"));
		}
		string location2 = Path.Combine(_serverParameters[ServerParameters.IisFolder], "IncidentManager", "bin");
		ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.SetLocationOnEndpointToBeInstalled(ServerConfiguratorData.IncidentManagerServerId, location2);
		ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.SetLocationOnEndpointToBeInstalled(ServerConfiguratorData.IncidentManagerClientId, location2);
		applyStatus = ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.ApplyCertificatesForEndpointsToBeInstalled(new Guid[1] { ServerConfiguratorData.IncidentManagerServerId });
		if (applyStatus.ResultType() != RegistrationResultType.Success)
		{
			throw new ServerConfiguratorFailedException(FormattableString.Invariant($"Failed while applying certificates, error status code: {applyStatus}"));
		}
		registrationResult = ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.ConfigureClientEndpointsToBeInstalled(new Guid[1] { ServerConfiguratorData.IncidentManagerClientId }, offlineRegistration: false);
		if (registrationResult.ResultType != RegistrationResultType.Success)
		{
			throw new ServerConfiguratorFailedException(FormattableString.Invariant($"Failed while registering client, error status code: {registrationResult}"));
		}
	}

	private static ushort ReadWebApiPortFromConfig()
	{
		return ReadWebApiPortFromConfigIfAny() ?? 9000;
	}

	private static ushort? ReadWebApiPortFromConfigIfAny()
	{
		string configFilePath = OemResources.Instance.Server.ConfigFilePath;
		ushort? result = null;
		if (!File.Exists(configFilePath))
		{
			return result;
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(configFilePath);
		XPathNavigator xPathNavigator = xmlDocument.CreateNavigator().CreateNavigator().SelectSingleNode("/server/WebApiConfig/Port");
		if (xPathNavigator != null)
		{
			result = (ushort)xPathNavigator.ValueAsInt;
		}
		return result;
	}

	private void ServerConfigure(ILogger logger, int webSitePort, string configuratorLogPath, Action registerManagementServer, ISqlConnectionManager managementServerSqlConnectionManager)
	{
		try
		{
			bool flag = _serverParameters[ServerParameters.SqlCreateDatabase] == "no" && _serverParameters[ServerParameters.SqlKeepData] == "yes";
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Use existing database: {0}", flag));
			bool flag2 = _serverParameters[ServerParameters.IsExternallyManaged].Equals("true", StringComparison.OrdinalIgnoreCase);
			LogManager.Write($"IsExternallyManaged {flag2}");
			Version version = null;
			if (flag)
			{
				LogManager.Write("Getting current DB version...");
				version = GetCurrentDbVersion(managementServerSqlConnectionManager);
				if (!flag2 && version == null)
				{
					throw new ServerConfiguratorFailedException("Could not retrieve database version.");
				}
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Current database version is: {0}, isExternallyManaged: {1}", version, flag2));
			}
			IEncryptionKeyCreator encryptionKeyCreator = KeyManagerFactory.GetEncryptionKeyCreator(OemResources.Instance.App.CompanyName, managementServerSqlConnectionManager, logger);
			string password = null;
			if (KeyManagementPasswordHelper.GetSystemPasswordMode(encryptionKeyCreator, new VideoOS.Installer.Common.GlobalDataWrapper(), logger) != SystemPasswordMode.NoPasswordRequired)
			{
				LoggerExtensions.LogInformation(logger, "Using user provided system configuration password.", Array.Empty<object>());
				password = EncryptHelper.DecryptPassword(_serverParameters[ServerParameters.SystemPassword], unicodePassword: true);
			}
			DatabaseConnectionInfo databaseConnectionInfo = new DatabaseConnectionInfo(_serverParameters[ServerParameters.SqlConnectionStringMs]);
			DatabaseConnectionInfo databaseConnectionInfo2 = new DatabaseConnectionInfo(_serverParameters[ServerParameters.SqlConnectionStringIdp]);
			DatabaseConnectionInfo databaseConnectionInfo3 = new DatabaseConnectionInfo(_serverParameters[ServerParameters.SqlConnectionStringIm]);
			if (!flag2)
			{
				new ServerUtilities(logger).TakeDatabaseOwnership(databaseConnectionInfo);
				UpdateProgress();
				new ServerUtilities(logger).TakeDatabaseOwnership(databaseConnectionInfo2);
				UpdateProgress();
				new ServerUtilities(logger).TakeDatabaseOwnership(databaseConnectionInfo3);
				UpdateProgress();
			}
			LogManager.Write("Calling SetupDatabaseTask");
			new SetupDatabaseTask(logger).Install(databaseConnectionInfo, databaseConnectionInfo2, databaseConnectionInfo3, encryptionKeyCreator, _serverParameters[ServerParameters.ProductSku], _serverParameters[ServerParameters.AccountName], password, flag2, flag);
			UpdateProgress();
			LogManager.Write("Calling SetPrivacyPolicyTask");
			new SetPrivacyPolicyTask(logger).Create(databaseConnectionInfo, _serverParameters[ServerParameters.CollectUsageData]);
			UpdateProgress();
			LogManager.Write("Obtaining MasterKey");
			string text = Convert.ToBase64String(encryptionKeyCreator.GetUnencryptedKey());
			LogManager.Write("Calling CreateLicenseAsymmetricKeyTask");
			new CreateLicenseAsymmetricKeyTask(logger).Create(databaseConnectionInfo, text);
			UpdateProgress();
			AccountInfo serviceAccount = new AccountInfo(_serverParameters[ServerParameters.AccountName], _serverParameters[ServerParameters.AccountPassword], requirePassword: false);
			if (!flag2)
			{
				new ServerUtilities(logger).SetDatabaseAccount(serviceAccount, databaseConnectionInfo);
				UpdateProgress();
				new ServerUtilities(logger).SetDatabaseAccount(serviceAccount, databaseConnectionInfo2);
				UpdateProgress();
				new ServerUtilities(logger).SetDatabaseAccount(serviceAccount, databaseConnectionInfo3);
				UpdateProgress();
			}
			LogManager.Write("Calling SetupIisTask");
			new SetupIisTask(logger).Install(_serverParameters[ServerParameters.IisFolder], _serverParameters[ServerParameters.WebSiteId], webSitePort, serviceAccount, securedServer: false, string.Empty, registerManagementServer);
			UpdateProgress();
			LogManager.Write("Calling ServerUtilities SetAdminRoleAccount");
			new ServerUtilities(logger).SetAdminRoleAccount(serviceAccount, databaseConnectionInfo);
			using (WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent())
			{
				AccountInfo serviceAccount2 = new AccountInfo(windowsIdentity.Name, "", requirePassword: false);
				LogManager.Write("Calling ServerUtilities SetAdminRoleAccount to add the current user");
				new ServerUtilities(logger).SetAdminRoleAccount(serviceAccount2, databaseConnectionInfo);
			}
			LogManager.Write("Calling ServerUtilities SetWebServiceAccount");
			new ServerUtilities(logger).SetWebServiceAccount(serviceAccount, new Uri(_serverParameters[ServerParameters.WebSiteAddress]).Host, webSitePort, _serverParameters[ServerParameters.WebSiteId]);
			LogManager.Write("Calling CreateMediaDatabaseCertificateTask");
			new CreateMediaDatabaseCertificateTask(logger).Create(databaseConnectionInfo, _serverParameters[ServerParameters.CertificatePathIssuer], _serverParameters[ServerParameters.CertificatePathKey], text);
			UpdateProgress();
			LogManager.Write("Calling SetupMiscTask");
			new SetupMiscTask(logger).Install(databaseConnectionInfo, _serverParameters[ServerParameters.DestinationDir], webSitePort, flag2);
			UpdateProgress();
			Uri managementServer = ServerConfigurationDataModelProvider.Instance.CertificateQueryModel.GetManagementServer();
			LogManager.Write("Calling RegisterReportServer");
			RegisterReportServer(logger, databaseConnectionInfo, managementServer.Port, managementServer.Scheme);
			LogManager.Write("Calling RegisterIncidentManagerService");
			RegisterIncidentManagerService(logger, databaseConnectionInfo, managementServer.Port, managementServer.Scheme);
			UpdateProgress();
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogDebug(logger, ex.Message, Array.Empty<object>());
			LoggerExtensions.LogDebug(logger, ex.StackTrace, Array.Empty<object>());
			if (ex.InnerException != null)
			{
				LoggerExtensions.LogDebug(logger, ex.InnerException.Message, Array.Empty<object>());
				LoggerExtensions.LogDebug(logger, ex.InnerException.StackTrace, Array.Empty<object>());
			}
			throw new ServerConfiguratorFailedException(string.Format(CultureInfo.InvariantCulture, "Error occured while configuring the server. See log file at {0} for more information.", configuratorLogPath), ex);
		}
	}

	private void CopyOldExternalPlugins(string sourceDirName, string destDirName)
	{
		LogManager.Write("CopyOldExternalPlugins: Started...");
		if (!Directory.Exists(sourceDirName))
		{
			return;
		}
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "CopyOldExternalPlugins: Found folder {0}", sourceDirName));
		DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
		DirectoryInfo[] directories = directoryInfo.GetDirectories();
		if (!Directory.Exists(destDirName))
		{
			Directory.CreateDirectory(destDirName);
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "CopyOldExternalPlugins: Folder {0} created", destDirName));
		}
		FileInfo[] files = directoryInfo.GetFiles();
		foreach (FileInfo fileInfo in files)
		{
			string text = Path.Combine(destDirName, fileInfo.Name);
			if (!File.Exists(text))
			{
				fileInfo.CopyTo(text, overwrite: false);
			}
		}
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "CopyOldExternalPlugins: Files copied from {0}", sourceDirName));
		DirectoryInfo[] array = directories;
		foreach (DirectoryInfo directoryInfo2 in array)
		{
			string destDirName2 = Path.Combine(destDirName, directoryInfo2.Name);
			CopyOldExternalPlugins(directoryInfo2.FullName, destDirName2);
		}
	}

	private string GetConnectionString(SqlConnectionServerType serverType)
	{
		switch (serverType)
		{
		case SqlConnectionServerType.ManagementServer:
		case SqlConnectionServerType.ReportServer:
		case SqlConnectionServerType.ServerService:
			return _serverParameters[ServerParameters.SqlConnectionStringMs];
		case SqlConnectionServerType.EventServer:
			return _serverParameters[ServerParameters.SqlConnectionStringEs];
		case SqlConnectionServerType.IDP:
			return _serverParameters[ServerParameters.SqlConnectionStringIdp];
		case SqlConnectionServerType.IncidentManager:
			return _serverParameters[ServerParameters.SqlConnectionStringIm];
		case SqlConnectionServerType.LogServer:
			return _serverParameters[ServerParameters.SqlConnectionStringLog];
		default:
			throw new ArgumentException("Please specify server type or use null for master db connection string.");
		}
	}

	private void WriteConnectionStrings()
	{
		new DatabaseConnectionInfoConfigurator(SqlConnectionServerType.ManagementServer).Write(GetConnectionString(SqlConnectionServerType.ManagementServer));
		new DatabaseConnectionInfoConfigurator(SqlConnectionServerType.ReportServer).Write(GetConnectionString(SqlConnectionServerType.ReportServer));
		new DatabaseConnectionInfoConfigurator(SqlConnectionServerType.ServerService).Write(GetConnectionString(SqlConnectionServerType.ServerService));
		new DatabaseConnectionInfoConfigurator(SqlConnectionServerType.IDP).Write(GetConnectionString(SqlConnectionServerType.IDP));
		new DatabaseConnectionInfoConfigurator(SqlConnectionServerType.IncidentManager).Write(GetConnectionString(SqlConnectionServerType.IncidentManager));
		new DatabaseConnectionInfoConfigurator(SqlConnectionServerType.EventServer).Write(GetConnectionString(SqlConnectionServerType.EventServer));
		new DatabaseConnectionInfoConfigurator(SqlConnectionServerType.LogServer).Write(GetConnectionString(SqlConnectionServerType.LogServer));
	}

	private static void WaitForSqlServer(string connectionString)
	{
		int num = -1;
		int num2 = 10;
		string text = "";
		while (num != 0 && num2 > 0)
		{
			Exception ex = null;
			using (DatabaseConnection databaseConnection = new DatabaseConnection(connectionString, null))
			{
				text = databaseConnection.Database;
				try
				{
					using DatabaseCommand databaseCommand = databaseConnection.CreateCommand("SELECT DATABASEPROPERTY( '" + databaseConnection.Database + "' , 'IsInRecovery' )");
					databaseCommand.CommandType = System.Data.CommandType.Text;
					using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader(CommandBehavior.SequentialAccess);
					if (databaseDataReader.Read())
					{
						num = databaseDataReader.GetInt32(0);
					}
				}
				catch (DatabaseException ex2)
				{
					ex = ex2;
				}
			}
			if (num != 0)
			{
				LogManager.Write("Waiting for SQL server to be online...");
				if (ex != null)
				{
					LogManager.Write(ex);
				}
				Thread.Sleep(TimeSpan.FromSeconds(3.0));
				num2--;
			}
		}
		if (num == 0)
		{
			LogManager.Write("Connected to database " + text);
		}
		if (num2 == 0)
		{
			throw new TimeoutException("Timeout waiting for database.");
		}
	}

	private static string GetSCCMInstallPath(RegistryView registryView, bool suppressError = false)
	{
		string result = null;
		using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
		{
			using RegistryKey registryKey2 = registryKey.OpenSubKey("Software\\VideoOS\\SSCM");
			if (registryKey2 != null)
			{
				result = (string)registryKey2.GetValue("InstalledPath");
			}
			else if (!suppressError)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Could not open subkey = {0}", "Software\\VideoOS\\SSCM"), TraceEventType.Error);
			}
		}
		return result;
	}

	private static string GetSCCMInstallPathLegacy()
	{
		return GetSCCMInstallPath(RegistryView.Registry64, suppressError: true) ?? GetSCCMInstallPath(RegistryView.Registry32);
	}

	private void RegisterDownloadManagerSite(string installDir)
	{
		LogManager.Write("RegisterDownloadManagerSite: Started...");
		string sCCMInstallPath = GetSCCMInstallPath(RegistryView.Registry64);
		if (sCCMInstallPath == null)
		{
			LogManager.Write("Could not find SCCM_InstalledPath", TraceEventType.Error);
			return;
		}
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Found SCCM_InstalledPath = {0}", sCCMInstallPath));
		string empty = string.Empty;
		ProcessStartInfo processStartInfo = new ProcessStartInfo();
		processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		processStartInfo.RedirectStandardOutput = true;
		processStartInfo.UseShellExecute = false;
		processStartInfo.FileName = Path.Combine(sCCMInstallPath, "SSCMRegister.exe");
		using (Process process = new Process())
		{
			processStartInfo.Arguments = string.Format(CultureInfo.InvariantCulture, "startsuppress \"{0}\"", _sscmSuppressTempFileName);
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Executing {0} {1}", processStartInfo.FileName, processStartInfo.Arguments), TraceEventType.Information);
			process.StartInfo = processStartInfo;
			process.Start();
			empty = process.StandardOutput.ReadToEnd();
			if (!process.WaitForExit(20000))
			{
				process.Kill();
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SSCMRegister did not exit after 20 sec's, killing process, StandardOutput =  {0}", empty), TraceEventType.Error);
				return;
			}
			if (process.ExitCode != 0)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SSCMRegister exited with code {0}{1}StandardOutput =  {2}", process.ExitCode, Environment.NewLine, empty), TraceEventType.Error);
				return;
			}
		}
		using (Process process2 = new Process())
		{
			processStartInfo.Arguments = string.Format(CultureInfo.InvariantCulture, "installsystem \"{0}\\IIS\\httpdocs\" \"VideoOS\" \"{1}\" ", installDir, OemResources.Instance.Server.FullName);
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Executing {0} {1}", processStartInfo.FileName, processStartInfo.Arguments), TraceEventType.Information);
			process2.StartInfo = processStartInfo;
			process2.Start();
			empty = null;
			empty = process2.StandardOutput.ReadToEnd();
			if (!process2.WaitForExit(20000))
			{
				process2.Kill();
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SSCMRegister did not exit after 20 sec's, killing process, StandardOutput =  {0}", empty), TraceEventType.Error);
				return;
			}
			if (process2.ExitCode != 0)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SSCMRegister exited with code {0}{1}StandardOutput =  {2}", process2.ExitCode, Environment.NewLine, empty), TraceEventType.Error);
				return;
			}
		}
		using (Process process3 = new Process())
		{
			processStartInfo.Arguments = string.Format(CultureInfo.InvariantCulture, "installpage \"{0}\"", OemResources.Instance.Server.FullName) + " \"VideoOS\" \"Default\" \"default-{0}.htm\"" + string.Format(CultureInfo.InvariantCulture, " \"{0}\\IIS\\httpdocs\" ", installDir);
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Executing {0} {1}", processStartInfo.FileName, processStartInfo.Arguments), TraceEventType.Information);
			process3.StartInfo = processStartInfo;
			process3.Start();
			empty = null;
			empty = process3.StandardOutput.ReadToEnd();
			if (!process3.WaitForExit(20000))
			{
				process3.Kill();
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SSCMRegister did not exit after 20 sec's, killing process, StandardOutput =  {0}", empty), TraceEventType.Error);
				return;
			}
			if (process3.ExitCode != 0)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SSCMRegister exited with code {0}{1}StandardOutput =  {2}", process3.ExitCode, Environment.NewLine, empty), TraceEventType.Error);
				return;
			}
		}
		using (Process process4 = new Process())
		{
			processStartInfo.Arguments = string.Format(CultureInfo.InvariantCulture, "installpage \"{0}\"", OemResources.Instance.Server.FullName) + " \"VideoOS\" \"Admin\" \"default-{0}.htm\"" + string.Format(CultureInfo.InvariantCulture, "  \"{0}\\IIS\\httpdocs\\admin\" ", installDir);
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Executing {0} {1}", processStartInfo.FileName, processStartInfo.Arguments), TraceEventType.Information);
			process4.StartInfo = processStartInfo;
			process4.Start();
			empty = null;
			empty = process4.StandardOutput.ReadToEnd();
			if (!process4.WaitForExit(20000))
			{
				process4.Kill();
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SSCMRegister did not exit after 20 sec's, killing process, StandardOutput =  {0}", empty), TraceEventType.Error);
				return;
			}
			if (process4.ExitCode != 0)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SSCMRegister exited with code {0}{1}StandardOutput =  {2}", process4.ExitCode, Environment.NewLine, empty), TraceEventType.Error);
				return;
			}
		}
		using (Process process5 = new Process())
		{
			processStartInfo.Arguments = string.Format(CultureInfo.InvariantCulture, "stopsuppresssilent \"{0}\"", _sscmSuppressTempFileName);
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Executing {0} {1}", processStartInfo.FileName, processStartInfo.Arguments), TraceEventType.Information);
			process5.StartInfo = processStartInfo;
			process5.Start();
			empty = null;
			empty = process5.StandardOutput.ReadToEnd();
			if (!process5.WaitForExit(20000))
			{
				process5.Kill();
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SSCMRegister did not exit after 20 sec's, killing process, StandardOutput =  {0}", empty), TraceEventType.Error);
				return;
			}
			if (process5.ExitCode != 0)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SSCMRegister exited with code {0}{1}StandardOutput =  {2}", process5.ExitCode, Environment.NewLine, empty), TraceEventType.Error);
				return;
			}
		}
		string text = string.Format(CultureInfo.InvariantCulture, "{0}\\IIS\\httpdocs\\", installDir);
		string text2 = text + "admin\\";
		if (!Directory.Exists(text2))
		{
			Directory.CreateDirectory(text2);
		}
		CreateDefaultHtmlPage(text);
		CreateDefaultHtmlPage(text2);
		LogManager.Write("RegisterDownloadManagerSite Done.", TraceEventType.Information);
	}

	private static void CreateDefaultHtmlPage(string htmlFolderPath)
	{
		string text = htmlFolderPath + "default.htm";
		if (!File.Exists(text))
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "File {0} does not exist", text), TraceEventType.Information);
			using StreamWriter streamWriter = new StreamWriter(text);
			streamWriter.WriteLine(string.Format(CultureInfo.InvariantCulture, "<html><head><META http-equiv=\"refresh\" content=\"0;URL=default-{0}.htm\"></head></html>", GetDefaultLanguageBasedOnOS(htmlFolderPath)));
		}
	}

	private static string GetDefaultLanguageBasedOnOS(string htmlDirectoryPath)
	{
		string name = CultureInfo.InstalledUICulture.Name;
		string[] files = Directory.GetFiles(htmlDirectoryPath);
		for (int i = 0; i < files.Length; i++)
		{
			Match match = Regex.Match(files[i], "default-(.+?).htm");
			if (match.Success && match.Groups.Count > 1 && match.Groups[1].Value.Equals(name))
			{
				return name;
			}
		}
		return "en-US";
	}

	private void UnRegisterDownloadManagerSite(string installDir)
	{
		string sCCMInstallPathLegacy = GetSCCMInstallPathLegacy();
		if (sCCMInstallPathLegacy == null)
		{
			LogManager.Write("Could not find SCCM_InstalledPath", TraceEventType.Error);
			return;
		}
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Found SCCM_InstalledPath = {0}", sCCMInstallPathLegacy));
		string empty = string.Empty;
		ProcessStartInfo processStartInfo = new ProcessStartInfo();
		processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		processStartInfo.RedirectStandardOutput = true;
		processStartInfo.UseShellExecute = false;
		processStartInfo.FileName = Path.Combine(sCCMInstallPathLegacy, "SSCMRegister.exe");
		try
		{
			using (Process process = new Process())
			{
				processStartInfo.Arguments = string.Format(CultureInfo.InvariantCulture, "startsuppress \"{0}\"", _sscmSuppressTempFileName);
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Executing {0} {1}", processStartInfo.FileName, processStartInfo.Arguments), TraceEventType.Information);
				process.StartInfo = processStartInfo;
				process.Start();
				empty = process.StandardOutput.ReadToEnd();
				if (process.WaitForExit(20000))
				{
					if (process.ExitCode != 0)
					{
						LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SSCMRegister exited with code {0}{1}StandardOutput =  {2}", process.ExitCode, Environment.NewLine, empty), TraceEventType.Error);
					}
				}
				else
				{
					process.Kill();
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SSCMRegister did not exit after 20 sec's, killing process, StandardOutput =  {0}", empty), TraceEventType.Error);
				}
			}
			string[] array = new string[7] { "VideoOSRecorder", "VideoOSManager", "SCInstaller", "VideoOSLogServer", "VideoOSEventServer", "MobileServer", "GatewayService" };
			string[] array2 = new string[1] { "DlnaServer" };
			string[] array3 = array;
			for (int i = 0; i < array3.Length; i++)
			{
				RemoveGroupFromDownloadManager(array3[i], sCCMInstallPathLegacy);
			}
			array3 = array2;
			for (int i = 0; i < array3.Length; i++)
			{
				RemoveGroupFromDownloadManager(array3[i], sCCMInstallPathLegacy, suppressErrors: true);
			}
			using (Process process2 = new Process())
			{
				processStartInfo.Arguments = string.Format(CultureInfo.InvariantCulture, "uninstallsystem \"{0}\" ", OemResources.Instance.Server.FullName);
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Executing {0} {1}", processStartInfo.FileName, processStartInfo.Arguments), TraceEventType.Information);
				process2.StartInfo = processStartInfo;
				process2.Start();
				empty = process2.StandardOutput.ReadToEnd();
				if (process2.WaitForExit(20000))
				{
					if (process2.ExitCode != 0)
					{
						LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SSCMRegister exited with code {0}{1}StandardOutput =  {2}", process2.ExitCode, Environment.NewLine, empty), TraceEventType.Error);
					}
				}
				else
				{
					process2.Kill();
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SSCMRegister did not exit after 20 sec's, killing process, StandardOutput =  {0}", empty), TraceEventType.Error);
				}
			}
			using (Process process3 = new Process())
			{
				processStartInfo.Arguments = string.Format(CultureInfo.InvariantCulture, "stopsuppresssilent \"{0}\"", _sscmSuppressTempFileName);
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Executing {0} {1}", processStartInfo.FileName, processStartInfo.Arguments), TraceEventType.Information);
				process3.StartInfo = processStartInfo;
				process3.Start();
				empty = process3.StandardOutput.ReadToEnd();
				if (process3.WaitForExit(20000))
				{
					if (process3.ExitCode != 0)
					{
						LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SSCMRegister exited with code {0}{1}StandardOutput =  {2}", process3.ExitCode, Environment.NewLine, empty), TraceEventType.Error);
					}
				}
				else
				{
					process3.Kill();
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SSCMRegister did not exit after 20 sec's, killing process, StandardOutput =  {0}", empty), TraceEventType.Error);
				}
			}
			FileInfo[] files = new DirectoryInfo(string.Format(CultureInfo.InvariantCulture, "{0}\\IIS\\httpdocs", installDir)).GetFiles("default*.htm");
			foreach (FileInfo fileInfo in files)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Removing file = {0}", fileInfo.Name), TraceEventType.Information);
				fileInfo.Delete();
			}
			files = new DirectoryInfo(string.Format(CultureInfo.InvariantCulture, "{0}\\IIS\\httpdocs\\admin", installDir)).GetFiles("default*.htm");
			foreach (FileInfo fileInfo2 in files)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Removing file = {0}", fileInfo2.Name), TraceEventType.Information);
				fileInfo2.Delete();
			}
			if (File.Exists(string.Format(CultureInfo.InvariantCulture, "{0}\\IIS\\httpdocs\\", installDir) + "sscm.config.xml"))
			{
				File.Delete(string.Format(CultureInfo.InvariantCulture, "{0}\\IIS\\httpdocs\\", installDir) + "sscm.config.xml");
			}
			if (File.Exists(string.Format(CultureInfo.InvariantCulture, "{0}\\IIS\\httpdocs\\admin\\", installDir) + "sscm.config.xml"))
			{
				File.Delete(string.Format(CultureInfo.InvariantCulture, "{0}\\IIS\\httpdocs\\admin\\", installDir) + "sscm.config.xml");
			}
			LogManager.Write("UnRegisterDownloadManagerSite Done.", TraceEventType.Information);
		}
		catch (Exception ex)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "UnRegisterDownloadManagerSite: Exception occured: {0} {1} {2}", ex.Message, Environment.NewLine, ex.StackTrace), TraceEventType.Error);
		}
	}

	private static void RemoveGroupFromDownloadManager(string groupName, string installerPath, bool suppressErrors = false)
	{
		using Process process = new Process();
		ProcessStartInfo processStartInfo = new ProcessStartInfo();
		processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		processStartInfo.RedirectStandardOutput = true;
		processStartInfo.UseShellExecute = false;
		processStartInfo.FileName = Path.Combine(installerPath, "SSCMRegister.exe");
		processStartInfo.Arguments = string.Format(CultureInfo.InvariantCulture, "removegroup  {0} ", groupName);
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Executing {0} {1}", processStartInfo.FileName, processStartInfo.Arguments), TraceEventType.Information);
		process.StartInfo = processStartInfo;
		process.Start();
		string text = process.StandardOutput.ReadToEnd();
		if (process.WaitForExit(20000))
		{
			if (process.ExitCode != 0 && !suppressErrors)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SSCMRegister exited with code {0}{1}StandardOutput =  {2}", process.ExitCode, Environment.NewLine, text), TraceEventType.Error);
			}
		}
		else
		{
			process.Kill();
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SSCMRegister did not exit after 20 sec's, killing process, StandardOutput =  {0}", text), TraceEventType.Error);
		}
	}

	private void UpdateProgress()
	{
		_progress++;
		if (_progress > MaxProgress)
		{
			_progress = MaxProgress;
		}
		if (this.OnProgressChanged != null)
		{
			this.OnProgressChanged(this, new ProgressChangedEventArgs(_progress));
		}
	}

	private static void RegisterIncidentManagerService(ILogger logComponent, IDatabaseConnectionInfo databaseConnectionInfo, int webServerPort, string scheme)
	{
		UriBuilder uriBuilder = new UriBuilder
		{
			Scheme = scheme,
			Host = IPHelper.GetFullCumputerName(),
			Port = webServerPort,
			Path = "IncidentManager/"
		};
		LogManager.Write("Calling RegisterServiceTask");
		new RegisterServiceTask(logComponent).Register(databaseConnectionInfo, VideoOS.Common.Services.ServiceTypes.ServiceType_IncidentManagerService, "Incident Manager service", uriBuilder.Uri);
	}

	private static void RegisterReportServer(ILogger logComponent, IDatabaseConnectionInfo databaseConnectionInfoMs, int webServerPort, string scheme)
	{
		UriBuilder uriBuilder = new UriBuilder();
		uriBuilder.Scheme = scheme;
		uriBuilder.Host = IPHelper.GetFullCumputerName();
		uriBuilder.Port = webServerPort;
		uriBuilder.Path = "Reporting/";
		LogManager.Write("Calling RegisterServiceTask");
		new RegisterServiceTask(logComponent).Register(databaseConnectionInfoMs, VideoOS.Common.Services.ServiceTypes.ServiceType_ReportService, "Report Server", uriBuilder.Uri);
	}

	public void RunUninstall()
	{
		ILogger val = (ILogger)(object)new LogComponent(Path.Combine(LogManager.GetMsiLogPath(), "ConfiguratorU.log"));
		try
		{
			LogManager.Write("Calling RemoveShortcuts");
			ServerConfiguratorShortcuts.RemoveShortcuts();
			UpdateProgress();
			bool num = IsVersionWithOldEncryptionImplementation();
			if (num)
			{
				LogManager.Write("Calling RemoveIDPSecret");
				ServerConfiguratorClientRegistration.RemoveIDPSecrets();
				UpdateProgress();
			}
			LogManager.Write("Calling SetupIisTask");
			new SetupIisTask(val).Uninstall(_serverParameters[ServerParameters.WebSiteId]);
			UpdateProgress();
			if (num)
			{
				LogManager.Write("Calling RemoveSslConfigurationAndUrlReservation");
				RemoveSslConfigurationAndUrlReservation();
			}
			LogManager.Write("Calling UnRegisterDownloadManagerSite");
			UnRegisterDownloadManagerSite(_serverParameters[ServerParameters.DestinationDir]);
			UpdateProgress();
			LogManager.Write("Calling RemoveSqlConfigurationFromRegistry");
			RemoveSqlConfigurationFromRegistry();
			UpdateProgress();
			LogManager.Write("Calling RemoveHelpFiles");
			RemoveHelpFiles(_serverParameters[ServerParameters.DestinationDir]);
			UpdateProgress();
			LogManager.Write("Uninstalling configuration is done");
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogDebug(val, ex.Message, Array.Empty<object>());
			LoggerExtensions.LogDebug(val, ex.StackTrace, Array.Empty<object>());
			throw;
		}
	}

	private static void RemoveSqlConfigurationFromRegistry()
	{
		try
		{
			using RegistryKey registryKey = Registry.LocalMachine;
			using RegistryKey registryKey2 = registryKey.OpenSubKey("SOFTWARE\\\\VideoOS\\\\Server", writable: true);
			if (registryKey2 != null)
			{
				registryKey2.DeleteValue("ClrEnabledOriginalSetting");
				registryKey2.DeleteValue("ClrStrictSecurityOriginalSetting");
			}
		}
		catch (Exception arg)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "RemoveSqlConfigurationFromRegistry(). Error occurred. {0}{1}", Environment.NewLine, arg));
		}
	}

	private static void RemoveHelpFiles(string installDir)
	{
		try
		{
			string path = Path.Combine(installDir, "IIS", "HtmlHelp");
			if (Directory.Exists(path))
			{
				Directory.Delete(path, recursive: true);
			}
		}
		catch (Exception arg)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "RemoveHelpFiles(). Error occurred. {0}{1}", Environment.NewLine, arg));
		}
	}

	private static void RemoveSslConfigurationAndUrlReservation()
	{
		ServerHttpSysConfigurator.RemoveServerHttpSysConfigurationAndReservation(ReadWebApiPortFromConfig());
	}

	public static LegacyEncryption GetManagementServerLegacyEncryption()
	{
		ushort? num = ReadWebApiPortFromConfigIfAny();
		if (num.HasValue)
		{
			string text = new HttpSysSettings(WellKnownAppIds.ServerApiService, num.Value).CertificateHash?.AsString();
			if (string.IsNullOrEmpty(text))
			{
				return LegacyEncryption.Unencrypted;
			}
			return LegacyEncryption.Encrypted(text);
		}
		return LegacyEncryption.Empty;
	}

	public static void RemoveCertificateRegistrationAndPersistData()
	{
		try
		{
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveClientEndpointInstalled(ServerConfiguratorData.DownloadManagerClientId);
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveServerEndpointInstalled(ServerConfiguratorData.ManagementServerId);
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveServerEndpointInstalled(ServerConfiguratorData.ReportServerId);
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveClientEndpointInstalled(ServerConfiguratorData.ManagementServerClientId);
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveConfigurationElementInstalled(ServerConfiguratorData.ManagementServerConfigurationElementId);
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveClientEndpointInstalled(ServerConfiguratorData.IncidentManagerClientId);
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveServerEndpointInstalled(ServerConfiguratorData.IncidentManagerServerId);
		}
		catch (Exception arg)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "RemoveCertificateRegistration(). Error occurred. {0}{1}", Environment.NewLine, arg));
		}
	}

	private static bool IsVersionWithOldEncryptionImplementation()
	{
		try
		{
			Version currentDbVersion = GetCurrentDbVersion(null);
			if (currentDbVersion == null)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "IsVersionWithOldEncryptionImplementation. Unable to get existing version"));
				return false;
			}
			return currentDbVersion < new Version(20, 2);
		}
		catch (Exception arg)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "IsVersionWithOldEncryptionImplementation. Exception occurred.\r\nException: {0}", arg));
			return false;
		}
	}

	private static Version GetDatabaseVersion(ISqlConnectionManager managementServerSqlConnectionManager)
	{
		try
		{
			string commandText = "SELECT [version] FROM [dbo].[System]";
			using DatabaseConnection databaseConnection = managementServerSqlConnectionManager.GetSqlConnection();
			using DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText);
			using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
			if (databaseDataReader.Read())
			{
				return new Version(databaseDataReader.GetString(0));
			}
		}
		catch (Exception ex)
		{
			if (ex is DatabaseException && ((DatabaseException)ex).IsObjectNotFoundException)
			{
				return null;
			}
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "GetDatabaseVersion. Exception occurred.\r\nException: {0}", ex));
		}
		return null;
	}

	private static Version GetCurrentDbVersion(ISqlConnectionManager managementServerSqlConnectionManager)
	{
		try
		{
			if (managementServerSqlConnectionManager != null)
			{
				return GetDatabaseVersion(managementServerSqlConnectionManager);
			}
			if (InstallerManager.Instance.OldInstallInfo == null)
			{
				return null;
			}
			string value = InstallerManager.Instance.OldInstallInfo.InstallEnvironment.Parameters.Single((KeyValueParameters<string, string> x) => x.Key == "VERSION_MAJOR").Value;
			string value2 = InstallerManager.Instance.OldInstallInfo.InstallEnvironment.Parameters.Single((KeyValueParameters<string, string> x) => x.Key == "VERSION_MINOR").Value;
			string value3 = InstallerManager.Instance.OldInstallInfo.InstallEnvironment.Parameters.Single((KeyValueParameters<string, string> x) => x.Key == "VERSION_REVISION").Value;
			return new Version(value + "." + value2 + "." + value3);
		}
		catch (Exception arg)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Retrieving current database version has failed.\r\nException: {0}", arg));
		}
		return null;
	}
}
