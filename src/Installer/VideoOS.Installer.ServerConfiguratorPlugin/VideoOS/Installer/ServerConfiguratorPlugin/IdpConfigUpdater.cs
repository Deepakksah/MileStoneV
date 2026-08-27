using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Newtonsoft.Json;
using VideoOS.Common.Integration;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;

namespace VideoOS.Installer.ServerConfiguratorPlugin;

public class IdpConfigUpdater
{
	private const string AppSettingsFile = "appsettings.json";

	public static void EnsureOldIdpAppSettingsValues(string installDir)
	{
		string text = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "appsettings.json");
		if (!File.Exists(text))
		{
			throw new ServerConfiguratorFailedException(FormattableString.Invariant($"Could not find the required file {text}!"));
		}
		string text2 = Path.Combine(installDir, "IIS\\IDP", "appsettings.json");
		if (!File.Exists(text2))
		{
			File.Copy(text, text2);
		}
		else
		{
			AppSettingsHelper.EnsureOldAppSettingsValues(text2, text, "appsettings.json");
		}
	}

	public static void ConfigureIdpSettings(Dictionary<ServerParameters, string> serverParameters, Uri newIdpAddress)
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
		folderPath = Path.Combine(folderPath, OemResources.Instance.App.CompanyName);
		folderPath = Path.Combine(folderPath, "IDP\\Logs");
		UpdateIdentityServerConfig(Path.Combine(serverParameters[ServerParameters.IisFolder], "IDP"), folderPath);
		UpdateAppSettingsIdentityProvider(newIdpAddress, serverParameters[ServerParameters.DestinationDir]);
	}

	private static void UpdateIdentityServerConfig(string identityServerFolder, string logPath)
	{
		string filename = Path.Combine(identityServerFolder, "NLog.config");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(filename);
		XmlNode attributeNode = xmlDocument.DocumentElement.GetAttributeNode("internalLogFile");
		if (attributeNode != null)
		{
			attributeNode.Value = logPath;
		}
		XmlNode xmlNode = (from XmlNode a in xmlDocument.GetElementsByTagName("variable")
			where a.Attributes["name"].Value.ToString() == "logDirectory"
			select a).First();
		if (xmlNode != null)
		{
			xmlNode.Attributes["value"].Value = logPath;
		}
		xmlDocument.Save(filename);
	}

	private static void UpdateAppSettingsIdentityProvider(Uri address, string destinationDir)
	{
		string text = Path.Combine(destinationDir, "IIS\\IDP", "appsettings.json");
		string value = string.Empty;
		GlobalData.TryGetValue("AUTHORITY", out value);
		if (File.Exists(text))
		{
			try
			{
				dynamic val = JsonConvert.DeserializeObject(File.ReadAllText(text));
				string text2 = (string.IsNullOrEmpty(value) ? val["IdentityProviderSettings"]["Authority"] : value);
				if (text2 != null)
				{
					val["IdentityProviderSettings"]["Authority"] = IdpUriParser.FormatAuthority(address, text2.ToString());
				}
				else
				{
					val["IdentityProviderSettings"]["Authority"] = IdpUriParser.FormatAuthority(address);
				}
				string contents = JsonConvert.SerializeObject(val, (Formatting)1);
				File.WriteAllText(text, contents);
				return;
			}
			catch (Exception inner)
			{
				throw new ServerConfiguratorFailedException("Updating Authorization Server config failed", inner);
			}
		}
		throw new ServerConfiguratorFailedException(string.Format(CultureInfo.InvariantCulture, "The configuration file of the Authorization Server service could not be found {0}", text));
	}
}
