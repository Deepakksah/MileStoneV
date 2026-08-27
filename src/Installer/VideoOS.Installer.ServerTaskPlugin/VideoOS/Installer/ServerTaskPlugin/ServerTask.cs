using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using Microsoft.Win32;
using VideoOS.Installer.Common;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.MSI;

namespace VideoOS.Installer.ServerTaskPlugin;

public class ServerTask : MSIInstallerTask, IInstallationAdapter
{
	private const string _systemPath = "software\\surveillancesystem";

	private const string _systemPathWow6432Node = "software\\Wow6432Node\\surveillancesystem";

	private string _configurationPath;

	public bool CheckPrequisite(string fileName, List<IPackageParameter> installParameters)
	{
		return false;
	}

	public void Initialize(List<IPackageParameter> installParameters)
	{
		using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("software\\surveillancesystem"))
		{
			if (registryKey != null)
			{
				_configurationPath = (string)registryKey.GetValue("ConfigurationPath");
				registryKey.Close();
			}
		}
		if (_configurationPath != null)
		{
			return;
		}
		using RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("software\\Wow6432Node\\surveillancesystem");
		if (registryKey2 != null)
		{
			_configurationPath = (string)registryKey2.GetValue("ConfigurationPath");
			registryKey2.Close();
		}
	}

	public void Finalize(List<IPackageParameter> installParameters)
	{
	}

	public void Install(string fileName, List<IPackageParameter> installParameters)
	{
		base.FileName = fileName;
		string text = "INSTALL:5F0DF842-CDD1-4382-8FE2-EF320E1C7D28";
		bool flag = false;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (IPackageParameter setting in GlobalData.Settings)
		{
			LogManager.Write($"Global: {setting.Target} = {setting.Value}", TraceEventType.Information);
			if (setting.Target == text)
			{
				flag = setting.Value == "yes";
				LogManager.Write("Event Server is enabled", TraceEventType.Information);
				break;
			}
		}
		foreach (IPackageParameter installParameter in installParameters)
		{
			dictionary.Add(installParameter.Target, installParameter.Value);
		}
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.OmitXmlDeclaration = true;
		xmlWriterSettings.ConformanceLevel = ConformanceLevel.Fragment;
		xmlWriterSettings.CloseOutput = false;
		StringBuilder stringBuilder = new StringBuilder();
		XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings);
		xmlWriter.WriteStartElement("settings");
		xmlWriter.WriteStartElement("services");
		xmlWriter.WriteStartElement("service");
		xmlWriter.WriteAttributeString("name", "Event Server");
		xmlWriter.WriteAttributeString("guid", "08AB8F23-9AEF-4298-9CAA-F39259FE7CDA");
		xmlWriter.WriteElementString("enabled", flag ? "yes" : "no");
		xmlWriter.WriteEndElement();
		xmlWriter.WriteEndElement();
		xmlWriter.WriteStartElement("partners");
		if (dictionary.ContainsKey("PARTNER_ID") || dictionary.ContainsKey("IDM_ID"))
		{
			xmlWriter.WriteStartElement("partner");
			xmlWriter.WriteAttributeString("id", dictionary["PARTNER_ID"]);
			xmlWriter.WriteAttributeString("idmid", dictionary["IDM_ID"]);
			xmlWriter.WriteEndElement();
		}
		xmlWriter.WriteEndElement();
		xmlWriter.WriteEndElement();
		xmlWriter.Flush();
		xmlWriter.Close();
		string text2 = stringBuilder.ToString();
		text2 = text2.Replace("\"", "\"\"");
		LogManager.Write($"INSTALLSETTINGS = {text2}", TraceEventType.Information);
		installParameters.Add(new PackageParameter("INSTALLSETTINGS", text2));
		if (_configurationPath != null)
		{
			installParameters.Add(new PackageParameter("CONFIGURATIONPATH", _configurationPath));
		}
		Init(base.TargetDir, installParameters);
	}

	public void Uninstall(string fileName, List<IPackageParameter> installParameters)
	{
	}
}
