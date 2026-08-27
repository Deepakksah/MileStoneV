using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Xml;
using System.Xml.XPath;
using VideoOS.CertificateRegistration.Common;
using VideoOS.Common.Crypto;
using VideoOS.Common.Integration;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.RecorderPlugins;
using VideoOS.Installer.ServerConfigurator;
using VideoOS.Utilities.Windows.Http;

namespace VideoOS.Installer.RecorderTaskPlugin;

public static class RecorderConfiguration
{
	public static void RemoveWebApiServiceConfiguration(Dictionary<string, string> installParameters, bool isFailover)
	{
		if (installParameters == null)
		{
			throw new ArgumentNullException("installParameters");
		}
		LogManager.Write("RemoveWebApiServiceConfiguration(). Starting...");
		try
		{
			int portFromConfigFile = GetPortFromConfigFile();
			new HttpSysSettings(WellKnownAppIds.RecorderApiService, (ushort)portFromConfigFile).DeleteSettings();
			if (isFailover)
			{
				portFromConfigFile = GetFailoverRestAPIPortFromConfigFile();
				new HttpSysSettings(WellKnownAppIds.FailoverApiService, (ushort)portFromConfigFile, OemResources.Instance.FailoverServer.ServiceName).DeleteSettings();
			}
			LogManager.Write("RemoveWebApiServiceConfiguration(). Done !");
		}
		catch (Exception arg)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "RemoveWebApiServiceConfiguration(). Exception occurred.\r\nException: {0}", arg));
		}
	}

	public static void RemoveCertificateRegistrationAndPersistData(bool isFailover)
	{
		try
		{
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveServerEndpointInstalled(ServerConfiguratorData.RecorderServerId);
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveServerEndpointInstalled(ServerConfiguratorData.ImageServerServerId);
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveClientEndpointInstalled(ServerConfiguratorData.RecorderClientId);
			if (isFailover)
			{
				ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveServerEndpointInstalled(ServerConfiguratorData.FailoverServerId);
				ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveClientEndpointInstalled(ServerConfiguratorData.FailoverClientId);
			}
		}
		catch (Exception arg)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "RemoveCertificateRegistration(). Error occurred. {0}{1}", Environment.NewLine, arg));
		}
	}

	public static void RemoveIdentityProviderSecret()
	{
		LogManager.Write("Remove Authorization Server SecretFile: Started...");
		try
		{
			FileSecretProvider fileSecretProvider = new FileSecretProvider(OemResources.Instance.Recorder.SecureDataFolder, "idp.tmp", "idp.sec");
			fileSecretProvider.DeleteTempSecret();
			fileSecretProvider.DeletePermSecret();
		}
		catch (Exception ex)
		{
			LogManager.Write("Failed to delete Authorization Server secret during uninstallation", ex.ToString(), TraceEventType.Error);
		}
	}

	public static int GetPortFromConfigFile()
	{
		int result = 9001;
		string configFilePath = OemResources.Instance.Recorder.ConfigFilePath;
		if (File.Exists(configFilePath))
		{
			XmlDocument xmlDocument = new XmlDocument();
			using (XmlReader reader = XmlReader.Create(configFilePath))
			{
				xmlDocument.Load(reader);
			}
			XPathNavigator xPathNavigator = xmlDocument.CreateNavigator().CreateNavigator().SelectSingleNode("/recorderconfig/webapi/port");
			if (xPathNavigator != null)
			{
				result = xPathNavigator.ValueAsInt;
			}
		}
		return result;
	}

	public static int GetFailoverRestAPIPortFromConfigFile()
	{
		int result = 8990;
		string configFilePath = OemResources.Instance.FailoverServer.ConfigFilePath;
		if (File.Exists(configFilePath))
		{
			XmlDocument xmlDocument = new XmlDocument();
			using (XmlReader reader = XmlReader.Create(configFilePath))
			{
				xmlDocument.Load(reader);
			}
			XPathNavigator xPathNavigator = xmlDocument.CreateNavigator().CreateNavigator().SelectSingleNode("/recorderfailoverconfig/failoverservice/restapiport");
			if (xPathNavigator != null)
			{
				result = xPathNavigator.ValueAsInt;
			}
		}
		return result;
	}

	public static void SetAccountData(Dictionary<string, string> parameters)
	{
		LogManager.Write("SetAccountData(): Starting...");
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SetAccountData(): SERVICEUSER= {0}", parameters["SERVICEUSER"]));
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SetAccountData(): SERVICEACCOUNT= {0}", parameters["SERVICEACCOUNT"]));
		if (parameters["SERVICEUSER"] == "false")
		{
			parameters["ACCOUNTTYPE"] = "false";
			string value = ((NTAccount)new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null).Translate(typeof(NTAccount))).Value;
			string value2 = ((NTAccount)new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null).Translate(typeof(NTAccount))).Value;
			if (parameters["SERVICEACCOUNT"].Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				parameters["BUILTINACCOUNTNAME"] = "networkservice";
			}
			else if (parameters["SERVICEACCOUNT"].Equals(value2, StringComparison.OrdinalIgnoreCase))
			{
				parameters["BUILTINACCOUNTNAME"] = "localsystem";
			}
		}
		else
		{
			parameters["ACCOUNTTYPE"] = "true";
			parameters["ACCOUNTNAME"] = parameters["SERVICEACCOUNT"];
			if (!parameters.ContainsKey("REMOVE") || string.IsNullOrEmpty(parameters["REMOVE"]))
			{
				DecryptPassword(parameters);
			}
		}
	}

	private static void DecryptPassword(Dictionary<string, string> parameters)
	{
		if (parameters.ContainsKey("ENCRYPTEDPASSWORD"))
		{
			string text = parameters["ENCRYPTEDPASSWORD"];
			if (!string.IsNullOrEmpty(text))
			{
				string value = EncryptHelper.DecryptPassword(text, unicodePassword: true);
				parameters["Password"] = value;
			}
		}
	}

	public static void RemoveFirewallRule()
	{
		string text = "VideoOS.Recorder.Service.WebApiPorts";
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Remove firewall rule '{0}'", text));
		try
		{
			FirewallHelper.RemoveRule(text);
		}
		catch (FirewallException ex)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Could not remove firewall rule '{0}': {1}", text, ex.Message), TraceEventType.Warning);
		}
	}

	public static void CreateFirewallRule(Dictionary<string, string> installParameters)
	{
		if (installParameters == null)
		{
			throw new ArgumentNullException("installParameters");
		}
		string text = "VideoOS.Recorder.Service.WebApiPorts";
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Create firewall rule '{0}'", text));
		int portFromConfigFile = GetPortFromConfigFile();
		List<int> list = new List<int> { portFromConfigFile };
		try
		{
			FirewallHelper.CreateRule(text, list.ToArray());
		}
		catch (FirewallException ex)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Could not create firewall rule '{0}': {1}", text, ex.Message), TraceEventType.Warning);
		}
	}

	public static bool IsVersionWithOldEncryptionImplementation()
	{
		try
		{
			string s = InstallerManager.Instance.OldInstallInfo?.InstallEnvironment.Parameters.SingleOrDefault((KeyValueParameters<string, string> x) => x.Key == "VERSION_MAJOR")?.Value;
			string s2 = InstallerManager.Instance.OldInstallInfo?.InstallEnvironment.Parameters.SingleOrDefault((KeyValueParameters<string, string> x) => x.Key == "VERSION_MINOR")?.Value;
			if (!int.TryParse(s, NumberStyles.None, CultureInfo.InvariantCulture, out var result) || !int.TryParse(s2, NumberStyles.None, CultureInfo.InvariantCulture, out var result2))
			{
				LogManager.Write("IsVersionWithOldEncryptionImplementation. Unable to get existing version");
				return false;
			}
			if (new Version(result, result2) < new Version(20, 2))
			{
				return true;
			}
		}
		catch (Exception arg)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "IsVersionWithOldEncryptionImplementation. Exception occurred.\r\nException: {0}", arg));
			return false;
		}
		return false;
	}

	public static void CreateSecureFolderAndSetPermissions(Dictionary<string, string> installParameters, bool isFailover)
	{
		if (installParameters == null)
		{
			throw new ArgumentNullException("installParameters");
		}
		bool flag = installParameters["ACCOUNTTYPE"].Equals("true", StringComparison.OrdinalIgnoreCase);
		string accountName = (flag ? installParameters["ACCOUNTNAME"] : installParameters["BUILTINACCOUNTNAME"]);
		List<FileSystemRights> userAccountRights = new List<FileSystemRights>
		{
			FileSystemRights.DeleteSubdirectoriesAndFiles,
			FileSystemRights.Delete,
			FileSystemRights.Read,
			FileSystemRights.Write,
			FileSystemRights.Synchronize
		};
		List<FileSystemRights> builtInAdminRights = new List<FileSystemRights> { FileSystemRights.FullControl };
		if (!Directory.Exists(OemResources.Instance.Recorder.SecureDataFolder))
		{
			Directory.CreateDirectory(OemResources.Instance.Recorder.SecureDataFolder);
		}
		SetPermissionsToSecureFolder(OemResources.Instance.Recorder.SecureDataFolder, flag, accountName, userAccountRights, builtInAdminRights);
		if (isFailover)
		{
			if (!Directory.Exists(OemResources.Instance.FailoverServer.SecureDataFolder))
			{
				Directory.CreateDirectory(OemResources.Instance.FailoverServer.SecureDataFolder);
			}
			SetPermissionsToSecureFolder(OemResources.Instance.FailoverServer.SecureDataFolder, flag, accountName, userAccountRights, builtInAdminRights);
		}
	}

	private static void SetPermissionsToSecureFolder(string secureFolderPath, bool isUserAccount, string accountName, List<FileSystemRights> userAccountRights, List<FileSystemRights> builtInAdminRights)
	{
		new AccessControlUpdater(secureFolderPath).SetDirectoryAccessControl(isUserAccount, accountName, userAccountRights, builtInAdminRights, AccessControlUpdater.InheritanceOption.DisableInheritance);
	}
}
