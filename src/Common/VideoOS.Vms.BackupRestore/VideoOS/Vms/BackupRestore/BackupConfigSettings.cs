using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.Vms.BackupRestore;

public class BackupConfigSettings
{
	private const string BackupConfigSettingsXmlRootName = "CorporateBackupSettings";

	private const string BackupTimeFormat = "yyyyMMddHHmmss";

	public string ComputerName { get; set; }

	public DateTime BackupTime { get; set; }

	public IList<RegisteredService> RegisteredServices { get; private set; }

	public IDictionary<string, BackupConfigComponentSettings> ComponentSettings { get; private set; }

	public BackupConfigSettings(string computerName, IList<RegisteredService> registeredServices, IDictionary<string, BackupConfigComponentSettings> componentSettings)
	{
		ComputerName = computerName;
		RegisteredServices = registeredServices;
		BackupTime = DateTime.UtcNow;
		if (componentSettings != null)
		{
			ComponentSettings = componentSettings;
		}
		else
		{
			ComponentSettings = new Dictionary<string, BackupConfigComponentSettings>();
		}
	}

	public static void WriteCorporateBackupSettingsAsXml(XmlWriter xmlWriter, BackupConfigSettings backupSettings)
	{
		if (xmlWriter == null)
		{
			throw new ArgumentNullException("xmlWriter");
		}
		if (backupSettings == null)
		{
			throw new ArgumentNullException("backupSettings");
		}
		xmlWriter.WriteStartElement("CorporateBackupSettings");
		xmlWriter.WriteAttributeString("ComputerName", backupSettings.ComputerName);
		xmlWriter.WriteAttributeString("BackupTime", backupSettings.BackupTime.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture));
		if (backupSettings.RegisteredServices != null)
		{
			xmlWriter.WriteStartElement("RegisteredServices");
			foreach (RegisteredService registeredService in backupSettings.RegisteredServices)
			{
				RegisteredService.WriteRegisteredServiceAsXml(xmlWriter, registeredService);
			}
			xmlWriter.WriteEndElement();
		}
		foreach (BackupConfigComponentSettings value in backupSettings.ComponentSettings.Values)
		{
			BackupConfigComponentSettings.WriteComponentSettingsAsXml(xmlWriter, value);
		}
		xmlWriter.WriteEndElement();
	}

	public static BackupConfigSettings ReadBackupConfigSettingsFromXml(string xml)
	{
		if (xml == null)
		{
			throw new ArgumentNullException("xml");
		}
		string computerName = null;
		DateTime backupTime = DateTime.MinValue;
		IList<RegisteredService> list = null;
		IDictionary<string, BackupConfigComponentSettings> dictionary = new Dictionary<string, BackupConfigComponentSettings>();
		new XmlDocument().LoadXml(xml);
		using (StringReader input = new StringReader(xml))
		{
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.IgnoreComments = true;
			xmlReaderSettings.IgnoreProcessingInstructions = true;
			xmlReaderSettings.IgnoreWhitespace = true;
			xmlReaderSettings.XmlResolver = null;
			xmlReaderSettings.ValidationType = ValidationType.Schema;
			using XmlReader xmlReader = XmlReader.Create(input, xmlReaderSettings);
			xmlReader.MoveToContent();
			if (!string.Equals(xmlReader.Name, "CorporateBackupSettings", StringComparison.Ordinal))
			{
				return null;
			}
			xmlReader.MoveToFirstAttribute();
			do
			{
				string name = xmlReader.Name;
				if (!(name == "ComputerName"))
				{
					if (!(name == "BackupTime"))
					{
						throw new NotSupportedException("Invalid attribute to the 'CorporateBackupSettings' element: '" + xmlReader.Name + "'.");
					}
					backupTime = XmlConvert.ToDateTime(xmlReader.Value, "yyyyMMddHHmmss");
				}
				else
				{
					computerName = xmlReader.Value;
				}
			}
			while (xmlReader.MoveToNextAttribute());
			xmlReader.MoveToElement();
			if (xmlReader.Read())
			{
				if (string.Equals(xmlReader.Name, "RegisteredServices", StringComparison.Ordinal))
				{
					list = new List<RegisteredService>();
					xmlReader.Read();
					while (string.Equals(xmlReader.Name, "RegisteredService", StringComparison.Ordinal))
					{
						RegisteredService item = RegisteredService.ReadRegisteredServiceFromXml(xmlReader.ReadOuterXml());
						list.Add(item);
					}
					if (!string.Equals(xmlReader.Name, "ComponentSettings", StringComparison.Ordinal))
					{
						xmlReader.Read();
					}
				}
				while (string.Equals(xmlReader.Name, "ComponentSettings", StringComparison.Ordinal))
				{
					BackupConfigComponentSettings backupConfigComponentSettings = BackupConfigComponentSettings.ReadComponentSettingsFromXml(xmlReader.ReadOuterXml());
					dictionary.Add(backupConfigComponentSettings.ComponentName, backupConfigComponentSettings);
				}
			}
		}
		return new BackupConfigSettings(computerName, list, dictionary)
		{
			BackupTime = backupTime
		};
	}

	public static void WriteToFile(BackupConfigSettings backupConfigSettings, string fileName)
	{
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.OmitXmlDeclaration = false;
		xmlWriterSettings.Encoding = Encoding.Unicode;
		xmlWriterSettings.Indent = true;
		using XmlWriter xmlWriter = XmlWriter.Create(fileName, xmlWriterSettings);
		WriteCorporateBackupSettingsAsXml(xmlWriter, backupConfigSettings);
		xmlWriter.Flush();
		xmlWriter.Close();
	}

	public static BackupConfigSettings ReadFromFile(string fileName)
	{
		return ReadBackupConfigSettingsFromXml(File.ReadAllText(fileName, Encoding.Unicode));
	}
}
