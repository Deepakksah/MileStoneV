using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.XPath;
using Microsoft.Win32;
using VideoOS.Common.Integration;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Logging;
using VideoOS.Installer.RecorderPlugins;

namespace VideoOS.Installer.RecorderTaskPlugin;

public static class RecorderConfigFileUpdater
{
	public const string RecorderConfigFileName = "RecorderConfig.xml";

	public const string FailoverConfigFileName = "FailoverServerConfig.xml";

	public static void InitializeInstallerIds(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		parameters["NEWRECORDERGUID"] = Guid.NewGuid().ToString();
		parameters["NEWFAILOVERGUID"] = Guid.NewGuid().ToString();
	}

	public static void InitializeRegistryParameters(Dictionary<string, string> parameters, bool isFailover)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		if (string.IsNullOrEmpty(RegistryHelper.GetRegistryValue("SOFTWARE\\VideoOS\\Recorder\\Installation", "RecorderIDOnMachine", RegistryView.Registry32)))
		{
			RegistryHelper.SetRegistryValue("SOFTWARE\\VideoOS\\Recorder\\Installation", "RecorderIDOnMachine", parameters["NEWRECORDERGUID"], RegistryView.Registry32);
		}
		if (isFailover)
		{
			string registryValue = RegistryHelper.GetRegistryValue("SOFTWARE\\VideoOS\\FailoverServer\\Installation", "FailoverIDOnMachine", RegistryView.Registry32);
			parameters["OLDFAILOVERID"] = (string.IsNullOrEmpty(registryValue) ? "0" : registryValue);
			if (string.IsNullOrEmpty(registryValue))
			{
				RegistryHelper.SetRegistryValue("SOFTWARE\\VideoOS\\FailoverServer\\Installation", "FailoverIDOnMachine", parameters["NEWFAILOVERGUID"], RegistryView.Registry32);
			}
		}
	}

	public static void CopyEmbeddedResourceToFile(string resourceFileName, string destinationFileName)
	{
		string name = typeof(RecorderConfigFileUpdater).Namespace + "." + resourceFileName;
		using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
		using FileStream destination = new FileStream(destinationFileName, FileMode.Create, FileAccess.Write);
		stream.CopyTo(destination);
	}

	public static void UpgradeRecorderXmlFile(Dictionary<string, string> parameters, IOemResources oemResources)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		if (oemResources == null)
		{
			throw new ArgumentNullException("oemResources");
		}
		LogManager.Write(FormattableString.Invariant(FormattableStringFactory.Create("{0}(). Starting...", "UpgradeRecorderXmlFile")));
		string text = Path.Combine(oemResources.Recorder.CommonApplicationDataFolder, "RecorderConfig.xml");
		string oldRecorderId = "0";
		if (!File.Exists(text))
		{
			CreateNewRecorderConfigFile(text);
		}
		else
		{
			oldRecorderId = GetOldRecorderId(text);
			CreateNewRecorderConfigFileAndUpdateFromOldConfigFile(text);
		}
		UpdateRecorderConfigFromInstallerParameters(text, parameters, oldRecorderId, oemResources);
	}

	private static void CreateNewRecorderConfigFile(string recorderConfigFilePath)
	{
		LogManager.Write(FormattableString.Invariant(FormattableStringFactory.Create("{0}(). File {1} does not exist, creating new file.", "UpgradeRecorderXmlFile", recorderConfigFilePath)));
		try
		{
			string directoryName = Path.GetDirectoryName(recorderConfigFilePath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			CopyEmbeddedResourceToFile("RecorderConfig.xml", recorderConfigFilePath);
		}
		catch (Exception inner)
		{
			throw new RecorderConfigFileUpdaterFailException(FormattableString.Invariant(FormattableStringFactory.Create("{0}(). File copy failed", "UpgradeRecorderXmlFile")), inner);
		}
	}

	private static void CreateNewRecorderConfigFileAndUpdateFromOldConfigFile(string recorderConfigFilePath)
	{
		LogManager.Write(FormattableString.Invariant(FormattableStringFactory.Create("{0}(). File {1} exists, starting RecorderConfig.xml upgrade", "UpgradeRecorderXmlFile", recorderConfigFilePath)));
		try
		{
			string directoryName = Path.GetDirectoryName(recorderConfigFilePath);
			string text = Path.Combine(directoryName, "NewRecorderConfig.xml");
			string text2 = Path.Combine(directoryName, "result.xml");
			CopyEmbeddedResourceToFile("RecorderConfig.xml", text);
			Rules[] array = new Rules[6]
			{
				new RecorderRulesInitial(),
				new RecorderRules60(),
				new RecorderRules61(),
				new RecorderRules102(),
				new RecorderRules233(),
				new EmptyRules()
			};
			for (int i = 1; i < array.Length; i++)
			{
				Rules obj = array[i - 1];
				Rules successor = array[i];
				obj.SetSuccessor(successor);
			}
			Version version = Version.Parse(ConfigXmlUtilities.GetVersionOfConfigFile(recorderConfigFilePath));
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
			XmlUpdater.UpdateXmlFiles(recorderConfigFilePath, text, text2, startPoint);
			File.Delete(recorderConfigFilePath);
			File.Delete(text);
			File.Move(text2, recorderConfigFilePath);
			LogManager.Write(FormattableString.Invariant(FormattableStringFactory.Create("{0}(). File {1} upgraded!", "UpgradeRecorderXmlFile", recorderConfigFilePath)));
		}
		catch (Exception inner)
		{
			throw new RecorderConfigFileUpdaterFailException("Exception occurred while upgrading", inner);
		}
	}

	private static void UpdateRecorderConfigFromInstallerParameters(string recorderConfigFilePath, Dictionary<string, string> parameters, string oldRecorderId, IOemResources oemResources)
	{
		try
		{
			LogManager.Write(FormattableString.Invariant(FormattableStringFactory.Create("{0}(). Updating non upgradable parts of {1}", "UpgradeRecorderXmlFile", recorderConfigFilePath)));
			string version = oemResources.App.Version;
			ConfigXmlUtilities.UpdateConfigFileVersion(recorderConfigFilePath, version);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(recorderConfigFilePath);
			XPathNavigator xPathNavigator = xmlDocument.CreateNavigator().CreateNavigator();
			XPathNavigator xPathNavigator2 = xPathNavigator.SelectSingleNode("/recorderconfig/recorder/version");
			if (xPathNavigator2 == null)
			{
				XmlNode xmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "version", null);
				xmlNode.InnerXml = version;
				xmlDocument.DocumentElement["recorder"].AppendChild(xmlNode);
			}
			else
			{
				xPathNavigator2.InnerXml = version;
			}
			xPathNavigator2 = xPathNavigator.SelectSingleNode("/recorderconfig/recorder/driverspath");
			if (xPathNavigator2 != null)
			{
				xPathNavigator2.InnerXml = "./Drivers";
			}
			xPathNavigator2 = xPathNavigator.SelectSingleNode("/recorderconfig/recorder/mipdriverspath");
			if (xPathNavigator2 != null)
			{
				xPathNavigator2.InnerXml = "../MIPDrivers";
			}
			xPathNavigator2 = xPathNavigator.SelectSingleNode("/recorderconfig/recorder/pluginspath");
			if (xPathNavigator2 != null)
			{
				xPathNavigator2.InnerXml = "./Plugins";
			}
			xPathNavigator2 = xPathNavigator.SelectSingleNode("/recorderconfig/recorder/ruleactionspath");
			if (xPathNavigator2 != null)
			{
				xPathNavigator2.InnerXml = "./RuleActions";
			}
			xPathNavigator2 = xPathNavigator.SelectSingleNode("/recorderconfig/recorder/displayname");
			if (xPathNavigator2 != null)
			{
				xPathNavigator2.InnerXml = parameters["RECORDERNAME"];
			}
			xPathNavigator2 = xPathNavigator.SelectSingleNode("/recorderconfig/database/database_default/path");
			if (xPathNavigator2 != null)
			{
				xPathNavigator2.InnerXml = parameters["MEDIADBPATH"];
			}
			xPathNavigator2 = xPathNavigator.SelectSingleNode("/recorderconfig/failoverserver/installedasfailoverserver");
			if (xPathNavigator2 != null)
			{
				xPathNavigator2.InnerXml = parameters["ISFAILOVER"];
			}
			else
			{
				xPathNavigator2 = xPathNavigator.SelectSingleNode("/recorderconfig/failoverserver");
				if (xPathNavigator2 != null)
				{
					XmlNode xmlNode2 = xmlDocument.DocumentElement.SelectSingleNode("failoverserver");
					XmlElement xmlElement = xmlDocument.CreateElement("installedasfailoverserver");
					xmlElement.InnerXml = parameters["ISFAILOVER"];
					xmlNode2.AppendChild(xmlElement);
				}
			}
			string text = parameters["NEWRECORDERGUID"];
			LogManager.Write(FormattableString.Invariant(FormattableStringFactory.Create("{0}(). New recorder id {1} ", "UpgradeRecorderXmlFile", text)));
			if (!oldRecorderId.Equals("0", StringComparison.OrdinalIgnoreCase))
			{
				xPathNavigator2 = xPathNavigator.SelectSingleNode("/recorderconfig/recorder/id");
				xPathNavigator2.InnerXml = oldRecorderId;
				text = oldRecorderId;
				LogManager.Write(FormattableString.Invariant(FormattableStringFactory.Create("{0}(). Old recorder id {1} ", "UpgradeRecorderXmlFile", text)));
			}
			else
			{
				xPathNavigator2 = xPathNavigator.SelectSingleNode("/recorderconfig/recorder/id");
				xPathNavigator2.InnerXml = parameters["NEWRECORDERGUID"];
			}
			parameters["RECORDERGUID"] = text;
			xPathNavigator2 = xPathNavigator.SelectSingleNode("/recorderconfig/webapi/publicUri");
			if (xPathNavigator2 != null && (string.IsNullOrEmpty(xPathNavigator2.InnerXml) || xPathNavigator2.InnerXml.StartsWith("localhost", StringComparison.OrdinalIgnoreCase)))
			{
				xPathNavigator2.InnerXml = FormattableString.Invariant(FormattableStringFactory.Create("{0}:{1}", Dns.GetHostEntry("").HostName, RecorderConfiguration.GetPortFromConfigFile()));
			}
			xmlDocument.Save(recorderConfigFilePath);
			LogManager.Write(FormattableString.Invariant(FormattableStringFactory.Create("{0}(). Updating non upgradable parts of {1} Done!", "UpgradeRecorderXmlFile", recorderConfigFilePath)));
			bool flag = parameters["ACCOUNTTYPE"].Equals("true", StringComparison.OrdinalIgnoreCase);
			string accountValue = (flag ? parameters["ACCOUNTNAME"] : parameters["BUILTINACCOUNTNAME"]);
			new AccessControlUpdater(recorderConfigFilePath).SetFileAccessControl(flag, accountValue);
		}
		catch (ConfigXmlParseVersionFailedException)
		{
			throw;
		}
		catch (Exception inner)
		{
			throw new RecorderConfigFileUpdaterFailException(FormattableString.Invariant(FormattableStringFactory.Create("{0}(). Failed, exception occurred while updating non upgradable parts", "UpgradeRecorderXmlFile")), inner);
		}
	}

	private static string GetOldRecorderId(string pathToConfigFile)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(pathToConfigFile);
		XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode("/recorderconfig/recorder/id");
		if (xmlNode != null)
		{
			return xmlNode.InnerText;
		}
		return "0";
	}

	public static void UpgradeFailoverRecorderXmlFile(Dictionary<string, string> parameters, IOemResources oemResources)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		if (oemResources == null)
		{
			throw new ArgumentNullException("oemResources");
		}
		LogManager.Write(FormattableString.Invariant(FormattableStringFactory.Create("{0}(): Starting...", "UpgradeFailoverRecorderXmlFile")));
		string commonApplicationDataFolder = oemResources.FailoverServer.CommonApplicationDataFolder;
		string text = Path.Combine(commonApplicationDataFolder, "FailoverServerConfig.xml");
		string text2 = "0";
		if (!File.Exists(text))
		{
			LogManager.Write(FormattableString.Invariant(FormattableStringFactory.Create("{0}(): File {1} does not exist, creating new file.", "UpgradeFailoverRecorderXmlFile", text)));
			try
			{
				if (!Directory.Exists(commonApplicationDataFolder))
				{
					Directory.CreateDirectory(commonApplicationDataFolder);
				}
				CopyEmbeddedResourceToFile("FailoverServerConfig.xml", text);
			}
			catch (Exception inner)
			{
				throw new RecorderConfigFileUpdaterFailException(FormattableString.Invariant(FormattableStringFactory.Create("{0}(): File copy failed", "UpgradeFailoverRecorderXmlFile")), inner);
			}
		}
		else
		{
			LogManager.Write(FormattableString.Invariant(FormattableStringFactory.Create("{0}(): File {1} exists, starting FailoverServerConfig.xml upgrade", "UpgradeFailoverRecorderXmlFile", text)));
			try
			{
				string text3 = Path.Combine(commonApplicationDataFolder, "NewFailoverServerConfig.xml");
				string text4 = Path.Combine(commonApplicationDataFolder, "result.xml");
				CopyEmbeddedResourceToFile("FailoverServerConfig.xml", text3);
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
				Version version = Version.Parse(ConfigXmlUtilities.GetVersionOfConfigFile(text));
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
				XmlUpdater.UpdateXmlFiles(text, text3, text4, startPoint);
				text2 = GetOldFailoverRecorderID(text);
				File.Delete(text);
				File.Delete(text3);
				File.Move(text4, text);
				LogManager.Write(FormattableString.Invariant(FormattableStringFactory.Create("{0}(): File {1} upgraded!", "UpgradeFailoverRecorderXmlFile", text)));
			}
			catch (Exception inner2)
			{
				throw new RecorderConfigFileUpdaterFailException("Exception occurred while upgrading failover recorder xml.", inner2);
			}
		}
		try
		{
			LogManager.Write(FormattableString.Invariant(FormattableStringFactory.Create("{0}(): Updating non upgradable parts of {1}", "UpgradeFailoverRecorderXmlFile", text)));
			string version2 = oemResources.App.Version;
			ConfigXmlUtilities.UpdateConfigFileVersion(text, version2);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(text);
			XPathNavigator xPathNavigator = xmlDocument.CreateNavigator().CreateNavigator();
			XPathNavigator xPathNavigator2 = xPathNavigator.SelectSingleNode("/recorderfailoverconfig/failoverservice/displayname");
			if (xPathNavigator2 != null)
			{
				xPathNavigator2.InnerXml = parameters["RECORDERNAME"];
			}
			Uri uri = new Uri(parameters["MANAGEMENTSERVERURI"]);
			xPathNavigator2 = xPathNavigator.SelectSingleNode("/recorderfailoverconfig/server/port");
			if (xPathNavigator2 != null)
			{
				xPathNavigator2.InnerXml = uri.Port.ToString(CultureInfo.InvariantCulture);
			}
			if (!parameters["OLDFAILOVERID"].Equals("0", StringComparison.OrdinalIgnoreCase))
			{
				xPathNavigator2 = xPathNavigator.SelectSingleNode("/recorderfailoverconfig/failoverservice/guid");
				if (xPathNavigator2 != null)
				{
					xPathNavigator2.InnerXml = parameters["OLDFAILOVERID"];
				}
			}
			else
			{
				xPathNavigator2 = xPathNavigator.SelectSingleNode("/recorderfailoverconfig/failoverservice/guid");
				if (xPathNavigator2 != null)
				{
					xPathNavigator2.InnerXml = parameters["NEWFAILOVERGUID"];
				}
			}
			if (!text2.Equals("0", StringComparison.OrdinalIgnoreCase))
			{
				xPathNavigator2 = xPathNavigator.SelectSingleNode("/recorderfailoverconfig/failoverservice/myrecorderguid");
				if (xPathNavigator2 != null)
				{
					xPathNavigator2.InnerXml = text2;
				}
			}
			else
			{
				xPathNavigator2 = xPathNavigator.SelectSingleNode("/recorderfailoverconfig/failoverservice/myrecorderguid");
				if (xPathNavigator2 != null)
				{
					xPathNavigator2.InnerXml = parameters["NEWRECORDERGUID"];
				}
			}
			xmlDocument.Save(text);
			LogManager.Write(FormattableString.Invariant(FormattableStringFactory.Create("{0}(): Updating non upgradable parts of {1} Done!", "UpgradeFailoverRecorderXmlFile", text)));
			bool userAccount = true;
			string accountValue = parameters["ACCOUNTNAME"];
			new AccessControlUpdater(text).SetFileAccessControl(userAccount, accountValue);
		}
		catch (Exception inner3)
		{
			throw new RecorderConfigFileUpdaterFailException(FormattableString.Invariant(FormattableStringFactory.Create("{0}() failed, exception occurred while updating non upgradable", "UpgradeFailoverRecorderXmlFile")), inner3);
		}
	}

	private static string GetOldFailoverRecorderID(string pathToConfigFile)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(pathToConfigFile);
		XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode("/recorderfailoverconfig/failoverservice/myrecorderguid");
		if (xmlNode != null)
		{
			return xmlNode.InnerText;
		}
		return "0";
	}
}
