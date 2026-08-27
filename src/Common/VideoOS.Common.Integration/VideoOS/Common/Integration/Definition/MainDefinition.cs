using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public class MainDefinition
{
	private definition _definition;

	private DriverDefinition _driverDefinition;

	private HardwareDefinition _hardwareDefinition;

	private EventDefinitionCollection events;

	private XPathDocument _definitionXpathDoc;

	public DriverDefinition Driver
	{
		get
		{
			if (_driverDefinition == null)
			{
				_driverDefinition = new DriverDefinition(this);
			}
			return _driverDefinition;
		}
	}

	public HardwareDefinition HardwareDefinition
	{
		get
		{
			if (_hardwareDefinition == null)
			{
				_hardwareDefinition = new HardwareDefinition(this);
			}
			return _hardwareDefinition;
		}
	}

	public XPathDocument DefinitionAsXPathDoc
	{
		get
		{
			if (_definitionXpathDoc == null)
			{
				using StringReader textReader = new StringReader(SaveAsXml());
				_definitionXpathDoc = new XPathDocument(textReader);
			}
			return _definitionXpathDoc;
		}
	}

	public definition Base
	{
		[DebuggerStepThrough]
		get
		{
			return _definition;
		}
	}

	public EventDefinitionCollection Events
	{
		get
		{
			if (events != null)
			{
				return events;
			}
			lock (this)
			{
				if (events != null)
				{
					return events;
				}
				EventDefinitionCollection eventDefinitionCollection = new EventDefinitionCollection();
				if (_definition.hardware.events?.@event != null)
				{
					eventType[] array = _definition.hardware.events.@event;
					foreach (eventType eventType2 in array)
					{
						eventDefinitionCollection.Add(new EventDefinition(this, eventType2));
					}
				}
				events = eventDefinitionCollection;
				return events;
			}
		}
	}

	private MainDefinition(definition definition)
	{
		_definition = definition;
	}

	public void DeleteCache()
	{
		_definitionXpathDoc = null;
	}

	private static string[] GetSettingsFragments(string settingsXml)
	{
		List<string> list = new List<string>();
		XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
		xmlReaderSettings.IgnoreComments = true;
		xmlReaderSettings.IgnoreProcessingInstructions = true;
		xmlReaderSettings.IgnoreWhitespace = true;
		if (settingsXml != null)
		{
			using XmlReader xmlReader = XmlReader.Create(new StringReader(settingsXml), xmlReaderSettings);
			if (xmlReader.IsStartElement() && (xmlReader.Name == "properties" || xmlReader.ReadToFollowing("properties")) && xmlReader.ReadToDescendant("settings"))
			{
				do
				{
					list.Add(xmlReader.ReadOuterXml());
				}
				while (xmlReader.Name == "settings");
			}
		}
		return list.ToArray();
	}

	public static string MergeSettings(IEnumerable<string> settingsXml)
	{
		List<string> list = new List<string>();
		StringBuilder stringBuilder = new StringBuilder();
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.OmitXmlDeclaration = true;
		if (settingsXml != null)
		{
			foreach (string item in settingsXml)
			{
				list.AddRange(GetSettingsFragments(item));
			}
		}
		using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings))
		{
			xmlWriter.WriteStartElement(SectionSettings.XmlRootName);
			foreach (string item2 in list)
			{
				xmlWriter.WriteRaw(item2);
			}
			xmlWriter.WriteEndElement();
			xmlWriter.Close();
		}
		return stringBuilder.ToString();
	}

	private static MainDefinition LoadXml(string xml)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(definition));
		StringReader textReader = new StringReader(xml);
		return new MainDefinition((definition)xmlSerializer.Deserialize(textReader));
	}

	public static MainDefinition LoadXml(string definitionXml, IEnumerable<string> settingsXmlList)
	{
		if (settingsXmlList == null)
		{
			return LoadXml(definitionXml);
		}
		string xml = MergeSettings(settingsXmlList);
		return LoadXml(UpdateMainDefinitionSettings(definitionXml, new DriverSettings(xml)));
	}

	public static MainDefinition LoadXml(string definitionXml, params string[] settingsXml)
	{
		if (settingsXml == null)
		{
			return LoadXml(definitionXml);
		}
		return LoadXml(definitionXml, new List<string>(settingsXml));
	}

	public static MainDefinition UpdateSettings(MainDefinition mainDefinition, params string[] settingsXml)
	{
		if (mainDefinition == null)
		{
			throw new ArgumentNullException("mainDefinition");
		}
		if (settingsXml == null)
		{
			return LoadXml(mainDefinition.SaveAsXml());
		}
		return LoadXml(mainDefinition.SaveAsXml(), new List<string>(settingsXml));
	}

	private static string UpdateMainDefinitionSettings(string definitionXml, DriverSettings settings)
	{
		if (definitionXml == null)
		{
			throw new ArgumentNullException("definitionXml");
		}
		StringBuilder stringBuilder = new StringBuilder(definitionXml.Length);
		XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
		xmlReaderSettings.IgnoreComments = true;
		xmlReaderSettings.IgnoreProcessingInstructions = true;
		xmlReaderSettings.IgnoreWhitespace = true;
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.OmitXmlDeclaration = true;
		using (XmlReader xmlReader = XmlReader.Create(new StringReader(definitionXml), xmlReaderSettings))
		{
			using XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings);
			if (!xmlReader.IsStartElement())
			{
				throw new ArgumentException("Invalid definition xml", "definitionXml");
			}
			xmlWriter.WriteStartElement(xmlReader.Name);
			if (xmlReader.HasAttributes)
			{
				xmlWriter.WriteAttributes(xmlReader, defattr: false);
			}
			xmlReader.ReadStartElement();
			while (xmlReader.NodeType == XmlNodeType.Element)
			{
				if (string.Equals(xmlReader.Name, SectionSettings.XmlRootName, StringComparison.OrdinalIgnoreCase))
				{
					WriteProperties(xmlReader, xmlWriter, settings);
				}
				else
				{
					xmlWriter.WriteNode(xmlReader, defattr: false);
				}
			}
			xmlWriter.WriteEndElement();
			xmlWriter.Close();
		}
		return stringBuilder.ToString();
	}

	private static void WriteProperties(XmlReader reader, XmlWriter writer, DriverSettings settings)
	{
		string xml = reader.ReadOuterXml();
		DriverSettings driverSettings = new DriverSettings(xml);
		driverSettings.SettingAttributeText = string.Empty;
		string[] sectionNames = settings.GetSectionNames();
		foreach (string section in sectionNames)
		{
			string[] sectionSettingNames = settings.GetSectionSettingNames(section);
			foreach (string name in sectionSettingNames)
			{
				string value = settings.Get(section, name);
				driverSettings.CreateOrUpdate(section, name, value, string.Empty);
			}
		}
		xml = driverSettings.SaveAsXml();
		writer.WriteRaw(xml);
	}

	public static MainDefinition LoadFromFile(string filename)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(definition));
		FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
		definition obj = (definition)xmlSerializer.Deserialize(fileStream);
		fileStream.Close();
		return new MainDefinition(obj);
	}

	public string SaveAsXml()
	{
		lock (this)
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(definition));
			MemoryStream memoryStream = new MemoryStream();
			xmlSerializer.Serialize(memoryStream, _definition);
			Thread.CurrentThread.CurrentCulture = currentCulture;
			return Encoding.UTF8.GetString(memoryStream.ToArray());
		}
	}

	public MainDefinition Clone()
	{
		return LoadXml(SaveAsXml());
	}

	public void UpdateImplementationDetailPathNames(string driversDirectoryPath, string basePath)
	{
		HardwareDefinition.Base.implementation.assemblyfilename = ChangeFilePath(HardwareDefinition.Base.implementation.assemblyfilename, driversDirectoryPath);
		HardwareDefinition.Base.implementation.tag = ChangeFilePath(HardwareDefinition.Base.implementation.tag, basePath);
		if (HardwareDefinition.Base.autodetect != null && HardwareDefinition.Base.autodetect.implementation != null)
		{
			HardwareDefinition.Base.autodetect.implementation.assemblyfilename = ChangeFilePath(HardwareDefinition.Base.autodetect.implementation.assemblyfilename, driversDirectoryPath);
			HardwareDefinition.Base.autodetect.implementation.tag = ChangeFilePath(HardwareDefinition.Base.autodetect.implementation.tag, basePath);
		}
		if (HardwareDefinition.Base.eventparsers != null)
		{
			eventparserType[] eventparsers = HardwareDefinition.Base.eventparsers;
			foreach (eventparserType eventparserType2 in eventparsers)
			{
				eventparserType2.implementation.assemblyfilename = ChangeFilePath(eventparserType2.implementation.assemblyfilename, driversDirectoryPath);
				eventparserType2.implementation.tag = ChangeFilePath(eventparserType2.implementation.tag, basePath);
			}
		}
		foreach (DeviceDefinition deviceDefinition in HardwareDefinition.DeviceDefinitions)
		{
			deviceDefinition.Base.implementation.assemblyfilename = ChangeFilePath(deviceDefinition.Base.implementation.assemblyfilename, driversDirectoryPath);
			deviceDefinition.Base.implementation.tag = ChangeFilePath(deviceDefinition.Base.implementation.tag, basePath);
		}
	}

	private string ChangeFilePath(string origFilePath, string newDirectoryPath)
	{
		if (string.IsNullOrEmpty(origFilePath))
		{
			return string.Empty;
		}
		if (newDirectoryPath == null)
		{
			return Path.GetFileName(origFilePath);
		}
		return Path.Combine(newDirectoryPath, Path.GetFileName(origFilePath));
	}

	public void UpdateImplementationDetailPathNamesToAbsolute(string driversDirectoryPath, string baseRelativePath)
	{
		HardwareDefinition.Base.implementation.assemblyfilename = ChangeFilePath(HardwareDefinition.Base.implementation.assemblyfilename, driversDirectoryPath);
		HardwareDefinition.Base.implementation.tag = GetAbsolutePath(HardwareDefinition.Base.implementation.tag, baseRelativePath);
		if (HardwareDefinition.Base.autodetect != null && HardwareDefinition.Base.autodetect.implementation != null)
		{
			HardwareDefinition.Base.autodetect.implementation.assemblyfilename = ChangeFilePath(HardwareDefinition.Base.autodetect.implementation.assemblyfilename, driversDirectoryPath);
			HardwareDefinition.Base.autodetect.implementation.tag = GetAbsolutePath(HardwareDefinition.Base.autodetect.implementation.tag, baseRelativePath);
		}
		if (HardwareDefinition.Base.eventparsers != null)
		{
			eventparserType[] eventparsers = HardwareDefinition.Base.eventparsers;
			foreach (eventparserType eventparserType2 in eventparsers)
			{
				eventparserType2.implementation.assemblyfilename = ChangeFilePath(eventparserType2.implementation.assemblyfilename, driversDirectoryPath);
				eventparserType2.implementation.tag = GetAbsolutePath(eventparserType2.implementation.tag, baseRelativePath);
			}
		}
		foreach (DeviceDefinition deviceDefinition in HardwareDefinition.DeviceDefinitions)
		{
			deviceDefinition.Base.implementation.assemblyfilename = ChangeFilePath(deviceDefinition.Base.implementation.assemblyfilename, driversDirectoryPath);
			deviceDefinition.Base.implementation.tag = GetAbsolutePath(deviceDefinition.Base.implementation.tag, baseRelativePath);
		}
	}

	public void UpdateImplementationDetailPathNamesToRelative(string driversDirectoryPath, string baseRelativePath)
	{
		HardwareDefinition.Base.implementation.assemblyfilename = ChangeFilePath(HardwareDefinition.Base.implementation.assemblyfilename, driversDirectoryPath);
		HardwareDefinition.Base.implementation.tag = GetRelativePath(HardwareDefinition.Base.implementation.tag, baseRelativePath);
		if (HardwareDefinition.Base.autodetect != null && HardwareDefinition.Base.autodetect.implementation != null)
		{
			HardwareDefinition.Base.autodetect.implementation.assemblyfilename = ChangeFilePath(HardwareDefinition.Base.autodetect.implementation.assemblyfilename, driversDirectoryPath);
			HardwareDefinition.Base.autodetect.implementation.tag = GetRelativePath(HardwareDefinition.Base.autodetect.implementation.tag, baseRelativePath);
		}
		if (HardwareDefinition.Base.eventparsers != null)
		{
			eventparserType[] eventparsers = HardwareDefinition.Base.eventparsers;
			foreach (eventparserType eventparserType2 in eventparsers)
			{
				eventparserType2.implementation.assemblyfilename = ChangeFilePath(eventparserType2.implementation.assemblyfilename, driversDirectoryPath);
				eventparserType2.implementation.tag = GetRelativePath(eventparserType2.implementation.tag, baseRelativePath);
			}
		}
		foreach (DeviceDefinition deviceDefinition in HardwareDefinition.DeviceDefinitions)
		{
			deviceDefinition.Base.implementation.assemblyfilename = ChangeFilePath(deviceDefinition.Base.implementation.assemblyfilename, driversDirectoryPath);
			deviceDefinition.Base.implementation.tag = GetRelativePath(deviceDefinition.Base.implementation.tag, baseRelativePath);
		}
	}

	private static string GetAbsolutePath(string relativePath, string baseFolder)
	{
		return Path.Combine(baseFolder, relativePath);
	}

	private static string GetRelativePath(string fullPath, string basePath)
	{
		if (!basePath.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
		{
			basePath += "\\";
		}
		Uri uri = new Uri(basePath);
		Uri uri2 = new Uri(fullPath);
		return uri.MakeRelativeUri(uri2).ToString().Replace("/", "\\");
	}

	public CapabilitiesCollection GetCapabilitiesByHwid(Hwid hwid)
	{
		if (hwid == null)
		{
			throw new ArgumentNullException("hwid");
		}
		if (TryGetCapabilitiesByHwid(hwid, out var capabilities))
		{
			return capabilities;
		}
		throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Capabilities with the specified hwid does not exist in the definition. Hwid '{0}'", hwid.Text), "hwid");
	}

	public bool TryGetCapabilitiesByHwid(Hwid hwid, out CapabilitiesCollection capabilities)
	{
		if (hwid == null)
		{
			throw new ArgumentNullException("hwid");
		}
		string text = hwid.Text;
		if (string.Equals(HardwareDefinition.Hwid.Text, text, StringComparison.OrdinalIgnoreCase))
		{
			capabilities = HardwareDefinition.Capabilities;
			return true;
		}
		if (HardwareDefinition.PtzSettings != null && string.Equals(HardwareDefinition.PtzSettings.Hwid.Text, text, StringComparison.OrdinalIgnoreCase))
		{
			capabilities = HardwareDefinition.PtzSettings.Capabilities;
			return true;
		}
		foreach (EventDefinition @event in HardwareDefinition.Events)
		{
			if (string.Equals(@event.Hwid.Text, text, StringComparison.OrdinalIgnoreCase))
			{
				capabilities = @event.Capabilities;
				return true;
			}
		}
		foreach (DeviceDefinition deviceDefinition in HardwareDefinition.DeviceDefinitions)
		{
			if (string.Equals(deviceDefinition.Hwid.Text, text, StringComparison.OrdinalIgnoreCase))
			{
				capabilities = deviceDefinition.Capabilities;
				return true;
			}
			if (deviceDefinition.PtzSettings != null && string.Equals(deviceDefinition.PtzSettings.Hwid.Text, text, StringComparison.OrdinalIgnoreCase))
			{
				capabilities = deviceDefinition.PtzSettings.Capabilities;
				return true;
			}
			foreach (StreamDefinition stream in deviceDefinition.Streams)
			{
				if (string.Equals(stream.Hwid.Text, text, StringComparison.OrdinalIgnoreCase))
				{
					capabilities = stream.Capabilities;
					return true;
				}
			}
			foreach (MetadataItemDefinition metadataItem in deviceDefinition.MetadataItems)
			{
				if (string.Equals(metadataItem.Hwid.Text, text, StringComparison.OrdinalIgnoreCase))
				{
					capabilities = metadataItem.Capabilities;
					return true;
				}
			}
		}
		capabilities = null;
		return false;
	}
}
