using System;
using System.Configuration;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using Server.UACParts;
using VideoOS.CertificateRegistration.Common;
using VideoOS.Common.Crypto;
using VideoOS.Common.Integration;
using VideoOS.IdentityServer.Common;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.Server.CertificateRegistration;

public class RegistrationIisManagementService : IRegistrationIisManagementService
{
	private const string ClientRegistrationKeyName = "ClientRegistrationId";

	private const string AuthorizationServerKeyName = "AuthorizationServerUri";

	private const string ReportServerAppPoolName = "VideoOS ReportServer AppPool";

	private const string ManagementServerAppPoolName = "VideoOS Management Server AppPool";

	public void RegisterReportService(string reportConfigFilePath, Uri authorizationServerUri, ILogger logger)
	{
		Register(reportConfigFilePath, authorizationServerUri, OemResources.Instance.ReportWebServer.CommonApplicationDataFolder, "Report Server", new string[3] { "datacollector", "managementserver", "failover" }, "VideoOS ReportServer AppPool", logger);
	}

	public void RegisterIisManagementService(string iisConfigFilePath, Uri authorizationServerUri, ILogger logger)
	{
		string secretPath = Path.Combine(OemResources.Instance.Server.CommonApplicationDataFolder, "IIS", "ManagementServer");
		Register(iisConfigFilePath, authorizationServerUri, secretPath, "Management Server", new string[4] { "datacollector", "idp_management", "managementserver", "serverlogs" }, "VideoOS Management Server AppPool", logger);
	}

	private static void Register(string configFilePath, Uri authorizationServerUri, string secretPath, string clientName, string[] scopes, string appPoolName, ILogger logger)
	{
		if (configFilePath == null)
		{
			throw new ArgumentNullException("configFilePath");
		}
		if (!File.Exists(configFilePath))
		{
			throw new FileNotFoundException("Cannot find the configuration file.", configFilePath);
		}
		if (authorizationServerUri == null)
		{
			authorizationServerUri = ReadAuthAddress(configFilePath);
		}
		string clientId = ReadClientIdFromConfig(configFilePath);
		string accountUser = new IisAccountUtility().GetAccountUser(appPoolName);
		RegisterClientResponse registerClientResponse = null;
		IRegistrationClient client = new IdpRegistrationClient(authorizationServerUri);
		try
		{
			registerClientResponse = ClientEndpointRegistrarBase.RegisterClientWithRetry(() => client.RegisterClient(clientId, clientName, scopes), logger);
		}
		finally
		{
			if (client != null)
			{
				client.Dispose();
			}
		}
		WriteSecretToDisk(registerClientResponse.Secret, secretPath, accountUser);
		WriteAddressToConfig(configFilePath, authorizationServerUri.ToString());
	}

	private static Uri ReadAuthAddress(string configFileName)
	{
		Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap
		{
			ExeConfigFilename = configFileName
		}, ConfigurationUserLevel.None);
		KeyValueConfigurationElement keyValueConfigurationElement = configuration.AppSettings.Settings["AuthorizationServerUri"];
		if (keyValueConfigurationElement == null)
		{
			AddOrUpdateAppSettings("AuthorizationServerUri", "", configuration);
			keyValueConfigurationElement = configuration.AppSettings.Settings["AuthorizationServerUri"];
		}
		return new Uri(keyValueConfigurationElement.Value);
	}

	private static void WriteSecretToDisk(string secret, string secretPath, string serviceAccount)
	{
		if (!Directory.Exists(secretPath))
		{
			Directory.CreateDirectory(secretPath);
		}
		using SecretHandler secretHandler = new SecretHandler(new FileSecretProvider(secretPath, "idp.tmp", "idp.sec"));
		FileSystemAccessRule rule = new FileSystemAccessRule(serviceAccount, FileSystemRights.Read | FileSystemRights.Write | FileSystemRights.Delete, AccessControlType.Allow);
		secretHandler.WriteSecret(Encoding.UTF8.GetBytes(secret), rule);
	}

	private static string ReadClientIdFromConfig(string configFileName)
	{
		Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap
		{
			ExeConfigFilename = configFileName
		}, ConfigurationUserLevel.None);
		KeyValueConfigurationElement keyValueConfigurationElement = configuration.AppSettings.Settings["ClientRegistrationId"];
		if (keyValueConfigurationElement == null)
		{
			AddOrUpdateAppSettings("ClientRegistrationId", "", configuration);
			keyValueConfigurationElement = configuration.AppSettings.Settings["ClientRegistrationId"];
		}
		Guid guid = Guid.NewGuid();
		if (string.IsNullOrEmpty(keyValueConfigurationElement.Value))
		{
			AddOrUpdateAppSettings("ClientRegistrationId", guid.ToString(), configuration);
		}
		if (Guid.TryParse(keyValueConfigurationElement.Value, out var result))
		{
			return result.ToString();
		}
		throw new RegistrationFailedException("The management server configuration file contains an invalid registration id.");
	}

	private static void WriteAddressToConfig(string configFileName, string authorizationServerAddress)
	{
		Configuration openMappedExeConfiguration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap
		{
			ExeConfigFilename = configFileName
		}, ConfigurationUserLevel.None);
		AddOrUpdateAppSettings("AuthorizationServerUri", authorizationServerAddress, openMappedExeConfiguration);
	}

	private static void AddOrUpdateAppSettings(string key, string value, Configuration openMappedExeConfiguration)
	{
		KeyValueConfigurationCollection settings = openMappedExeConfiguration.AppSettings.Settings;
		if (settings[key] == null)
		{
			settings.Add(key, value);
		}
		else
		{
			settings[key].Value = value;
		}
		openMappedExeConfiguration.Save(ConfigurationSaveMode.Modified);
	}

	public void CleanupIisManagementService()
	{
		DeleteSecretFiles(Path.Combine(OemResources.Instance.Server.CommonApplicationDataFolder, "IIS", "ManagementServer"));
	}

	public void CleanupReportService()
	{
		DeleteSecretFiles(OemResources.Instance.ReportWebServer.CommonApplicationDataFolder);
	}

	private static void DeleteSecretFiles(string secretFolder)
	{
		FileSecretProvider fileSecretProvider = new FileSecretProvider(secretFolder, "idp.tmp", "idp.sec");
		fileSecretProvider.DeleteTempSecret();
		fileSecretProvider.DeletePermSecret();
	}
}
