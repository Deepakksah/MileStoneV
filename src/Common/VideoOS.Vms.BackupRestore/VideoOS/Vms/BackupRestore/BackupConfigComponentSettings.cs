using System;
using System.IO;
using System.Xml;

namespace VideoOS.Vms.BackupRestore;

public class BackupConfigComponentSettings
{
	private const string ComponentSettingsXmlRootName = "ComponentSettings";

	public string ComponentName { get; set; }

	public string ConfigData { get; set; }

	public BackupConfigComponentSettings(string componentName, string configData)
	{
		ComponentName = componentName;
		ConfigData = configData;
	}

	public static void WriteComponentSettingsAsXml(XmlWriter xmlWriter, BackupConfigComponentSettings componentSettings)
	{
		if (xmlWriter == null)
		{
			throw new ArgumentNullException("xmlWriter");
		}
		if (componentSettings == null)
		{
			throw new ArgumentNullException("componentSettings");
		}
		xmlWriter.WriteStartElement("ComponentSettings");
		xmlWriter.WriteAttributeString("ComponentName", componentSettings.ComponentName);
		xmlWriter.WriteStartElement("ConfigData");
		xmlWriter.WriteString(componentSettings.ConfigData);
		xmlWriter.WriteEndElement();
		xmlWriter.WriteEndElement();
	}

	public static BackupConfigComponentSettings ReadComponentSettingsFromXml(string xml)
	{
		if (xml == null)
		{
			throw new ArgumentNullException("xml");
		}
		BackupConfigComponentSettings backupConfigComponentSettings = new BackupConfigComponentSettings(null, null);
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
			if (!string.Equals(xmlReader.Name, "ComponentSettings", StringComparison.Ordinal))
			{
				return null;
			}
			xmlReader.MoveToFirstAttribute();
			do
			{
				if (xmlReader.Name == "ComponentName")
				{
					backupConfigComponentSettings.ComponentName = xmlReader.Value;
					continue;
				}
				throw new NotSupportedException("Invalid attribute to the 'ComponentSettings' element: '" + xmlReader.Name + "'.");
			}
			while (xmlReader.MoveToNextAttribute());
			xmlReader.MoveToElement();
			if (xmlReader.Read() && string.Equals(xmlReader.Name, "ConfigData", StringComparison.Ordinal))
			{
				backupConfigComponentSettings.ConfigData = xmlReader.ReadString();
			}
		}
		return backupConfigComponentSettings;
	}
}
