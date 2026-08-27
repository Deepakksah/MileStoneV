using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using VideoOS.Common.Integration;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.ServerConfiguratorPlugin;

public static class ServerConfigFileUpdater
{
	private const string NetworkServerAccountName = "networkservice";

	public static void CreateUpdateConfigFile(Dictionary<ServerParameters, string> serverParameters)
	{
		string path = "ServerConfig.xml";
		string commonApplicationDataFolder = OemResources.Instance.Server.CommonApplicationDataFolder;
		string sourceFileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);
		string configFilePath = OemResources.Instance.Server.ConfigFilePath;
		LogManager.Write("CreateUpdateConfigFile(): Starting...");
		if (!File.Exists(configFilePath))
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "CreateUpdateConfigFile(): File {0} does not exist, creating new file ", configFilePath));
			try
			{
				if (!Directory.Exists(commonApplicationDataFolder))
				{
					Directory.CreateDirectory(commonApplicationDataFolder);
				}
				File.Copy(sourceFileName, configFilePath);
			}
			catch (Exception inner)
			{
				throw new ServerConfigFileUpdaterFailedException("CreateUpdateConfigFile(): File copy failed", inner);
			}
			LogManager.Write("CreateUpdateConfigFile(): File created, no need for update");
		}
		else
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "CreateUpdateConfigFile(): File {0} exists, starting ServerConfig.xml upgrade", configFilePath));
			try
			{
				string text = Path.Combine(commonApplicationDataFolder, "NewServerConfig.xml");
				string text2 = Path.Combine(commonApplicationDataFolder, "result.xml");
				File.Copy(sourceFileName, text);
				Rules[] array = new Rules[1]
				{
					new EmptyRules()
				};
				for (int i = 1; i < array.Length; i++)
				{
					Rules obj = array[i - 1];
					Rules successor = array[i];
					obj.SetSuccessor(successor);
				}
				Version version = Version.Parse(ConfigXmlUtilities.GetVersionOfConfigFile(configFilePath));
				if (version.CompareTo(new Version(11, 0)) < 0)
				{
					UpgradeClientTokenTimout(serverParameters[ServerParameters.DestinationDir], configFilePath);
				}
				if (version.CompareTo(new Version(13, 2)) < 0)
				{
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, "UpgradeXmlFile(): Upgrading from a version that requires remoting to communicate to the recording server. The support has been discontinued and the recording server should be upgraded to a newer version"));
				}
				Rules startPoint = array.Last();
				Rules[] array2 = array;
				foreach (Rules rules in array2)
				{
					if (version < rules.ConfigurationFileVersion)
					{
						startPoint = rules;
						break;
					}
				}
				XmlUpdater.UpdateXmlFiles(configFilePath, text, text2, startPoint);
				File.Delete(configFilePath);
				File.Delete(text);
				File.Move(text2, configFilePath);
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "UpgradeXmlFile(): File {0} upgraded!", configFilePath));
			}
			catch (Exception inner2)
			{
				throw new ServerConfigFileUpdaterFailedException("Exception occurred while upgrading", inner2);
			}
		}
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "UpgradeXmlFile(): Updating non upgradable parts of {0}", configFilePath));
		try
		{
			string version2 = OemResources.Instance.App.Version;
			ConfigXmlUtilities.UpdateConfigFileVersion(configFilePath, version2);
			bool flag = serverParameters[ServerParameters.UserAccount].Equals("true", StringComparison.OrdinalIgnoreCase);
			string accountValue = (flag ? serverParameters[ServerParameters.AccountName] : "networkservice");
			new AccessControlUpdater(configFilePath).SetFileAccessControl(flag, accountValue);
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "CreateUpdateConfigFile(): Updating non upgradable parts of {0} Done!", configFilePath));
		}
		catch (ConfigXmlParseVersionFailedException)
		{
			throw;
		}
		catch (Exception inner3)
		{
			throw new ServerConfigFileUpdaterFailedException("Exception occurred while updating non upgradable parts", inner3);
		}
	}

	private static void UpgradeClientTokenTimout(string installLocation, string installedConfigPath)
	{
		string clientTokenTimeout = ConfigXmlUtilities.GetClientTokenTimeout(installedConfigPath);
		UpdateManagementWebConfig(Path.Combine(installLocation, "IIS", "ManagementServer"), clientTokenTimeout);
	}

	private static void UpdateManagementWebConfig(string managementServerFolder, string ClientTokenTimoutMinutes)
	{
		string text = Path.Combine(managementServerFolder, "Web.config");
		XmlDocument xmlDocument = null;
		if ((File.GetAttributes(text) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
		{
			File.SetAttributes(text, FileAttributes.Normal);
		}
		xmlDocument = new XmlDocument();
		xmlDocument.Load(text);
		foreach (XmlNode item in xmlDocument.DocumentElement.SelectNodes("appSettings/add"))
		{
			if (item.Attributes.GetNamedItem("key").Value.Equals("ClientTokenTimoutMinutes", StringComparison.OrdinalIgnoreCase))
			{
				item.Attributes.GetNamedItem("value").Value = ClientTokenTimoutMinutes;
			}
		}
		xmlDocument.Save(text);
	}

	public static string GetPortFromConfigFile()
	{
		string result = "9000";
		string path = "ServerConfig.xml";
		string commonApplicationDataFolder = OemResources.Instance.Server.CommonApplicationDataFolder;
		string filename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);
		string text = Path.Combine(commonApplicationDataFolder, path);
		XmlDocument xmlDocument = new XmlDocument();
		if (File.Exists(text))
		{
			xmlDocument.Load(text);
		}
		else
		{
			xmlDocument.Load(filename);
		}
		XPathNavigator xPathNavigator = xmlDocument.CreateNavigator().CreateNavigator().SelectSingleNode("/server/WebApiConfig/Port");
		if (xPathNavigator != null)
		{
			result = xPathNavigator.Value;
		}
		return result;
	}
}
